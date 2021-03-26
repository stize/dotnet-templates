using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Stize.ApiTemplate.Api.Controllers;
using Stize.ApiTemplate.Api.TestHost.Extensions;
using Stize.ApiTemplate.Business.Models.ToDoItem;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.Domain.Model;
using Stize.DotNet.Delta;
using Stize.DotNet.Search.Model;
using Stize.Testing.Xunit.AspNetCore.Mvc;
using Stize.Testing.Xunit.AspNetCore.TestHost;
using Xunit;
using Xunit.Abstractions;

namespace Stize.ApiTemplate.Api.TestHost.ToDoList
{
    public class ToDoItem : BaseTest
    {
        public ToDoItem(WebApplicationFactory<Startup> fixture, ITestOutputHelper output) : base(fixture, output)
        {

        }

        [Fact]
        public async Task GetAsyncTest()
        {
            // Arrange
            var todolist = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var todoItem = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoItem()
            {
                Name = "Before",
                ToDoListId = todolist.Id
            });

            var request = this.Fixture
                .Server
                .CreateApiRequest<ToDoItemController>(c => c.GetAsync(todolist.Id, default))
                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Get);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ToDoItemModel>>();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);

        }

        [Fact]
        public async Task SearchAsyncTest()
        {
            // Arrange
            var todolist = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var todoItem = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoItem()
            {
                Name = "Before",
                ToDoListId = todolist.Id
            });

            var page = new PageDescriptorModel()
            {
                Skip = 0,
                Take = 5,
                Envelope = true
            };
            var request = this.Fixture
                .Server
                .CreateApiRequest<ToDoItemController>(c => c.SearchAsync(todolist.Id, page, default))
                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Post);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<EnvelopedModel<ToDoItemModel>>();
            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            Assert.Single(result.Data);
            Assert.Equal(page.Skip, result.Skip);
            Assert.Equal(page.Take, result.Take);
            Assert.Equal(1, result.Total);

        }

        [Fact]
        public async Task PostAsyncTest()
        {
            // Arrange
            var todolist = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var createModel = new CreateToDoItemModel()
            {
                Name = "Before",
                ToDoListId = todolist.Id
            };

            var request = this.Fixture
                .Server
                .CreateApiRequest<ToDoItemController>(c => c.PostAsync(todolist.Id, createModel, default))
                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Post);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<ToDoItemModel>();
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(todolist.Id, result.ToDoListId);
            Assert.False(result.IsCompleted);
        }

        [Fact]
        public async Task PutAsyncTest()
        {
            // Arrange
            var todolist = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var todoItem = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoItem()
            {
                Name = "Before",
                ToDoListId = todolist.Id
            });

            var updateToDoListModel = new UpdateToDoItemModel()
            {
                Id = todoItem.Id,                
                Name = "After"
            };

            var request = this.Fixture
                                .Server
                                .CreateApiRequest<ToDoItemController>(c => c.PutAsync(todolist.Id, todoItem.Id, updateToDoListModel, default))
                                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Put);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        }

        [Fact]
        public async Task PatchAsync()
        {
            // Arrange
            var todolist = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var todoItem = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoItem()
            {
                Name = "Before",
                ToDoListId = todolist.Id
            });

            const string name = "After";
            var delta = new Delta<UpdateToDoItemModel>();
            delta.TrySetPropertyValue(nameof(UpdateToDoItemModel.Id), todoItem.Id);
            delta.TrySetPropertyValue(nameof(UpdateToDoItemModel.Name), name);

            var request = this.Fixture
                                .Server
                                .CreateApiRequest<ToDoItemController>(c => c.PatchAsync(todolist.Id, todoItem.Id, delta, default))
                                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Patch);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<ToDoListModel>();
            Assert.Equal(name, result.Name);

        }

        [Fact]
        public async Task CompleteAsync()
        {
            // Arrange
            var todolist = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var todoItem = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoItem()
            {
                Name = "Before",
                ToDoListId = todolist.Id
            });

            var request = this.Fixture
                                .Server
                                .CreateApiRequest<ToDoItemController>(c => c.CompleteAsync(todolist.Id, todoItem.Id, default))
                                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Post);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Arrange
            var todolist = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var todoItem = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoItem()
            {
                Name = "Before",
                ToDoListId = todolist.Id
            });

            var request = this.Fixture
                                .Server
                                .CreateApiRequest<ToDoItemController>(c => c.DeleteAsync(todolist.Id, todoItem.Id, default))
                                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Delete);

            // Assert
            Assert.True(response.IsSuccessStatusCode);

        }
    }


}
