using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryDispatcher;
using Xunit;

namespace Stize.ApiTemplate.Business.Test.ToDoList
{
    public class ToDoListTest
    {
        private readonly IToDoListService service;
        private readonly Mock<IInquiryDispatcher> inquiryDispatcherMoq;
        private readonly Mock<IEntityRepository<EntityDbContext>> repositoryMoq;
        private readonly Mock<ISpecificationBuilder> specificationBuilderMoq;

        public ToDoListTest()
        {
            this.inquiryDispatcherMoq = new Mock<IInquiryDispatcher>(MockBehavior.Strict);
            this.repositoryMoq = new Mock<IEntityRepository<EntityDbContext>>(MockBehavior.Strict);
            this.specificationBuilderMoq = new Mock<ISpecificationBuilder>(MockBehavior.Strict);
            this.service = new ToDoListService(this.inquiryDispatcherMoq.Object, this.repositoryMoq.Object, this.specificationBuilderMoq.Object);
        }
        
        [Fact]
        public async Task SearchAsync()
        {
            // Arrange
            const int todoListId = 1;
            var pageMoq = new Mock<IPageDescriptor>();

            this.repositoryMoq
                .Setup(r => r.GetAll<Domain.Entities.ToDoList>())
                .Returns(() => Array.Empty<Domain.Entities.ToDoList>().AsQueryable());

            this.specificationBuilderMoq
                .Setup(sb => sb.Create<ToDoListModel>(It.IsAny<IEnumerable<FilterDescriptor>>()))
                .Returns(() => Specification<ToDoListModel>.True);

            this.inquiryDispatcherMoq
                .Setup(qd => qd.HandleAsync(It.IsAny<PagedValueInquiry<Domain.Entities.ToDoList, ToDoListModel>>(), default))
                .ReturnsAsync((PagedValueInquiry<Domain.Entities.ToDoList, ToDoListModel> q, CancellationToken _) =>
                {
                    return new PagedValueResult<ToDoListModel>()
                    {
                        Value = new ToDoListModel[1]
                    };
                });
            
            // Act
            var todoItems = await this.service.SearchAsync(pageMoq.Object, default);
            
            // Assert
            Assert.NotEmpty(todoItems.Value);
        }
    }
}
