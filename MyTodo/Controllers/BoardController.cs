using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyTodo.Utils;
using System.Security.Cryptography.X509Certificates;
using WebApi.Dtos;
using WebApi.Models;

namespace MyTodo.Controllers
{
    //User/Action
    [Route("[controller]/[action]")]
    public class BoardController : Controller
    {
        // 의존성주입 API사용을 위한 객체, 쿠키접근을 위한 Accessor, 로거
        private readonly WebAPIs _webAPIs;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<BoardController> _logger;

        public BoardController(WebAPIs webAPIs, IHttpContextAccessor httpContextAccessor, ILogger<BoardController> logger)
        {
            _webAPIs = webAPIs;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// pageSize는 페이지당 보여줄 글의수
        /// pageBlock은 화면에서 보여줄 페이징 버튼의 수
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageBlock"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber, int pageSize = 10, int pageBlock = 10)
        {
            try
            {
                if (pageNumber == null)
                {
                    pageNumber = 1;
                }

                var response = await _webAPIs.GetBoardList(pageNumber, pageSize, pageBlock);

                if (response.IsSuccessStatusCode)
                {
                    var boardList = response.Content.ReadFromJsonAsync<PagingDto<BoardDto>>().Result;

                    return View(boardList);
                }
                else
                {
                    return RedirectToAction("Error500", "Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "게시글 리스트 불러오기 실패");
                return RedirectToAction("Error500", "Error");
            }
           
        }

        [HttpGet("{boardNo}")]
        public async Task<IActionResult> Detail(int boardNo)
        {
            try
            {
                var response = await _webAPIs.GetBoardDetail(boardNo);

                if(response.IsSuccessStatusCode)
                {
                    var boardDetail = await response.Content.ReadFromJsonAsync<BoardDto>();

                    var token = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];

                    if (!token.IsNullOrEmpty())
                    {
                        boardDetail.Id = Int32.Parse(JwtDecoder.GetUserIdFromClaims(token));
                        return View(boardDetail);
                    }
                    else
                    {
                        return View(boardDetail);
                    }
                }

                return RedirectToAction("Error500", "Error");

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "게시글 불러오기 실패");
                return RedirectToAction("Error500", "Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Put()
        {
			var token = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];

            // 로그인 유무 토큰 검증
            if (_webAPIs.IsTokenExpired(token))
            {
				if (!await _webAPIs.RefreshToken())
				{
                    TempData["returnURL"] = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

                    return RedirectToAction("Login", "User");
				}
			}

            return View();
		}

        [HttpPost]
        public async Task<IActionResult> Put(Board model)
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

                        return RedirectToAction("Login", "User");
					}
				}

				// 토큰에서 유저 번호 추출
				model.UserId = Int32.Parse(JwtDecoder.GetUserIdFromClaims(token));
                // 헤더에 토큰 세팅
                _webAPIs.SetAccessToken(token);

                var response = await _webAPIs.PutBoard(model);

                if(response.IsSuccessStatusCode)
                {
					return RedirectToAction("Index", "Board");
				} 
                else
                {
                    return RedirectToAction("Error500", "Error");
				}

                
			}
            catch(Exception ex)
            {
				_logger.LogError(ex, "게시글 등록 실패");
                return RedirectToAction("Error500", "Error");
            }
		}
    }
}
