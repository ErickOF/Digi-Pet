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
using WebApi.Model;

namespace XUnitTestProject1
{
    
    public class Test 
    {

        private readonly HttpClient _client;
        private const string AdminToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNTYwNDQxOTc3LCJleHAiOjE1NjEwNDY3NzcsImlhdCI6MTU2MDQ0MTk3N30.90FQh9uNVfYGhWWHnyeuBIZ45-7AYvEYhRPCQOSc1_M";
        
        #region tempData
        private WalkerDto TempWalker = new  WalkerDto
        {
            SchoolId = "300943530",
            Password = "12345678",
            Name = "TEST",
            LastName = "WALKER",
            Email = "TEST@pWALKER.com",
            Mobile = "8888888",
            University = "TEC",
            Province = "CartagoTEST123",
            Canton = "CartagoTEST123",
            DoesOtherProvinces = true,
            OtherProvinces = new string[] { "HerediaTEST",   "AlajuelaTEST"},
            Description = "no",
            Photo = "urlphoto"
        };

        private WeekScheduleDto TempWeekSchedule = new WeekScheduleDto
        {
            Week = new List<ScheduleDto>()
            {
                new ScheduleDto{Date=DateTime.Today.AddDays(3).Date, HoursAvailable= Enumerable.Range(0,23).ToArray()},
                new ScheduleDto{Date=DateTime.Today.AddDays(4).Date, HoursAvailable= new int[]{13,14,15,16,17,18 } },
            }
        };
        private static string TempWalkerToken { get; set; }
        private OwnerDto TestOwner = new OwnerDto
        {
            Password = "12345678",
            Name = "Dueño 2",
            LastName = "de Perro",
            Email = "dueno_TEST@gmail.com",
            Province = "Cartago",
            Canton = "Cartago",
            Description = "no",
            Pets = new List<PetDto>()
            {
                new PetDto{
                    Name = "PERRO TEST 1",
                    Race = "SRD",
                    Age = 1,
                    Size = "M",
                    Description = "zaguate"
                },
                new PetDto {
                    Name = "PERRO TEST 2",
                    Race = "Beagle",
                    Age = 2,
                    Size = "M",
                    Description = "gordo"
                }
            },
            Mobile = "88888888",
            Photo = "url test"
        };
        private static string TestOwnerToken { get; set; }

        #endregion
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

        #region Tt
        /*
        [Fact]
        public async Task CreateNewWalker__AndThenDeleteIt()
        {
            
            var response = await CreateWalker(TempWalker);//se crea un cuidador
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<CreatedWalkerResponse>(stringResponse);

            Assert.Equal(TempWalker.SchoolId, parsedResponse.userName);
            Assert.NotNull(parsedResponse.id);

            if (string.IsNullOrEmpty(TempWalkerToken))//autentica nuevo usuario
            {
                TempWalkerToken = await Authenticate(TempWalker.SchoolId, TempWalker.Password);
            }

            response = await PostWalkerSchedule(TempWeekSchedule);//se crea un horario
            response.EnsureSuccessStatusCode();


            var res = await DeleteUser(TempWalker.SchoolId);
            res.EnsureSuccessStatusCode();
            var stringResponse2 = await res.Content.ReadAsStringAsync();
            var parsedMessage2 = JsonConvert.DeserializeObject<MessageResponse>(stringResponse2);

            Assert.Equal($"{TempWalker.SchoolId} deleted",
               parsedMessage2.Message);
        }
        */
        #endregion

        [Fact]
        public async Task CreateWalker_FailsOn_DuplicatedSchoolId()
        {
            //crea un usuario nuevo
            var response = await CreateWalker(TempWalker);
            response.EnsureSuccessStatusCode();

            //debe fallar al volver a postear el mismo usuario
            response = await CreateWalker(TempWalker);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var serialResponse = await response.Content.ReadAsStringAsync();
            var parsedMessage = JsonConvert.DeserializeObject<MessageResponse>(serialResponse);

            Assert.Equal("error creating user: 23505: duplicate key value violates unique constraint \"AK_Users_Username\"",
               parsedMessage.Message);

            var res = await DeleteUser(TempWalker.SchoolId);
            res.EnsureSuccessStatusCode();
        }

