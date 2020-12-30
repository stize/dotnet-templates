using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Business.Services;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.Persistence;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;
using Xunit;

namespace Stize.ApiTemplate.Business.Test.ToDoList
{
    public class ToDoListTest
    {
        private readonly IToDoListService service;
        private readonly Mock<IQueryDispatcher> queryDispatcherMoq;
        private readonly Mock<ISpecificationBuilder> specificationBuilderMoq;

        public ToDoListTest()
        {
            this.queryDispatcherMoq = new Mock<IQueryDispatcher>(MockBehavior.Strict);
            this.specificationBuilderMoq = new Mock<ISpecificationBuilder>(MockBehavior.Strict);
            this.service = new ToDoListService(this.queryDispatcherMoq.Object, this.specificationBuilderMoq.Object);
        }
        
        [Fact]
        public async Task SearchAsync()
        {
            // Arrange
            const int todoListId = 1;
            var pageMoq = new Mock<IPageDescriptor>();

            this.specificationBuilderMoq
                .Setup(sb => sb.Create<Domain.Entities.ToDoList>(It.IsAny<IEnumerable<FilterDescriptor>>()))
                .Returns(() => Specification<Domain.Entities.ToDoList>.True);

            this.queryDispatcherMoq
                .Setup(qd => qd.DispatchAsync<PagedQuery<Domain.Entities.ToDoList>, PagedQueryResult<ToDoListModel>>(It.IsAny<PagedQuery<Domain.Entities.ToDoList>>(), default))
                .ReturnsAsync((PagedQuery<Domain.Entities.ToDoList> q, CancellationToken _) =>
                {
                    return new PagedQueryResult<ToDoListModel>()
                    {
                        Result = new ToDoListModel[1]
                    };
                });
            
            // Act
            var todoItems = await this.service.SearchAsync(pageMoq.Object, default);
            
            // Assert
            Assert.NotEmpty(todoItems.Result);
        }
    }
}
