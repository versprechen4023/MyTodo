using Microsoft.AspNetCore.Mvc;
using MyTodo.Models;
using MyTodo.Utils;
using System.Diagnostics;

namespace MyTodo.Controllers
{
    public class HomeController : Controller
    {
		// ���������� API����� ���� ��ü, ��Ű������ ���� Accessor, �ΰ�
		private readonly WebAPIs _webAPIs;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<HomeController> _logger;

        public HomeController(WebAPIs webAPIs, IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger)
        {
			_webAPIs = webAPIs;
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
		}

        public async Task<IActionResult> Index()
        {
            try
            {
				var token = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];

				// �α��� ���� ��ū ����
				if (_webAPIs.IsTokenExpired(token))
				{
					if (!await _webAPIs.RefreshToken())
					{
						TempData["returnURL"] = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

						return View();
					}
				}

				return RedirectToAction("Index", "Todo");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ȩ ���� ���� �߻�");
				return RedirectToAction("Error500", "Error");
			}
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
