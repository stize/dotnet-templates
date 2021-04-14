using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Stize.ApiTemplate.Business.Models.ToDoItem;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using Xunit;

namespace Stize.ApiTemplate.Business.Test.ToDoItem
{
    public class ToDoItemTest
    {
        private readonly ITodoItemService service;
        private readonly Mock<IEntityRepository<EntityDbContext>> repositoryMoq;
        private readonly Mock<ISpecificationBuilder> specificationBuilderMoq;

        public ToDoItemTest()
        {
            this.repositoryMoq = new Mock<IEntityRepository<EntityDbContext>>(MockBehavior.Strict);
            this.specificationBuilderMoq = new Mock<ISpecificationBuilder>(MockBehavior.Strict);
            this.service = new TodoItemService(this.repositoryMoq.Object, this.specificationBuilderMoq.Object);
        }

        [Fact]
        public async Task CompleteTest()
        {
            // Arrange
            const int id = 1;
            var todoItem = new Domain.Entities.ToDoItem { Id = id };

            this.repositoryMoq
                .Setup(r => r.FindOneAsync<Domain.Entities.ToDoItem, int>(id, default))
                .ReturnsAsync(() =>
                {
                    return todoItem;
                });

            this.repositoryMoq
                .Setup(r => r.CommitAsync(default))
                .Returns(Task.CompletedTask);

            // Act
            await this.service.CompleteAsync(id, CancellationToken.None);

            // Assert
            Assert.True(todoItem.IsCompleted);
        }

        [Fact]
        public async Task GetAllAsync()
        {
            // Arrange
            const int todoListId = 1;

            //this.repositoryMoq
            //    .Setup(r => r.GetAll<Domain.Entities.ToDoItem>())
            //    .Returns(() => Array.Empty<Domain.Entities.ToDoItem>().AsQueryable());

            this.repositoryMoq
                .Setup(r => r.RunQueryAsync(It.IsAny<MultipleValueInquiry<Domain.Entities.ToDoItem, ToDoItemModel>>(), default))
                .ReturnsAsync((MultipleValueInquiry<Domain.Entities.ToDoItem, ToDoItemModel> q, CancellationToken _) =>
                {
                    return new MultipleValueResult<ToDoItemModel>()
                    {
                        Value = new ToDoItemModel[1]
                    };
                });

            // Act
            var todoItems = await this.service.GetAllAsync<ToDoItemModel>(todoListId, default);

            // Assert
            Assert.NotEmpty(todoItems.Value);
        }

        [Fact]
        public async Task SearchAsync()
        {
            // Arrange
            const int todoListId = 1;

            //this.repositoryMoq
            //    .Setup(r => r.GetAll<Domain.Entities.ToDoItem>())
            //    .Returns(() => Array.Empty<Domain.Entities.ToDoItem>().AsQueryable());

            var pageMoq = new Mock<IPageDescriptor>();

            this.specificationBuilderMoq
                .Setup(sb => sb.Create<Domain.Entities.ToDoItem>(It.IsAny<IEnumerable<FilterDescriptor>>()))
                .Returns(() => Specification<Domain.Entities.ToDoItem>.True);

            this.repositoryMoq
                .Setup(r => r.RunQueryAsync(It.IsAny<PagedValueInquiry<Domain.Entities.ToDoItem, ToDoItemModel>>(), default))
                .ReturnsAsync((PagedValueInquiry<Domain.Entities.ToDoItem, ToDoItemModel> q, CancellationToken _) =>
                {
                    return new PagedValueResult<ToDoItemModel>()
                    {
                        Value = new ToDoItemModel[1]
                    };
                });

            // Act
            var todoItems = await this.service.SearchAsync<ToDoItemModel>(todoListId, pageMoq.Object, default);

            // Assert
            Assert.NotEmpty(todoItems.Value);
        }
    }
}
