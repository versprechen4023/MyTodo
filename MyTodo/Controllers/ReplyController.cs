using Microsoft.AspNetCore.Mvc;
using MyTodo.Utils;
using WebApi.Models;

namespace MyTodo.Controllers
{
    // Reply/Action
    [Route("[controller]/[action]")]
    public class ReplyController : Controller
    {
        // 의존성주입 API사용을 위한 객체, 쿠키접근을 위한 Accessor, 로거
        private readonly WebAPIs _webAPIs;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ReplyController> _logger;

        public ReplyController(WebAPIs webAPIs, IHttpContextAccessor httpContextAccessor, ILogger<ReplyController> logger)
        {
            _webAPIs = webAPIs;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Put(Reply model)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];

                // 로그인 유무 토큰 검증
                if (_webAPIs.IsTokenExpired(token))
                {
                    if (!await _webAPIs.RefreshToken())
                    {
                        TempData["returnURL"] = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

                        return Unauthorized();
					}
                }

                // 토큰에서 유저 번호 추출
                model.UserId = Int32.Parse(JwtDecoder.GetUserIdFromClaims(token));
                // 헤더에 토큰 세팅
                _webAPIs.SetAccessToken(token);

                var response = await _webAPIs.PutReply(model);

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }

			catch(Exception ex)
            {
				_logger.LogError(ex, "댓글 등록 실패");
				return BadRequest();
			}
		}

        [HttpDelete("{ReplyNo}")]
        public async Task<IActionResult> Delete(int replyNo)
        {
            try
            {
				var token = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];

				// 로그인 유무 토큰 검증
				if (_webAPIs.IsTokenExpired(token))
				{
					if (!await _webAPIs.RefreshToken())
					{
						TempData["returnURL"] = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

						return Unauthorized();
					}
				}

				// 토큰에서 유저 번호 추출
				int userId = Int32.Parse(JwtDecoder.GetUserIdFromClaims(token));
				// 헤더에 토큰 세팅
				_webAPIs.SetAccessToken(token);

				var response = await _webAPIs.DeleteReply(replyNo, userId);

				if (response.IsSuccessStatusCode)
				{
					return Ok();
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "댓글 삭제 실패");
				return BadRequest();
			}
		}
    }
}
