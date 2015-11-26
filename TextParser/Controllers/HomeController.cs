using System.Web.Mvc;

// MVC Controller
// Display home page
namespace TextParser.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}