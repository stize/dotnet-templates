using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stize.ApiTemplate.Api.Controllers;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Specifications;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.CQRS.EntityFrameworkCore.Query;
using Stize.DotNet.Delta;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Model;
using Stize.DotNet.Search.Sort;
using Stize.Hosting.AspNetCore.ActionResult;
using Stize.Mapping.Service;
using Stize.Mediator;
using Xunit;

namespace Stize.ApiTemplate.Api.Test.ToDoList
{
    public class ToDoListCqrsTest
    {
        
        private readonly MockRepository mockRepository;        
        private readonly Mock<IMediator> mediatorMock;
        private readonly ToDoListCqrsController controller;

        public ToDoListCqrsTest()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mediatorMock = this.mockRepository.Create<IMediator>();
            this.controller = new ToDoListCqrsController(this.mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            // Arrange
            this.mediatorMock
                .Setup(m => m.HandleAsync(It.IsAny<GetAllModelsFromEntityQuery<ToDoListModel, Domain.Entities.ToDoList, int, EntityDbContext>>(), default))
                .ReturnsAsync(() => new MultipleValueResult<ToDoListModel>
                {
                    Value = new ToDoListModel[1]
                });

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
            this.mediatorMock
                .Setup(m => m.HandleAsync(It.IsAny<GetAllModelsFromEntityByPageQuery<ToDoListModel, Domain.Entities.ToDoList, int, EntityDbContext>>(), default))
                .ReturnsAsync(() => new PagedValueResult<ToDoListModel>
                {
                    Value = new ToDoListModel[1]
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
            this.mediatorMock
                .Setup(m => m.HandleAsync(It.IsAny<GetModelFromEntityByIdQuery<ToDoListModel, Domain.Entities.ToDoList, int, EntityDbContext>>(), default))
                .ReturnsAsync(() => new SingleValueResult<ToDoListModel>
                {
                    Value = new ToDoListModel()
                });

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
            this.mediatorMock
                .Setup(m => m.HandleAsync(It.IsAny<CreateEntityFromModelCommand<CreateToDoListModel, Domain.Entities.ToDoList, int, EntityDbContext>>(), default))
                .ReturnsAsync(() => Result<int>.Success(createdId));

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
            var updateModel = new UpdateToDoListModel() { Id = id } ;
            this.mediatorMock
               .Setup(m => m.HandleAsync(It.IsAny<UpdateEntityFromModelCommand<UpdateToDoListModel, Domain.Entities.ToDoList, int, EntityDbContext>>(), default))
               .ReturnsAsync(() => Result<int>.Success(id));


            // Act
            var result = await this.controller.PutAsync(id, updateModel, default);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<int>(okResult.Value);
            Assert.Equal(id, model);
        }

        [Fact]
        public async Task PatchAsyncTest()
        {
            // Arrange
            const int id = 1;
            var delta = new Delta<UpdateToDoListModel>() ;
            delta.TrySetPropertyValue(nameof(UpdateToDoListModel.Id), id);
            this.mediatorMock
              .Setup(m => m.HandleAsync(It.IsAny<PatchEntityFromModelCommand<UpdateToDoListModel, Domain.Entities.ToDoList, int, EntityDbContext>>(), default))
              .ReturnsAsync(() => Result<int>.Success(id));


            // Act
            var result = await this.controller.PatchAsync(id, delta, default);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<int>(okResult.Value);
            Assert.Equal(id, model);
        }
        
        [Fact]
        public async Task DeleteAsyncTest()
        {
            // Arrange
            const int id = 1;
            this.mediatorMock
              .Setup(m => m.HandleAsync(It.IsAny<DeleteEntityByIdCommand<Domain.Entities.ToDoList, int, EntityDbContext>>(), default))
              .ReturnsAsync(() => Result<int>.Success(id));

            // Act
            var result = await this.controller.DeleteAsync(id,  default);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
