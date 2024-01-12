using Microsoft.AspNetCore.Mvc;
using MyTodo.Models;
using MyTodo.Utils;
using Newtonsoft.Json;
using WebApi.Dtos;
using WebApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyTodo.Controllers
{
    //User/Action
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        // 의존성주입 API사용을 위한 객체, 쿠키접근을 위한 Accessor, 로거
        private readonly WebAPIs _webAPIs;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;

        public UserController(WebAPIs webAPIs, IHttpContextAccessor httpContextAccessor, ILogger<UserController> logger)
        {
            _webAPIs = webAPIs;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            try
            {
                var response = await _webAPIs.Login(model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<TokenDto>(result);

                    // 쿠키에 토큰 저장
                    Response.Cookies.Append("AccessToken", token.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });

                    Response.Cookies.Append("RefreshToken", token.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorViewModel>(result);

                    ModelState.AddModelError("Login", error.Message);
                    return View();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "로그인에 실패하였습니다.");
                ModelState.AddModelError("Login", "로그인에 실패하였습니다.");

                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            try
            {
                var response = await _webAPIs.Register(model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("login");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorViewModel>(result);

                    ModelState.AddModelError("Register", error.Message);
                    return View();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "회원가입 실패");
                ModelState.AddModelError("Register", "회원가입중에 문제가 발생했습니다");

                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // 쿠키에 있는 토큰들 다 삭제
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("AccessToken");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("RefreshToken");

            return RedirectToAction("Index", "Home");
        }
    }
}
