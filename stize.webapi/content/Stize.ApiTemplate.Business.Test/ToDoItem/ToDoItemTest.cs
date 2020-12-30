using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Stize.ApiTemplate.Business.Models.ToDoItem;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.Persistence;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;
using Xunit;

namespace Stize.ApiTemplate.Business.Test.ToDoItem
{
    public class ToDoItemTest
    {
        private readonly ITodoItemService service;
        private readonly Mock<IEntityRepository<EntityDbContext>> repositoryMoq;
        private readonly Mock<IQueryDispatcher> queryDispatcherMoq;
        private readonly Mock<ISpecificationBuilder> specificationBuilderMoq;

        public ToDoItemTest()
        {
            this.repositoryMoq = new Mock<IEntityRepository<EntityDbContext>>(MockBehavior.Strict);
            this.queryDispatcherMoq = new Mock<IQueryDispatcher>(MockBehavior.Strict);
            this.specificationBuilderMoq = new Mock<ISpecificationBuilder>(MockBehavior.Strict);
            this.service = new TodoItemService(repositoryMoq.Object, this.queryDispatcherMoq.Object, this.specificationBuilderMoq.Object);
        }

        [Fact]
        public async Task CompleteTest()
        {
            // Arrange
            const int id = 1;
            var todoItem = new Domain.Entities.ToDoItem {Id = id};

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

            this.queryDispatcherMoq
                .Setup(qd => qd.DispatchAsync<Query<Domain.Entities.ToDoItem>, MultipleQueryResult<ToDoItemModel>>(It.IsAny<Query<Domain.Entities.ToDoItem>>(), default))
                .ReturnsAsync((Query<Domain.Entities.ToDoItem> q, CancellationToken _) =>
                {
                    return new MultipleQueryResult<ToDoItemModel>()
                    {
                        Result = new ToDoItemModel[1]
                    };
                });
            
            // Act
            var todoItems = await this.service.GetAllAsync<ToDoItemModel>(todoListId, default);
            
            // Assert
            Assert.NotEmpty(todoItems.Result);
        }

        [Fact]
        public async Task SearchAsync()
        {
            // Arrange
            const int todoListId = 1;
            var pageMoq = new Mock<IPageDescriptor>();

            this.specificationBuilderMoq
                .Setup(sb => sb.Create<Domain.Entities.ToDoItem>(It.IsAny<IEnumerable<FilterDescriptor>>()))
                .Returns(() => Specification<Domain.Entities.ToDoItem>.True);

            this.queryDispatcherMoq
                .Setup(qd => qd.DispatchAsync<PagedQuery<Domain.Entities.ToDoItem>, PagedQueryResult<ToDoItemModel>>(It.IsAny<PagedQuery<Domain.Entities.ToDoItem>>(), default))
                .ReturnsAsync((PagedQuery<Domain.Entities.ToDoItem> q, CancellationToken _) =>
                {
                    return new PagedQueryResult<ToDoItemModel>()
                    {
                        Result = new ToDoItemModel[1]
                    };
                });
            
            // Act
            var todoItems = await this.service.SearchAsync<ToDoItemModel>(todoListId, pageMoq.Object, default);
            
            // Assert
            Assert.NotEmpty(todoItems.Result);
        }
    }
}