        #region t1
            /*
        [Fact]
        public async Task CreateNewOwner__AndThenDeleteIt()
        {


            var response = await  CreateOwner(TestOwner);
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<CreatedWalkerResponse>(stringResponse);

            Assert.Equal(TestOwner.Email, parsedResponse.userName);
            Assert.NotNull(parsedResponse.id);

         

            //eliminar usuario
            var res = await DeleteUser(TestOwner.Email);
            res.EnsureSuccessStatusCode();
            var stringResponse2 = await res.Content.ReadAsStringAsync();
            var parsedMessage2 = JsonConvert.DeserializeObject<MessageResponse>(stringResponse2);

            Assert.Equal($"{TestOwner.Email} deleted",parsedMessage2.Message);
        }
        */
        #endregion t1

        internal async Task<HttpResponseMessage> CreateOwner(OwnerDto obj)
        {
            var url = "/api/owners";
            var serializedObj = JsonConvert.SerializeObject(TestOwner);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            return await _client.PostAsync(url, content);
        }

        [Fact]
        public async Task RequestWalk_Path()
        {
            try
            {
                var res = await CreateWalker(TempWalker);//crea cuidador
                res.EnsureSuccessStatusCode();
                var returnedWalker = JsonConvert.DeserializeObject<CreatedWalkerResponse>(await res.Content.ReadAsStringAsync());

                Assert.Equal(TempWalker.SchoolId, returnedWalker.userName);
                Assert.NotEqual(0, returnedWalker.id);
                if (string.IsNullOrEmpty(TempWalkerToken))//autentica nuevo usuario
                {
                    TempWalkerToken = await Authenticate(TempWalker.SchoolId, TempWalker.Password);
                }

                res = await PostWalkerSchedule(TempWeekSchedule);//se crea un horario
                res.EnsureSuccessStatusCode();

                await CompareWalkerScheduleWithOriginal();// se compara el horario creado

                var res2 = await CreateOwner(TestOwner);
                if (string.IsNullOrEmpty(TestOwnerToken))
                {
                    TestOwnerToken = await Authenticate(TestOwner.Email, TestOwner.Password);
                }

                var pets = await GetPets(TestOwnerToken);
                Assert.Equal(2, pets.Count);
                var walkRequest = new WalkRequestDto
                {
                    PetId = pets[0].Id,
                    Begin = DateTime.Today.AddDays(3).AddHours(9).ToUniversalTime(),
                    Duration = 1,
                    Province = TempWalker.Province,
                    Canton = TempWalker.Canton,
                    Description = "noo",
                    ExactAddress = "direccion exacta"
                };

                // pide paseo para una mascota
                var res3 = await PostWalkRequest(walkRequest, TestOwnerToken);
                res3.EnsureSuccessStatusCode();
                var message3 = await res3.Content.ReadAsStringAsync();
                var parsedWalkRequest = JsonConvert.DeserializeObject<WalkInfoDto>(message3);

                Assert.Equal(returnedWalker.id, parsedWalkRequest.WalkerId);
                //pide paseo para la otra mascota y debe asignar el mismo
                walkRequest.PetId = pets[1].Id;
                var res4 = await PostWalkRequest(walkRequest, TestOwnerToken);
                res4.EnsureSuccessStatusCode();
                var message4 = await res3.Content.ReadAsStringAsync();
                var parsedWalkRequest2 = JsonConvert.DeserializeObject<WalkInfoDto>(message4);

                Assert.Equal(returnedWalker.id, parsedWalkRequest2.WalkerId);

                await CheckOwnerWalks(pets);
                await CheckWalkerWalks(pets);

                //pide un paseo para el siguiente dia y debe fallar porque el cuidador no tiene hora disponible
                walkRequest.Begin = DateTime.Today.AddDays(4).AddHours(9);
                var res5 = await PostWalkRequest(walkRequest, TestOwnerToken);
                Assert.Equal(HttpStatusCode.BadRequest, res5.StatusCode);

                var resBlockWalker = await BlockUnblockWalker(returnedWalker.id, true);
                resBlockWalker.EnsureSuccessStatusCode();

                var resGetProfile = await GetProfile("walkers", TempWalkerToken);
                resGetProfile.EnsureSuccessStatusCode();
                var resWalkerProfile = JsonConvert.DeserializeObject<ReturnWalker>(await resGetProfile.Content.ReadAsStringAsync());
                Assert.Equal(returnedWalker.id, resWalkerProfile.Id);
                //se verifica que se haya bloqueado perfil
                Assert.True(resWalkerProfile.Blocked);

                //al pedir en el dia que tiene disponible pero al estar bloqueado, no se pueden encontrar 
                walkRequest.Begin = DateTime.Today.AddDays(4).AddHours(15);
                var res6 = await PostWalkRequest(walkRequest, TestOwnerToken);
                Assert.Equal(HttpStatusCode.BadRequest, res6.StatusCode);

                var resUnBlockWalker = await BlockUnblockWalker(returnedWalker.id, false);
                resUnBlockWalker.EnsureSuccessStatusCode();
                var resGetProfile2 = await GetProfile("walkers", TempWalkerToken);
                resGetProfile2.EnsureSuccessStatusCode();
                var resWalkerProfile2 = JsonConvert.DeserializeObject<ReturnWalker>(await resGetProfile2.Content.ReadAsStringAsync());
                Assert.Equal(returnedWalker.id, resWalkerProfile2.Id);

                //se verifica que se haya desbloqueado perfil
                Assert.False(resWalkerProfile2.Blocked);

                //al desbloquear usuario se debe poder volver a encontrar un cuidador

                var res7 = await PostWalkRequest(walkRequest, TestOwnerToken);
                res7.EnsureSuccessStatusCode();
                var message7 = await res7.Content.ReadAsStringAsync();
                var parsedWalkRequest7 = JsonConvert.DeserializeObject<WalkInfoDto>(message7);

                Assert.Equal(returnedWalker.id, parsedWalkRequest7.WalkerId);

                //modifico fecha para que pueda hacer tarjeta de reporte
                var resModifyDate =await ModifyDate(parsedWalkRequest.Id, DateTime.Today.AddDays(-1));
                resModifyDate.EnsureSuccessStatusCode();

                await PostReportCard(parsedWalkRequest.Id);
                
            }
            finally
            {
                //eliminar dueño y cuidador
                await DeleteUser(TestOwner.Email);
                await DeleteUser(TempWalker.SchoolId);
            }
        }
        internal async Task CheckNewRating()
        {
            var res = await GetProfile("walkers", TempWalkerToken);
            res.EnsureSuccessStatusCode();
            var WalkerProfile = JsonConvert.DeserializeObject<ReturnWalker>(await res.Content.ReadAsStringAsync());
            //Assert.Equal(4, WalkerProfile.Rating);
        }
        internal async Task PostReportCard(int walkId)
        {
            var url = "/api/walkers/postReport";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempWalkerToken);
            var report = new
            {
                WalkId = walkId,
                Comments = "me atacooo",
                Photos = new string[] { "urls" },
                Distance = 6,
                Necesidades =new string[]{"orinooo"}
            };
            var serializedObj = JsonConvert.SerializeObject(report);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            var res = await _client.PostAsync(url,content);
            res.EnsureSuccessStatusCode();
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            var urlGetReport = $"/api/owners/report/{walkId}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestOwnerToken);
            var resGetReport = await _client.GetAsync(urlGetReport);
            resGetReport.EnsureSuccessStatusCode();
            var reportReturn = JsonConvert.DeserializeObject<ReportWalk>(await resGetReport.Content.ReadAsStringAsync());
            Assert.Equal(report.WalkId, reportReturn.WalkId);
            Assert.Equal(report.Comments, reportReturn.Comments);
            Assert.Equal(report.Photos, reportReturn.Photos);
            Assert.Equal(report.Distance, reportReturn.Distance);
            Assert.Equal(report.Necesidades, reportReturn.Necesidades);
            _client.DefaultRequestHeaders.Clear();

