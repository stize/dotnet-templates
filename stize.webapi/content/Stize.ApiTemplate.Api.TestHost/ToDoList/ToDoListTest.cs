using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Stize.ApiTemplate.Api.Controllers;
using Stize.ApiTemplate.Api.TestHost.Extensions;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.DotNet.Delta;
using Stize.Hosting.AspNetCore.Model;
using Stize.Testing.Xunit.AspNetCore.Mvc;
using Stize.Testing.Xunit.AspNetCore.TestHost;
using Xunit;
using Xunit.Abstractions;

namespace Stize.ApiTemplate.Api.TestHost.ToDoList
{
    public class ToDoListTest : BaseTest
    {
        public ToDoListTest(WebApplicationFactory<Startup> fixture, ITestOutputHelper output) : base(fixture, output)
        {

        }

        [Fact]
        public async Task GetAsyncTest()
        {
            // Arrange
            var entity = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var request = this.Fixture
                .Server
                .CreateApiRequest<ToDoListController>(c => c.GetAsync(default))
                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Get);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ToDoListModel>>();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);

        }

        [Fact]
        public async Task SearchAsyncTest()
        {
            // Arrange
            var entity = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var page = new PageDescriptorModel()
            {
                Skip = 0,
                Take = 5,
                Envelope = true
            };
            var request = this.Fixture
                .Server
                .CreateApiRequest<ToDoListController>(c => c.SearchAsync(page, default))
                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Post);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<EnvelopedModel<ToDoListModel>>();
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
            var createModel = new CreateToDoListModel()
            {
                Name = "Before"
            };

            var request = this.Fixture
                .Server
                .CreateApiRequest<ToDoListController>(c => c.PostAsync(createModel, default))
                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Post);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<ToDoListModel>();
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

        }
        
        [Fact]
        public async Task PutAsyncTest()
        {
            // Arrange
            var entity = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            var updateToDoListModel = new UpdateToDoListModel()
            {
                Id = entity.Id,
                Name = "After"
            };

            var request = this.Fixture
                                .Server
                                .CreateApiRequest<ToDoListController>(c => c.PutAsync(entity.Id, updateToDoListModel, default))
                                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Put);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<ToDoListModel>();
            Assert.Equal(updateToDoListModel.Name, result.Name);

        }
        
        [Fact]
        public async Task PatchAsync()
        {
            // Arrange
            var entity = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });

            const string name = "After";
            var delta = new Delta<UpdateToDoListModel>();
            delta.TrySetPropertyValue(nameof(UpdateToDoListModel.Id), entity.Id);
            delta.TrySetPropertyValue(nameof(UpdateToDoListModel.Name), name);

            var request = this.Fixture
                                .Server
                                .CreateApiRequest<ToDoListController>(c => c.PatchAsync(entity.Id, delta, default))
                                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Patch);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<ToDoListModel>();
            Assert.Equal(name, result.Name);

        }
        
        [Fact]
        public async Task DeleteAsync()
        {
            // Arrange
            var entity = await this.WithEntityInTheDatabaseAsync(new Domain.Entities.ToDoList()
            {
                Name = "Before"
            });
            
            var request = this.Fixture
                                .Server
                                .CreateApiRequest<ToDoListController>(c => c.DeleteAsync(entity.Id, default))
                                .WithIdentity(Identities.Basic);
            // Act
            var response = await request.SendAsync(HttpMethods.Delete);

            // Assert
            Assert.True(response.IsSuccessStatusCode);

        }
    }
}
