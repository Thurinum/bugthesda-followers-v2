using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SessionProject2W5
{
	public class Program
	{
		static public void Main(string[] args)
		{
			IHostBuilder builder = Host.CreateDefaultBuilder(args);
			builder.ConfigureWebHostDefaults(
				builder => builder.UseStartup<Startup>()
			);
			builder.Build().Run();
		}
	}
}
