using Microsoft.AspNetCore.Mvc;
using SessionProject2W5.Models;

namespace SessionProject2W5.Controllers
{
	public class FavoriteController : Controller
	{
		private readonly Database Database;

		public FavoriteController(Database db)
		{
			this.Database = db;
		}

		[Route("/favoris")]
		[Route("/favorites")]
		public IActionResult Index()
		{
			return View(Database.Favorites);
		}

		[Route("/favoris/{favoriteid}")]
		[Route("/favorites/{favoriteid}")]
		public IActionResult Details(int favoriteid)
		{
			return View();
		}

		public IActionResult Add()
		{
			return View();
		}

		public IActionResult Remove()
		{
			return View();
		}
	}
}
