using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stize.ApiTemplate.Api.Controllers;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Specifications;
using Stize.DotNet.Delta;
using Stize.DotNet.Search.Model;
using Stize.DotNet.Search.Sort;
using Stize.Hosting.AspNetCore.ActionResult;
using Stize.Mapping.Service;
using Stize.Persistence.QueryResult;
using Xunit;
using FilterDescriptor = Stize.DotNet.Search.Filter.FilterDescriptor;

namespace Stize.ApiTemplate.Api.Test.ToDoList
{
    public class ToDoListTest
    {
        private readonly ToDoListController controller;
        private readonly Mock<IMappingService<EntityDbContext>> mappingServiceMock;
        private readonly Mock<IToDoListService> toDoListServiceMock;
        private readonly MockRepository mockRepository;
       

        public ToDoListTest()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mappingServiceMock = this.mockRepository.Create<IMappingService<EntityDbContext>>();
            this.toDoListServiceMock = this.mockRepository.Create<IToDoListService>();
            this.controller = new ToDoListController(this.mappingServiceMock.Object, this.toDoListServiceMock.Object);
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            // Arrange
            this.mappingServiceMock
                .Setup(ms => ms.GetAllAsync<ToDoListModel, Domain.Entities.ToDoList>(default))
                .ReturnsAsync(() => new ToDoListModel[1]);

            // Act
            var result = await this.controller.GetAsync(default);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var models = Assert.IsAssignableFrom<IEnumerable<ToDoListModel>>(okResult.Value);
            Assert.NotEmpty(models);
        }

        [Fact]
        public async Task SearchAsyncTest()
        {
            // Arrange
            var page = new PageDescriptorModel()
            {
                Filters = new List<FilterDescriptor>(),
                Sorts = new List<SortDescriptor>()
            };
            this.toDoListServiceMock
                .Setup(ms => ms.SearchAsync(page, default))
                .ReturnsAsync(() => new PagedQueryResult<ToDoListModel>()
                {
                    Result = new ToDoListModel[1]
                });

            // Act            
            var result = await this.controller.SearchAsync(page,default);

            // Assert
            var okResult = Assert.IsType<PagedJsonResult<ToDoListModel>>(result);
            var models = Assert.IsAssignableFrom<IEnumerable<ToDoListModel>>(okResult.GetContent());
            Assert.NotEmpty(models);
        }

        [Fact]
        public async Task GetByIdAsyncTest()
        {
            // Arrange
            const int id = 1;
            this.mappingServiceMock
                .Setup(ms => ms.FindOneAsync<ToDoListModel, Domain.Entities.ToDoList, int>(id, default))
                .ReturnsAsync(() => new ToDoListModel());

            // Act
            var result = await this.controller.GetAsync(id, default);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ToDoListModel>(okResult.Value);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task PostAsyncTest()
        {
            // Arrange
            const int createdId = 1;
            var createModel = new CreateToDoListModel() ;    
            this.mappingServiceMock
                .Setup(ms => ms.AddAsync<CreateToDoListModel, Domain.Entities.ToDoList, int>(createModel, default))
                .ReturnsAsync(() => createdId);

            // Act
            var result = await this.controller.PostAsync(createModel, default);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdAt.Value);
            
            var returnedId = createdAt.Value.GetType().GetProperty("Id").GetValue(createdAt.Value);
            Assert.Equal(createdId, returnedId);
        }

        [Fact]
        public async Task PutAsyncTest()
        {
            // Arrange
            const int id = 1;
              
            this.mappingServiceMock
                .Setup(ms => ms.AnyAsync(It.IsAny<EntityByIdSpecification<Domain.Entities.ToDoList>>(), default))
                .ReturnsAsync(() => true);

            var updateModel = new UpdateToDoListModel() ;  
            this.mappingServiceMock
                .Setup(ms => ms.ApplyChangesAsync<UpdateToDoListModel, Domain.Entities.ToDoList, int>(updateModel, default))
                .Returns(Task.CompletedTask);

            this.mappingServiceMock
                .Setup(ms => ms.FindOneAsync<ToDoListModel, Domain.Entities.ToDoList, int>(id, default))
                .ReturnsAsync(() => new ToDoListModel {Id = id});


            // Act
            var result = await this.controller.PutAsync(id, updateModel, default);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ToDoListModel>(okResult.Value);
            Assert.NotNull(model);
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task PatchAsyncTest()
        {
            // Arrange
            const int id = 1;
              
            this.mappingServiceMock
                .Setup(ms => ms.AnyAsync(It.IsAny<EntityByIdSpecification<Domain.Entities.ToDoList>>(), default))
                .ReturnsAsync(() => true);

            var updateModel = new Delta<UpdateToDoListModel>() ;  
            this.mappingServiceMock
                .Setup(ms => ms.PatchAsync<UpdateToDoListModel, Domain.Entities.ToDoList, int>(updateModel, default))
                .Returns(Task.CompletedTask);

            this.mappingServiceMock
                .Setup(ms => ms.FindOneAsync<ToDoListModel, Domain.Entities.ToDoList, int>(id, default))
                .ReturnsAsync(() => new ToDoListModel {Id = id});


            // Act
            var result = await this.controller.PatchAsync(id, updateModel, default);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ToDoListModel>(okResult.Value);
            Assert.NotNull(model);
            Assert.Equal(id, model.Id);
        }

        
        [Fact]
        public async Task DeleteAsyncTest()
        {
            // Arrange
            const int id = 1;
              
            this.mappingServiceMock
                .Setup(ms => ms.AnyAsync(It.IsAny<EntityByIdSpecification<Domain.Entities.ToDoList>>(), default))
                .ReturnsAsync(() => true);

            this.mappingServiceMock
                .Setup(ms => ms.RemoveAsync<Domain.Entities.ToDoList, int>(id, default))
                .Returns(Task.CompletedTask);
            
            // Act
            var result = await this.controller.DeleteAsync(id,  default);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
