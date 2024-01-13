using Microsoft.AspNetCore.Mvc;
using MyTodo.Utils;

namespace MyTodo.Controllers
{
    public class UploadController : Controller
    {
        private readonly WebAPIs _webAPIs;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UploadController> _logger;


        // 의존성주입 API사용을 위한 객체, 쿠키접근을 위한 Accessor, 경로얻기위한 webHost,  로거
        public UploadController(WebAPIs webApis, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, ILogger<UploadController> logger)
        {
            _webAPIs = webApis;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [HttpPost, Route("api/upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];

                // 로그인 유무 토큰 검증
                if (_webAPIs.IsTokenExpired(token))
                {
                    if (!await _webAPIs.RefreshToken())
                    {
                        return RedirectToAction("Login", "User");
                    }
                }

                // 헤더에 토큰 세팅
                _webAPIs.SetAccessToken(token);

                // 업로드 권한있는지 검증
                var auth = await _webAPIs.GetUserAction();

                if (!auth.IsSuccessStatusCode)
                {
                    _logger.LogWarning("권한 없음");
                    return BadRequest();
                }

                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("파일 없음");
                    return BadRequest();
                }

                // trumbowyg 업로드 통신 데이터타입 검증
                var type = file.ContentType;

                string[] imageTypes = ["image/jpeg", "image/png", "image/gif"];

                if (!Array.Exists(imageTypes, t => t.Equals(type, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogWarning("확장자 불허용");
                    return BadRequest();
                }

                // 주소 wwwroot/images/upload
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images/upload");

                // 폴더없으면 생성
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // 파일명
                var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;


                // 파일 저장
                using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { file = "/images/upload/" + fileName, success = true });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
