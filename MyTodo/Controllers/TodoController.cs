using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTodo.Utils;
using WebApi.Models;

namespace MyTodo.Controllers
{
    //Todo/Action
    [Route("[controller]/[action]")]
    public class TodoController : Controller
    {
        // 의존성주입 API사용을 위한 객체, 쿠키접근을 위한 Accessor, 로거
        private readonly WebAPIs _webAPIs;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TodoController> _logger;

        public TodoController(WebAPIs webAPIs, IHttpContextAccessor httpContextAccessor, ILogger<TodoController> logger)
        {
            _webAPIs = webAPIs;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTodoList()
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

                var response = await _webAPIs.GetTodoList(userId);

                if (response.IsSuccessStatusCode)
                {
                    var todoList = await response.Content.ReadFromJsonAsync<List<Todo>>();
                    return Ok(todoList);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "투두 리스트 호출 실패");
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Put(Todo model)
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

                var response = await _webAPIs.PutTodo(model);

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
                _logger.LogError(ex, "투두리스트 작성 실패");
                return BadRequest();
            }

        }
        [HttpDelete("{todoNo}")]
        public async Task<IActionResult> Delete(int todoNo)
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

                var response = await _webAPIs.DeleteTodo(todoNo, userId);

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
                _logger.LogError(ex, "투두리스트 삭제 실패");
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Todo model)
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

                var response = await _webAPIs.UpdateTodo(model);

                if(response.IsSuccessStatusCode)
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
                _logger.LogError(ex, "투두리스트 수정 실패");
                return BadRequest();
            }
        }
    }
}
