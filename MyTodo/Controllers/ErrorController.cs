using Microsoft.AspNetCore.Mvc;

namespace MyTodo.Controllers
{
	public class ErrorController : Controller
	{
		public IActionResult Error500()
		{
			return View();
		}
	}
}
