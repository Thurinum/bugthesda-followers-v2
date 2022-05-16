using SessionProject2W5.Models;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;

namespace SessionProject2W5.Controllers
{
	public class FollowerManagerController : Controller
	{
		private readonly Database Database;

		public FollowerManagerController(Database database)
		{
			this.Database = database;
		}

		/// <summary>
		/// Obtient le formulaire de création de compagnion.
		/// </summary>
		/// <returns>La vue Create.</returns>
		[HttpGet]
		[Route("/gestionenfant/create")]
		[Route("/managefollowers/create")]
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

		/// <summary>
		/// Envoie les données du nouveau compagnion, crée ce dernier, et l'ajoute à la base de données.
		/// </summary>
		/// <param name="follower">Le compagnion retourné par le model binder</param>
		/// <returns>Une redirection vers la recherche (et non l'index).</returns>
		[Route("/gestionenfant/create")]
		[Route("/managefollowers/create")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Follower follower)
		{
			if (!ModelState.IsValid)
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

			int maxid = Database.Followers.Max(f => f.Id);
			Game parent = Database.Games[(int)follower.ParentId];
			Race followerRace = Database.SharedInfo.Races[(int)follower.RaceId];
			Class followerClass = Database.SharedInfo.Classes[(int)follower.ClassId];

			follower.Id = maxid + 1;
			follower.Parent = parent;
			follower.Race = followerRace;
			follower.Class = followerClass;
			follower.ShortName = follower.Name.Replace(" ", "").ToLower();
			
			parent.Followers.Add(follower);

			// mettre a jour la db
			Database.AddFollower(follower);

			return Redirect("/search");
		}

		/// <summary>
		/// Obtient le formulaire de suppression de compagnion.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("/{id:int}/delete")]
		[Route("gestionenfant/delete/{id:int}")]
		[Route("managefollowers/delete/{id:int}")]
		public ActionResult Delete(int id)
		{
			Follower follower = Database.Followers.Where(f => f.Id == id).SingleOrDefault();

			if (follower == null) 
				return View("404_FollowerNotFound", id.ToString());

			return View(follower);
		}

		/// <summary>
		/// Supprime le compagnion spécifié.
		/// </summary>
		/// <param name="id">L</param>
		/// <param name="collection"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("/{id:int}/delete")]
		[Route("gestionenfant/delete/{id:int}")]
		[Route("managefollowers/delete/{id:int}")]
		public ActionResult Delete(Follower fl)
		{
			Follower follower = Database.Followers.Where(f => f.Id == fl.Id).SingleOrDefault();

			if (follower == null)
				return View("500_GenericError");

			follower.Parent.Followers.Remove(follower);

			Database.DeleteFollower(fl.Id);

			return Redirect("/search");
		}
	}
}
