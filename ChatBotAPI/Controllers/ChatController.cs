using ChatBotAPI.Services;

using Microsoft.AspNetCore.Mvc;

namespace ChatBotAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChatController : ControllerBase
	{
		private readonly ChatService _chatService;

		public ChatController(ChatService chatService)
		{
			_chatService = chatService;
		}

		[HttpPost]
		public async Task<IActionResult> Send(Guid conversationId, [FromBody] string message)
		{
			var result = await _chatService.Send( conversationId , message);
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetMessages(Guid conversationId)
		{
			var messages = await _chatService.GetMessages(conversationId);
			return Ok(messages);
		}
	}
}