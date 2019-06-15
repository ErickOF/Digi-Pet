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
using System.Net.Http.Headers;
using System.Linq;

namespace XUnitTestProject1
{
    
    public class Test 
    {

        private readonly HttpClient _client;
        private const string AdminToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNTYwNDQxOTc3LCJleHAiOjE1NjEwNDY3NzcsImlhdCI6MTU2MDQ0MTk3N30.90FQh9uNVfYGhWWHnyeuBIZ45-7AYvEYhRPCQOSc1_M";


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


        public async Task<string> Authenticate(string username, string password)
        {
            var url = "/users/authenticate";
            var user = new { UserName = username, Password =password };
             var formContent = new FormUrlEncodedContent(new[]
             {
                 new KeyValuePair<string, string>("UserName", username),
                 new KeyValuePair<string, string>("Password", password)
             });
             var response = await _client.PostAsync(url,formContent);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var parsed = JsonConvert.DeserializeObject<AuthedUser>(await response.Content.ReadAsStringAsync());

            Assert.Equal( username, parsed.UserName);
            //Assert.Equal("admin@digipet.com", parsed.Email);
            Assert.NotNull(parsed.Token);
            return (parsed.Token);
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
            var stringResponse = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<CreatedWalkerResponse>(stringResponse);

            Assert.Equal(obj.SchoolId, parsedResponse.userName);
            Assert.NotNull(parsedResponse.id);

            //ruta para eliminar usuario
            var urlDelteUser = $"/users/delete/{obj.SchoolId}";
            //usar el token de admin
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);

            var res = await _client.DeleteAsync(urlDelteUser);
            res.EnsureSuccessStatusCode();
            var stringResponse2 = await res.Content.ReadAsStringAsync();
            var parsedMessage2 = JsonConvert.DeserializeObject<MessageResponse>(stringResponse2);

            Assert.Equal($"{obj.SchoolId} deleted",
               parsedMessage2.Message);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
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

        //crea owner, lo autentica, consulta mascotas y luego elimina
        [Fact]
        public async Task CreateNewOwner__AndThenDeleteIt()
        {
            var url = "/api/owners";

            var obj = new OwnerDto
            {
                Password = "12345678",
                Name = "Nuevo Dueño 2",
                LastName = "de Perro",
                Email = "duenoTest@gmail.com",
                Province = "Cartago",
                Canton = "Cartago",
                Description = "no",
                Pets = new PetDto[]
                {
                    new PetDto { Name="Perro Test", Race = "Test ", Age=10,Size = "M",Description="zaguate"},
                    new PetDto {Name = "Perro Test 2", Race="Test", Age = 20,Size="M",Description = "nada"}
                },
                Mobile = "88888888"
            };
            var serializedObj = JsonConvert.SerializeObject(obj);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<CreatedWalkerResponse>(stringResponse);

            Assert.Equal(obj.Email, parsedResponse.userName);
            Assert.NotNull(parsedResponse.id);

            var ownerToken = await Authenticate(obj.Email, obj.Password);
            await CheckProfileOwner(obj, ownerToken);


            //ruta para eliminar usuario
            var urlDelteUser = $"/users/delete/{obj.Email}";
            //usar el token de admin
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);

            var res = await _client.DeleteAsync(urlDelteUser);
            res.EnsureSuccessStatusCode();
            var stringResponse2 = await res.Content.ReadAsStringAsync();
            var parsedMessage2 = JsonConvert.DeserializeObject<MessageResponse>(stringResponse2);

            Assert.Equal($"{obj.Email} deleted",
               parsedMessage2.Message);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
        }

        internal async Task CheckProfileOwner(OwnerDto ownerDto, string ownerToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ownerToken);

            await CheckPets(ownerToken);
            
            _client.DefaultRequestHeaders.Clear();
        }
        internal async Task CheckPets( string ownerToken)
        {
            var url = "api/pets/";

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<PetDto[]>(stringResponse);
            Assert.Equal(2, parsedResponse.Length);
            Assert.All(parsedResponse, item => Assert.Contains("Perro Test", item.Name));

            // compara pet 1 o 1
            var url2 = $"api/pets/{parsedResponse[0].Id}";
            var response2 = await _client.GetAsync(url2);
            response2.EnsureSuccessStatusCode();
            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var parsedResponse2 = JsonConvert.DeserializeObject<PetDto>(stringResponse2);
            Assert.True(ComparePets(parsedResponse[0], parsedResponse2));

            var url3 = $"api/pets/{parsedResponse[1].Id}";
            var response3 = await _client.GetAsync(url3);
            response3.EnsureSuccessStatusCode();
            var stringResponse3 = await response3.Content.ReadAsStringAsync();
            var parsedResponse3 = JsonConvert.DeserializeObject<PetDto>(stringResponse3);
           Assert.True(ComparePets(parsedResponse[1], parsedResponse3));
        }

        internal bool ComparePets(PetDto pet1, PetDto pet2)
        {
            
            return pet1.Id == pet2.Id && pet1.Name == pet2.Name
                && pet1.Race==pet2.Race &&pet1.Size==pet2.Size &&
                (
                (pet1.Photos!=null && pet2.Photos!=null)? 
                    pet1.Photos.SequenceEqual(pet2.Photos):true) &&
                pet1.Trips==pet2.Trips &&
                pet1.Description==pet2.Description && pet1.DateCreated==pet2.DateCreated;
        }

        #region helperClasses
        class MessageResponse
        {
            public string Message { get; set; }

        }
        class CreatedWalkerResponse
        {
            public string id { get; set; }
            public string userName { get; set; }
        }
        #endregion
    }
}
