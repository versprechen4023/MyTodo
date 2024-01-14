using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.LinqExtenstion;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly ILogger<BoardController> _logger;
        private readonly DBContext _dbContext;

        public BoardController(ILogger<BoardController> logger, DBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBoardList(int pageNumber, int pageSize, int pageBlock)
        {
            _logger.LogWarning("게시글 가져오기 실행");
            try
            {
                // SELECT b.boardNo, b.BoardTitle, b.BoardContent, b.BoardDate, u.UserName
                // FROM Boards b JOIN Users u
                // ON (b.UserId = u.Id)
                var boardList = await _dbContext.Boards.Include(u => u.User).
                Select(b => new
                {
                    b.BoardNo,
                    b.BoardTitle,
                    b.BoardContent,
                    b.BoardDate,
                    b.BoardCount,
                    b.User.UserName
                }).
                OrderByDescending(b => b.BoardNo).
                ToPagingAsync(pageNumber, pageSize, pageBlock);

                return Ok(boardList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("{boardNo}")]
        public async Task<IActionResult> GetBoardDetail(int boardNo)
        {
            try
            {
                var boardDetail = await _dbContext.Boards.FirstOrDefaultAsync(b => b.BoardNo == boardNo);

                if (boardDetail.BoardCount == null)
                {
                    boardDetail.BoardCount = 1;
                }
                else
                {
                    boardDetail.BoardCount += 1;
                }

                await _dbContext.SaveChangesAsync();

                return Ok(boardDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PutBoard(Board model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Boards.Add(model);
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

        [HttpDelete("{boardNo}/{userId}")]
        public async Task<IActionResult> DeleteBoard(int boardNo, int userId)
        {
            try
            {
                var board = await _dbContext.Boards.FirstOrDefaultAsync(b => b.BoardNo == boardNo && b.UserId == userId);

                if (board == null)
                {
                    return BadRequest();
                }

                _dbContext.Boards.Remove(board);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Board model)
        {
            try
            {
                var detail = await _dbContext.Boards.FirstOrDefaultAsync(b => b.BoardNo == model.BoardNo && b.UserId == model.UserId);

                detail.BoardTitle = model.BoardTitle;
                detail.BoardContent = model.BoardContent;

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
