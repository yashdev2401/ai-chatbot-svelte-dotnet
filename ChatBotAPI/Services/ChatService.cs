using ChatBotAPI.Models;

using Microsoft.EntityFrameworkCore;

using System.Text;

namespace ChatBotAPI.Services
{
	public class ChatService
	{
		private readonly AppDbContext _db;
		private readonly OllamaService _ollama;
		private readonly EmbeddingService _embeddingService;
		private readonly VectorDbService _vectorDbService;

		public ChatService(
			AppDbContext db,
			OllamaService ollama,
			EmbeddingService embeddingService,
			VectorDbService vectorDbService)
		{
			_db = db;
			_ollama = ollama;
			_embeddingService = embeddingService;
			_vectorDbService = vectorDbService;
		}
		public async Task<string> Send( Guid conversationId, string message)
		{
			try {
				Console.WriteLine("Saving user message...");

				// Save user message
				var userMessage = new ChatMessage
				{
					ConversationId = conversationId,
					Role = "user",
					Content = message,
					CreatedAt = DateTime.UtcNow
				};

				_db.Messages.Add(userMessage);
				await _db.SaveChangesAsync();

				Console.WriteLine("Generating embedding...");

				// Generate embedding
				var embedding = await _embeddingService
					.GenerateEmbedding(message);

				Console.WriteLine("Searching vector DB...");

				// Search semantic memory
				var cachedAnswer = await _vectorDbService
					.SearchMemory(conversationId,embedding);

				// Cache hit
				if (!string.IsNullOrEmpty(cachedAnswer))
				{
					Console.WriteLine("CACHE HIT - Returning from Qdrant");
					return cachedAnswer;
				}

				Console.WriteLine("No cache found.");

				// Get last messages
				var history = await _db.Messages
					.Where(x =>x.ConversationId == conversationId)
					.OrderByDescending(x => x.CreatedAt)
					.Take(10)
					.OrderBy(x => x.CreatedAt)
					.ToListAsync();

				Console.WriteLine("Building prompt...");

				// Build prompt
				var promptBuilder = new StringBuilder();

				promptBuilder.AppendLine(
					"You are a helpful AI assistant. Answer clearly and properly formatted."
				);

				foreach (var msg in history)
				{
					promptBuilder.AppendLine($"{msg.Role}: {msg.Content}");
				}

				promptBuilder.AppendLine("assistant:");

				var finalPrompt = promptBuilder.ToString();

				Console.WriteLine("Calling Ollama...");

				// AI response
				var aiReply = await _ollama.Generate(finalPrompt);

				Console.WriteLine("Saving vector memory...");

				// Save semantic memory
				await _vectorDbService.SaveMemory(
					conversationId,
					message,
					aiReply,
					embedding
				);
				Console.WriteLine("Saving assistant message...");

				// Save assistant response
				var assistantMessage = new ChatMessage
				{
					ConversationId = conversationId,
					Role = "assistant",
					Content = aiReply,
					CreatedAt = DateTime.UtcNow
				};

				_db.Messages.Add(assistantMessage);
				await _db.SaveChangesAsync();

				Console.WriteLine("Response completed.");
				return aiReply;
			}
			catch (Exception e) {
				Console.WriteLine("Some thing went wrong" , e);
				throw;
			}
		}
	}
}