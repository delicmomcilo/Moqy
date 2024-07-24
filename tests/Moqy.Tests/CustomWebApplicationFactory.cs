/*using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Moqy.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseEnvironment("Testing");
                
            });

            return base.CreateHost(builder);
        }
    }
}*/