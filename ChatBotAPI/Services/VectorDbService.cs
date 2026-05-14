using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace ChatBotAPI.Services
{
	public class VectorDbService
	{
		private readonly QdrantClient _client;

		private const string CollectionName = "chat-memory";

		public VectorDbService()
		{
			_client = new QdrantClient("localhost", 6334);
		}

		public async Task InitializeCollection()
		{
			var collections = await _client.ListCollectionsAsync();

			var exists = collections.Any(x => x == CollectionName);
			if (!exists)
			{
				await _client.CreateCollectionAsync(
					collectionName: CollectionName,
					vectorsConfig: new VectorParams
					{
						Size = 768,
						Distance = Distance.Cosine
					}
				);
			}
		}

		public async Task SaveMemory(Guid conversationId, string question, string answer, float[] embedding)
		{
			var point = new PointStruct
			{
				Id = new PointId
				{
					Uuid = Guid.NewGuid().ToString()
				},
				Vectors = embedding,
				Payload =
				{
					["conversationId"] = conversationId.ToString(),
					["question"] = question,
					["answer"] = answer
				}
			};

			await _client.UpsertAsync(
				collectionName: CollectionName,
				points: new[] { point }
			);
		}

		public async Task<string?> SearchMemory(Guid conversationId, float[] embedding)
		{
			var searchResult = await _client.SearchAsync(
				collectionName: CollectionName,
				vector: embedding,
				limit: 1,
				filter: new Filter
				{
					Must =
					{
						new Condition
						{
							Field = new FieldCondition
							{
									Key = "conversationId",
									Match = new Match
									{
											Keyword = conversationId.ToString()
										}
							}
						}
					}
				}
			);
			var match = searchResult.FirstOrDefault();

			if (match != null && match.Score > 0.90)
			{
				return match.Payload["answer"].StringValue;
			}

			return null;
		}
	}
}