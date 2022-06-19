using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace TaskBoard.APITests
{
    public class ApiTests
    {
        private const string url = "https://taskboard.nikolayrusev1.repl.co/api/tasks";
       // private const string url = "https://taskboard.nakov.repl.co/api/tasks";
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(url);
        }

        [Test]
        public void Test_GetAllTasks_CheckFirstTasks()
        {
            //Arrange
            this.request = new RestRequest(url);

            //Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].title, Is.EqualTo("Project skeleton"));
            // Assert.That(tasks[0].board_name, Is.EqualTo("Done"));  ;
        }
        [Test]
        public void Test_SearchTasks_ByValidKeyword()
        {
            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "home");

            //Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].title, Is.EqualTo("Home page"));


        }


        [Test]
        public void Test_SearchTask_ByInvalidKeywoard()
        {
            //Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "missing1231239");

            //Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.EqualTo(0));


        }

        [Test]
        public void Test_Create_InvalidData()
        {
            //Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                id = "12321",
                email = "asdasd@abv.bg",
                phone = "123123231"
            };
            request.AddJsonBody(body);

            //Act
            var response = this.client.Execute(request, Method.Post);


            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));
        }

        [Test]
        public void Test_CreateTask_ValidData()
        {
            //Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                title = "Add Tests",
                description = "API + UI tests",
                board = "Open"
            };
            request.AddJsonBody(body);

            //Act
            var response = this.client.Execute(request, Method.Post);


            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));


            var allTasks = this.client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(allTasks.Content);

            var lastContact = tasks.Last();


            Assert.That(lastContact.title, Is.EqualTo(body.title));
            Assert.That(lastContact.description, Is.EqualTo(body.description));
        }
    }
}