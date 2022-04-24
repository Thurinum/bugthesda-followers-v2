using Microsoft.AspNetCore.Mvc;
using SessionProject2W5.Models;

namespace SessionProject2W5.Controllers
{
	public class FavoriteController : Controller
	{	
		public FavoriteController(Database db)
		{
			this.database = db;
			ViewData["sDatabaseError"] = db.ErrorString; // Montrer certains messages d'erreur
		}

		[Route("/favoris")]
		[Route("/favorites")]
		public IActionResult Index()
		{
			return View(database.Favorites);
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

		private Database database;
	}
}
