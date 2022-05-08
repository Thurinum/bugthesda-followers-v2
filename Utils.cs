using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionProject2W5.Utils
{
	public class Utils
	{
		// TODO: Handle French plural
		public static string Plural(string input, int count)
		{
			return count > 1 ? input + "s" : input;
		}
	}
}
