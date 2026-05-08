using System.Text;
using System.Text.Json;

namespace ChatBotAPI.Services
{
	public class EmbeddingService
	{
		private readonly HttpClient _httpClient;

		public EmbeddingService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<float[]> GenerateEmbedding(string text)
		{
			var request = new
			{
				model = "nomic-embed-text",
				prompt = text
			};

			var json = JsonSerializer.Serialize(request);

			var response = await _httpClient.PostAsync(
				"http://192.168.1.240:11434/api/embeddings",
				new StringContent(json, Encoding.UTF8, "application/json")
			);

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsStringAsync();

			using var document = JsonDocument.Parse(result);

			var embeddingArray = document.RootElement
				.GetProperty("embedding");

			var vector = embeddingArray
				.EnumerateArray()
				.Select(x => x.GetSingle())
				.ToArray();

			return vector;
		}
	}
}