using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using WebApi;
using WebApi.Dtos;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace XUnitTestProject1
{
    #region snippet1
    public class Test : IClassFixture<CustomWebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;


        public Test(CustomWebApplicationFactory<Startup> factory)
        {

            var contentRoot = "C:\\Users\\Allan\\source\\repos\\Digi-Pet\\BackEnd\\BackEnd\\";

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(contentRoot)
                .AddJsonFile("appsettings.json");

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                //.ConfigureServices(InitializeServices)
                .UseConfiguration(configurationBuilder.Build())

                .UseEnvironment("Development")
                .UseStartup<Startup>();

           var  _testServer = new TestServer(builder);

            _client = _testServer.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:9999");
        }


        [Fact]
        public void DummyTest()
        {
            Assert.Equal(.0, .0, 0);
        }
        [Theory]
        [InlineData("/users/authenticate")]
        //[InlineData("/Index")]
        public async Task Authenticate(string url)
        {
            var user = new { UserName = "admin", Password = "12345678" };
             var formContent = new FormUrlEncodedContent(new[]
             {
                 new KeyValuePair<string, string>("UserName", "admin"),
                 new KeyValuePair<string, string>("Password", "12345678")
             });
             var response = await _client.PostAsync(url,formContent);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            var parsed = JsonConvert.DeserializeObject<AuthedUser>(await response.Content.ReadAsStringAsync());
            Assert.Equal( "admin", parsed.UserName);
            Assert.Equal("admin@digipet.com", parsed.Email);
            Assert.NotNull(parsed.Token);
        }
        #endregion
    }
}
