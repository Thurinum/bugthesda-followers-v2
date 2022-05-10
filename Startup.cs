using SessionProject2W5.Models;

using System.Xml;
using System.Runtime.Serialization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SessionProject2W5
{
	public class Startup
	{
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage(); // activer la page d'erreur
				app.UseStaticFiles(new StaticFileOptions
				{
					// desactiver cache pendant developpement pour prendre en compte
					// changements aux ressources (j'imagine...)
					OnPrepareResponse = 
						context => context.Context.Response.Headers.Add("Cache-Control", "no-cache")
				});
			} else
			{
				app.UseStaticFiles();
			}

			// activer et configurer le routage
			app.UseSession();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action}/{id?}",
					defaults: new
					{
						controller = "Home",
						action = "Index"
					}
				);
			});
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDistributedMemoryCache();
			services.AddSession(options => { options.IdleTimeout = System.TimeSpan.FromMinutes(20); });
			services.AddMvc().AddRazorRuntimeCompilation();
			services.AddSingleton<Database>(new Database("wwwroot/data.xml"));
		}
	}
}
