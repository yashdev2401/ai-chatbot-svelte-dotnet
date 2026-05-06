using ChatBotAPI.Models;

using Microsoft.EntityFrameworkCore;

using System.Text;

namespace ChatBotAPI.Services
{
	public class ChatService
	{
		private readonly AppDbContext _db;
		private readonly OllamaService _ollama;

		public ChatService(AppDbContext db, OllamaService ollama)
		{
			_db = db;
			_ollama = ollama;
		}

		public async Task<string> Send(string userId, string message)
		{
			// Save user message
			var userMessage = new ChatMessage
			{
				UserId = userId,
				Role = "user",
				Content = message,
				CreatedAt = DateTime.UtcNow
			};

			_db.Messages.Add(userMessage);
			await _db.SaveChangesAsync();

			// Get last 10 messages
			var history = await _db.Messages.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).Take(10).OrderBy(x => x.CreatedAt).ToListAsync();

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

			// AI Response
			var aiReply = await _ollama.Generate(finalPrompt);

			// Save assistant response
			var assistantMessage = new ChatMessage
			{
				UserId = userId,
				Role = "assistant",
				Content = aiReply,
				CreatedAt = DateTime.UtcNow
			};

			_db.Messages.Add(assistantMessage);
			await _db.SaveChangesAsync();

			return aiReply;
		}
	}
}