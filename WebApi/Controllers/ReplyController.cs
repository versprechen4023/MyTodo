using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
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
					await _dbContext.Replys.AddAsync(model);
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
	}
}
