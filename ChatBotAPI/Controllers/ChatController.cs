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
		public async Task<IActionResult> Send([FromBody] string message)
		{
			var result = await _chatService.Send("user1", message);
			return Ok(result);
		}
	}
}