            await Rate(walkId);
        }
        internal async Task Rate(int walkId)
        {
            var url = "/api/owners/rate";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestOwnerToken);
            var rate = new {walkId,stars=4 };
            var serializedObj = JsonConvert.SerializeObject(rate);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            var res = await _client.PostAsync(url,content);
            _client.DefaultRequestHeaders.Clear();
            res.EnsureSuccessStatusCode();
            await CheckNewRating();
        }
        internal async Task CompareWalkerScheduleWithOriginal()
        {
            var url = "/api/walkers/schedule";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempWalkerToken);
            var res = await _client.GetAsync(url);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            var resWalkerSchedule = JsonConvert.DeserializeObject<List<ScheduleDto>>(await res.Content.ReadAsStringAsync());

            Assert.Equal(TempWeekSchedule.Week.ElementAt(0).Date, resWalkerSchedule[0].Date);
            Assert.Equal(TempWeekSchedule.Week.ElementAt(0).HoursAvailable, resWalkerSchedule[0].HoursAvailable);

            Assert.Equal(TempWeekSchedule.Week.ElementAt(1).Date, resWalkerSchedule[1].Date);
            Assert.Equal(TempWeekSchedule.Week.ElementAt(1).HoursAvailable, resWalkerSchedule[1].HoursAvailable);

        }

        internal async Task CheckOwnerWalks(List<PetDto> pets)
        {
            var url = "/api/owners/upcoming";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestOwnerToken);
            var res = await _client.GetAsync(url);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            var walks = JsonConvert.DeserializeObject<List<WalkInfoDto>>(await res.Content.ReadAsStringAsync());

            Assert.Equal(pets[0].Name,walks[0].Pet.Name);
            Assert.Equal(pets[1].Name,walks[1].Pet.Name);

        }

        internal async Task CheckWalkerWalks(List<PetDto> pets)
        {
            var url = "/api/walkers/upcoming";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempWalkerToken);
            var res = await _client.GetAsync(url);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            var walks = JsonConvert.DeserializeObject<List<WalkInfoDto>>(await res.Content.ReadAsStringAsync());

            Assert.Equal(pets[0].Name, walks[0].Pet.Name);
            Assert.Equal(pets[1].Name, walks[1].Pet.Name);

        }


        internal  Task<HttpResponseMessage> GetProfile(string route,string token)
        {
            var url = $"/api/{route}/getprofile";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res =  _client.GetAsync(url);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            return res;
        }

        internal async Task<HttpResponseMessage> BlockUnblockWalker(int walkerId,bool block)
        {
            string url = block ? $"/api/admins/blockWalker/{walkerId}": $"api/admins/unblockWalker/{walkerId}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
            var res = await _client.PostAsync(url,null);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            return res;
        }
        internal Task<HttpResponseMessage> ModifyDate(int walkId,DateTime newDate)
        {
            var url = $"/api/admins/modifyDateOfWalk/{walkId}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
            var serializedObj = JsonConvert.SerializeObject(newDate);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            var res = _client.PostAsync(url,content);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            return res;
        }
        #region interno
        internal async Task<HttpResponseMessage> PostWalkerSchedule(WeekScheduleDto obj)
        {
            var url = "/api/walkers/schedule";
            var serializedObj = JsonConvert.SerializeObject(obj);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempWalkerToken);
            var res = await _client.PostAsync(url, content);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            var parsedWalkRequest = await res.Content.ReadAsStringAsync();
            return res;
        }

        internal async Task<HttpResponseMessage> CreateWalker(WalkerDto obj)
        {
            var url = "/api/walkers";
            var serializedObj = JsonConvert.SerializeObject(obj);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            return await _client.PostAsync(url, content);
        }

        internal async Task<HttpResponseMessage> PostWalkRequest(WalkRequestDto requestDto, string ownerToken)
        {
            var url = "/api/owners/requestWalk";
            var serializedObj = JsonConvert.SerializeObject(requestDto);
            var content = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ownerToken);
            var res= await _client.PostAsync(url,content);
            _client.DefaultRequestHeaders.Clear();
            var parsedWalkRequest = await res.Content.ReadAsStringAsync();

            return res;
        }

        internal async Task<HttpResponseMessage> DeleteUser(string username)
        {
            var urlDelteUser = $"/users/delete/{username}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
            var res = await _client.DeleteAsync(urlDelteUser);
            //limpiar los headers
            _client.DefaultRequestHeaders.Clear();
            return res;
        }

        internal async Task<List<PetDto>> GetPets( string ownerToken)
        {
            var url = "api/pets/";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ownerToken);
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            _client.DefaultRequestHeaders.Clear();
            return JsonConvert.DeserializeObject<List<PetDto>>(stringResponse);

        }
        /*
        internal bool ComparePets(PetDto pet1, PetDto pet2)
        {
            
            return pet1.Id == pet2.Id && pet1.Name == pet2.Name
                && pet1.Race==pet2.Race &&pet1.Size==pet2.Size &&
                (
                (pet1.Photos!=null && pet2.Photos!=null)? 
                    pet1.Photos.SequenceEqual(pet2.Photos):true) &&
                pet1.Trips==pet2.Trips &&
                pet1.Description==pet2.Description && pet1.DateCreated==pet2.DateCreated;
        }*/

        #region helperClasses
        class MessageResponse
        {
            public string Message { get; set; }

        }
        class CreatedWalkerResponse
        {
            public int id { get; set; }
            public string userName { get; set; }
        }
        #endregion

        #endregion
    }
}
