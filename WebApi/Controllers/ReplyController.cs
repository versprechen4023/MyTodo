using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ReplyController : ControllerBase
	{
		// 의존성 주입 로거, DB컨텍스트
		private readonly ILogger<ReplyController> _logger;
		private readonly DBContext _dbContext;

		public ReplyController(ILogger<ReplyController> logger, DBContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		[HttpPost]
		public async Task<IActionResult> PutReply(Reply model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					await _dbContext.Replies.AddAsync(model);
					if (await _dbContext.SaveChangesAsync() > 0)
					{
						return Ok();
					}
					else
					{
						return BadRequest();
					}
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest();
			}
		}

		[HttpDelete("{replyNo}/{userId}")]
		public async Task<IActionResult> DeleteReply(int replyNo, int userId)
		{
			try
			{
				var reply = await _dbContext.Replies.FirstOrDefaultAsync(r => r.ReplyNo == replyNo && r.UserId == userId); 

				if(reply == null)
				{
					return BadRequest();
				}

				_dbContext.Replies.Remove(reply);

				if (await _dbContext.SaveChangesAsync() > 0)
				{
					return Ok();
				} else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest();
			}
		}
	}
}
