﻿using SessionProject2W5.Models;
using SessionProject2W5.ViewModels;

using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace SessionProject2W5.Controllers
{
	public class FollowerController : Controller
	{
		private Database Database;

		public FollowerController(Database database)
		{
			this.Database = database;
			ViewData["sDatabaseError"] = database.ErrorString; // Montrer certains messages d'erreur
		}

		#region Lister compagnions
		// Affiche page de recherche avec les compagnions de TOUS les jeux
		[Route("/search")]
		public IActionResult Search()
		{
			SearchViewModel search = new SearchViewModel();
			SearchCriteriaViewModel searchCriteria = new SearchCriteriaViewModel();
			List<Follower> followers = new List<Follower>();

			foreach (Game game in this.Database.Games)
			{
				searchCriteria.GamesFilters.Add(new TypeFilterViewModel()
				{
					Name = game.ShortName,
					Allowed = true
				});
				followers.AddRange(game.Followers);
			}

			foreach (Race race in Database.SharedInfo.Races)
				searchCriteria.RacesFilters.Add(new TypeFilterViewModel()
				{
					Name = race.ShortName,
					Allowed = true
				});

			foreach (Datum cls in Database.SharedInfo.Classes)
				searchCriteria.ClassesFilters.Add(new TypeFilterViewModel()
				{
					Name = cls.ShortName,
					Allowed = true
				});

			search.Criteria = searchCriteria;
			search.Results = followers;

			ViewData["sPageTitle"] = "Recherche de follower";

			return followers.Count > 0 ? View(search) : View("404_SearchNoResults", searchCriteria.Keywords);
		}

		// Filtrer la liste des compagnions en fonction du CriteriaViewModel
		// je l'ai mis comme un overload c'est plus simple et logique
		[Route("/search/filter")]
		public IActionResult Search(SearchCriteriaViewModel criteria)
		{
			// récupérer les données
			List<Follower> followers = new List<Follower>();
			foreach (Game game in this.Database.Games)
				followers.AddRange(game.Followers);

			// filtrer les données
			for (int i = 0; i < followers.Count; i++)
			{
				Follower follower = followers[i];
				//return Content($"{criteria.MinHitpoints} | {follower.Hitpoints}");
				if ( // TODO ADD case sensitive option
					(criteria.Keywords != null && !follower.Name.ToLower().Contains(criteria.Keywords.ToLower())) ||
					(criteria.GamesFilters.Where(f => f.Name == follower.Parent.ShortName).First().Allowed == false) ||
					(criteria.RacesFilters.Where(f => f.Name == follower.Race.ShortName).First().Allowed == false) ||
					(criteria.ClassesFilters.Where(f => f.Name == follower.Class.ShortName).First().Allowed == false) ||
					(criteria.FavoriteFilter != FavoriteFilter.Ignore && ((follower.IsFavorite ? FavoriteFilter.Favorite : FavoriteFilter.NotFavorite) != criteria.FavoriteFilter)) ||
					(criteria.ProtectionFilter != ProtectionFilter.Ignore && ((follower.IsEssential ? ProtectionFilter.Essential : (follower.DoesRespawn ? ProtectionFilter.Protected : ProtectionFilter.None)) != criteria.ProtectionFilter)) || // "true production code"
					(criteria.MinAlignment != null && (follower.Alignment < criteria.MinAlignment)) ||
					(criteria.MaxAlignment != null && (follower.Alignment > criteria.MaxAlignment)) ||
					(criteria.MinHitpoints != null && (follower.Hitpoints < criteria.MinHitpoints)) ||
					(criteria.MaxHitpoints != null && (follower.Hitpoints > criteria.MaxHitpoints)) ||
					(criteria.MinEnergy != null && (follower.Energy < criteria.MinEnergy)) ||
					(criteria.MaxEnergy != null && (follower.Energy > criteria.MaxEnergy))
				)
				{
					followers.Remove(follower);
					i--;
				}
			}

			// construire le modèle des résultats
			SearchViewModel search = new SearchViewModel();
			search.Criteria = criteria;
			search.Results = followers;

			ViewData["sPageTitle"] = "Recherche de follower";
			return followers.Count > 0 ? View(search) : View("404_SearchNoResults", criteria.Keywords);
		}

		// Affiche page de recherche avec les compagnions d'UN jeu specifique
		[Route("/search/{name}/")]
		[Route("/recherche/{name}/")]
		public IActionResult Search(string name)
		{
			Game game = Database.Games.Where(g => g.ShortName == name).FirstOrDefault();

			if (game == null)
				return View("404_GameNotFound", name);

			if (game.Followers.Count == 0)
				return View("204_GameEmpty", game.Name);

			ViewData["sPageTitle"] = "Recherche de follower pour " + game.Name;
			return View(game.Followers.Where(f => f.Parent.ShortName == name).ToList());
		}

		// Affiche page de recherche avec les compagnions d'UN jeu specifique (par ID)
		[Route("/search/{id:int}/")]
		[Route("/recherche/{id:int}/")]
		public IActionResult Search(int id)
		{
			Game game = Database.Games[id];

			if (game == null)
				return PartialView("404_GameNotFound", id);

			if (game.Followers.Count == 0)
				return PartialView("204_GameEmpty", game.Name);

			ViewData["sPageTitle"] = "Recherche de follower pour " + game.Name;
			return View(game.Followers.Where(f => f.Parent.Id == id).ToList());
		}
		#endregion

		#region Afficher details d'un compagnion
		// Affiche les details d'un compagnion selon son nom (supporte plusieurs routes)
		[Route("/{name}")]
		[Route("/details/{name}")]

		[Route("/follower/{name}")]
		[Route("/follower/details/{name}")]

		[Route("/enfant/{name}")]
		[Route("/enfant/detail/{name}")]
		[Route("/enfant/details/{name}")]
		public IActionResult Details(string name)
		{
			Follower follower = Database.Followers.Where(f => f.ShortName == name.ToLower()).FirstOrDefault();

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(null, name));

			ViewData["sPageTitle"] = $"{follower.Parent.Name} - {follower.Name}";
			return View(follower);
		}

		// Affiche les details d'un compagnion selon son id (supporte plusieurs routes)
		[Route("/{id:int}")]
		[Route("/details/{id:int}")]

		[Route("/follower/{id:int}")]
		[Route("/follower/details/{id:int}")]

		[Route("/enfant/{id:int}")]
		[Route("/enfant/detail/{id:int}")]
		[Route("/enfant/details/{id:int}")]
		public IActionResult Details(int id)
		{
			Follower follower = Database.Followers[id];

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(null, id.ToString()));

			ViewData["sPageTitle"] = $"{follower.Parent.Name} - {follower.Name}";
			return View(follower);
		}

		// Affiche les details d'un compagnion selon le chemin parent/enfant
		[Route("/{gamename}/{followername}")]
		public IActionResult Details(string gamename, string followername)
		{
			Game game = Database.Games.Where(g => g.ShortName == gamename.ToLower()).FirstOrDefault();
			if (game == null)
				return View("204_GameEmpty");

			Follower follower = game.Followers.Where(f => f.ShortName == followername.ToLower()).FirstOrDefault();
			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(game.Name, followername));

			ViewData["sPageTitle"] = $"{game.Name} - {follower.Name}";
			return View(follower);
		}

		// Affiche les details d'un compagnion selon le chemin parent/enfant (par ID)
		[Route("/{gameid:int}/{followerid:int}")]
		public IActionResult Details(int gameid, int followerid)
		{
			Follower follower = Database.Games[gameid].Followers[followerid];

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(gameid.ToString(), followerid.ToString()));

			ViewData["sPageTitle"] = "Recherche du follower " + follower.Name;
			return View(follower);
		}
		#endregion
	}
}
