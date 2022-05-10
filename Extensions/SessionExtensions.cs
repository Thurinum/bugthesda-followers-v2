using Microsoft.AspNetCore.Http;
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
	}
}
