﻿using System.Collections.Generic;
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
    
    public class Test 
    {

        private readonly HttpClient _client;


        public Test()
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
            response.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var parsed = JsonConvert.DeserializeObject<AuthedUser>(await response.Content.ReadAsStringAsync());

            Assert.Equal( "admin", parsed.UserName);
            Assert.Equal("admin@digipet.com", parsed.Email);
            Assert.NotNull(parsed.Token);
        }
        [Fact]
        public async Task CreateNewWalker__AndThenDeleteIt()
        {
            var url = "/api/walkers";

            string[] otherProvinces = { "Heredia", "Alajuela" };
            var obj = new
            {
                SchoolId = "190043530",
                Password = "12345678",
                Name = "Bernardo",
                LastName = "Soto",
                Email = "bernardo@soto.com",
                Mobile = "8888888",
                University = "UNA",
                Province = "Heredia",
                Canton = "Heredia",
                DoesOtherProvinces = "true",
                OtherProvinces = otherProvinces,
                Description = "pet nerd"
            };
            var serializedObj = JsonConvert.SerializeObject(obj);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async Task CreateWalker_FailsOn_DuplicatedSchoolId()
        {
            var url = "/api/walkers";

            string[] otherProvinces = { "San Jose", "Cartago" };
            var obj = new
            {
                SchoolId = "200943531",
                Password = "12345678",
                Name = "Juan",
                LastName = "Perez",
                Email = "juan@perez.com",
                Mobile = "8888888",
                University = "TEC",
                Province = "Cartago",
                Canton = "Cartago",
                DoesOtherProvinces = "true",
                OtherProvinces = otherProvinces,
                Description = "no"
            };
            var serializedObj = JsonConvert.SerializeObject(obj);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);

            var serialResponse = await response.Content.ReadAsStringAsync();
            var parsedMessage = JsonConvert.DeserializeObject<MessageResponse>(serialResponse);

            Assert.Equal("error creating user: 23505: duplicate key value violates unique constraint \"AK_Users_Username\"",
               parsedMessage.Message);


        }


        class MessageResponse
        {
            public string Message { get; set; }

        }
    }
}
