using SessionProject2W5.Models;
using SessionProject2W5.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SessionProject2W5.Controllers
{
	public class FollowerManagerController : Controller
	{
		private Database Database;

		public FollowerManagerController(Database database)
		{
			this.Database = database;
		}

		// GET: FollowerManagementController/Create
		[Route("/Test")]
		public ActionResult Create()
		{
			List<string> gameNames = new List<string>();

			foreach (Game game in Database.Games.OrderBy(g => g.Id))
				gameNames.Add(game.Name);

			ViewData["sGameNames"] = gameNames;
			return View();
		}

		// POST: FollowerManagementController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: FollowerManagementController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: FollowerManagementController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
