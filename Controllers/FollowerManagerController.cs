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

		// GET: FollowerManagerController/Create
		[Route("/Test")]
		public ActionResult Create()
		{
			List<string> gameNames = new List<string>();
			List<string> raceNames = new List<string>();
			List<string> classNames = new List<string>();

			foreach (Game game in Database.Games.OrderBy(g => g.Id))
				gameNames.Add(game.Name);

			foreach (Race race in Database.SharedInfo.Races.OrderBy(g => g.Id))
				raceNames.Add(race.CommonName);

			foreach (Class @class in Database.SharedInfo.Classes.OrderBy(g => g.Id))
				classNames.Add(@class.Name);

			ViewData["sGameNames"] = gameNames;
			ViewData["sRaceNames"] = raceNames;
			ViewData["sClassNames"] = classNames;
			return View();
		}

		// POST: FollowerManagerController/Create
		[Route("/Test")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Follower follower)
		{
			List<string> gameNames = new List<string>();
			List<string> raceNames = new List<string>();
			List<string> classNames = new List<string>();

			foreach (Game game in Database.Games.OrderBy(g => g.Id))
				gameNames.Add(game.Name);

			foreach (Race race in Database.SharedInfo.Races.OrderBy(g => g.Id))
				raceNames.Add(race.CommonName);

			foreach (Class @class in Database.SharedInfo.Classes.OrderBy(g => g.Id))
				classNames.Add(@class.Name);

			ViewData["sGameNames"] = gameNames;
			ViewData["sRaceNames"] = raceNames;
			ViewData["sClassNames"] = classNames;

			if (!ModelState.IsValid)
				return View();

			int maxid = Database.Followers.Max(f => f.Id) + 1;

			Game parent = Database.Games[follower.Id];
			follower.Parent = parent;
			parent.Followers.Add(follower);

			return RedirectToAction("Index", "Game");
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
