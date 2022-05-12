using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace SessionProject2W5.Models
{
	/// <summary>
	/// Un jeu, soit une représentation contenant des compagnions (Followers).
	/// </summary>
	public class Game
	{
		#region Proprietes
		/// <summary>
		/// L'ID numérique (PK) du jeu
		/// </summary>
		public int Id;

		/// <summary>
		/// Le nom du jeu
		/// </summary>
		public string Name;

		/// <summary>
		/// Le nom court du jeu. Il s'agit d'un nom sans espace et en miniscules utilisé pour former 
		/// les chemins d'image et dans certaines routes.
		/// </summary>
		public string ShortName;

		/// <summary>
		/// L'url de la video du jeu
		/// </summary>
		public string BackgroundUrl { get; set; }

		/// <summary>
		/// La description du jeu.
		/// </summary>
		public string Description;

		/// <summary>
		/// Le slogan du jeu.
		/// </summary>
		public string Tagline;

		/// <summary>
		/// L'année de sortie du jeu.
		/// </summary>
		public int YearReleased;

		/// <summary>
		/// Le Game Director. Probablement Todd Howard.
		/// </summary>
		public string Director;

		/// <summary>
		/// La couleur du thème du jeu. Utilisée pour styler le CSS.
		/// </summary>
		public string Color;
		#endregion

		#region Collections
		/// <summary>
		/// Une liste de faits à propos du jeu.
		/// </summary>
		public List<string> Facts;

		/// <summary>
		/// La liste des compagnions du jeu.
		/// </summary>
		public List<Follower> Followers;

		/// <summary>
		/// Référence aux Informations Partagées par la base de données
		/// </summary>
		public SharedInfo SharedInfo;
		#endregion

		/// <summary>
		/// Instantie un nouveau jeu
		/// </summary>
		public Game()
		{
			Facts = new List<string>();
			Followers = new List<Follower>();
		}
	}
}
