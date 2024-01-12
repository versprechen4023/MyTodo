using Microsoft.AspNetCore.Mvc;
using MyTodo.Utils;
using WebApi.Dtos;

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
                    return View(new List<BoardDto>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "게시글 리스트 불러오기 실패");
                return View(new List<BoardDto>());
            }
           
        }
    }
}
