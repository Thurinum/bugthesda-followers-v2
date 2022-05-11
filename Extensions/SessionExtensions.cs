using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace SessionProject2W5.Extensions
{
	public static class SessionExtensions
	{
		/// <summary>
		/// Templated method pour obtenir une variable de session de n'importe quel type.
		/// Utilise JSON pour sérialiser/déseérialiser les données
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="session"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		/// <summary>
		/// Templated method pour set une variable de session de n'importe quel type.
		/// Utilise JSON pour sérialiser/déseérialiser les données
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="session"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default : JsonSerializer.Deserialize<T>(value);
		}

		/// <summary>
		/// Obtient par réflection la valeur de la valeur Name de l'attribut Display d'un enum selon une de ses valeurs.
		/// Basé sur l'implémentation donnée ici 
		/// https://stackoverflow.com/questions/13099834/how-to-get-the-display-name-attribute-of-an-enum-member-via-mvc-razor-code
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns>La valeur du nom display.</returns>
		public static string GetEnumDisplay<T>(T value)
		{
			return value.GetType().GetMember(value.ToString()).Single().GetCustomAttribute<DisplayAttribute>().Name;
		}
	}
}
