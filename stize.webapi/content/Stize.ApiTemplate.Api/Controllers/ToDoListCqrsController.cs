using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Entities;
using Stize.ApiTemplate.Domain.Specifications;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.CQRS.EntityFrameworkCore.Query;
using Stize.DotNet.Delta;
using Stize.DotNet.Result.Reasons;
using Stize.DotNet.Search.Model;
using Stize.Hosting.AspNetCore.Extensions;
using Stize.Mapping.Service;
using Stize.Mediator;

namespace Stize.ApiTemplate.Api.Controllers
{
    [ApiController]
    [Route("ToDoListCqrs")]
    public class ToDoListCqrsController : Controller
    {

        private readonly IMediator mediator;

        public ToDoListCqrsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
        {
            var request = new GetAllModelsFromEntityQuery<ToDoListModel, ToDoList, int, EntityDbContext>();
            var result = await mediator.HandleAsync(request, cancellationToken);


            return this.Ok(result.Value);
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchAsync([FromBody] PageDescriptorModel page, CancellationToken cancellationToken = default)
        {
            var request = new GetAllModelsFromEntityByPageQuery<ToDoListModel, ToDoList, int, EntityDbContext>(page);
            var result = await mediator.HandleAsync(request, cancellationToken);
            return this.PagedJsonResult(result, page.Envelope ?? false);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var request = new GetModelFromEntityByIdQuery<ToDoListModel, ToDoList, int, EntityDbContext>(id);
            var result = await mediator.HandleAsync(request, cancellationToken);

            if (result.Value == null)
            {
                return this.NotFound();
            }
            return this.Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateToDoListModel model, CancellationToken cancellationToken = default)
        {
            var request = new CreateEntityFromModelCommand<CreateToDoListModel, ToDoList, int, EntityDbContext>(model);
            var result = await mediator.HandleAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                var problemDetails = new ProblemDetails();
                problemDetails.Extensions.Add(nameof(Reason), result.Reasons);

                return new BadRequestObjectResult(problemDetails);
            }
            return this.CreatedAtAction("GetAsync", new { Id = result.Value });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateToDoListModel model, CancellationToken cancellationToken = default)
        {
            if (id != model.Id)
            {
                return this.BadRequest("ToDoList ID is different in the route and the model");
            }

            var request = new UpdateEntityFromModelCommand<UpdateToDoListModel, ToDoList, int, EntityDbContext>(model);
            var result = await mediator.HandleAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                var problemDetails = new ProblemDetails();
                problemDetails.Extensions.Add(nameof(Reason), result.Reasons);

                return new BadRequestObjectResult(problemDetails);
            }

            return this.Ok(result.Value);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] Delta<UpdateToDoListModel> model, CancellationToken cancellationToken = default)
        {
            if (!model.TryGetPropertyValue(nameof(UpdateToDoListModel.Id), out object idValue) || !(idValue is int modelId) || (id != modelId))
            {
                return this.BadRequest("ToDoList ID is different in the route and the model");
            }

            var request = new PatchEntityFromModelCommand<UpdateToDoListModel, ToDoList, int, EntityDbContext>(model);
            var result = await mediator.HandleAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                var problemDetails = new ProblemDetails();
                problemDetails.Extensions.Add(nameof(Reason), result.Reasons);

                return new BadRequestObjectResult(problemDetails);
            }

            return this.Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var request = new DeleteEntityByIdCommand<ToDoList, int, EntityDbContext>(id);
            var result = await mediator.HandleAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                var problemDetails = new ProblemDetails();
                problemDetails.Extensions.Add(nameof(Reason), result.Reasons);

                return new BadRequestObjectResult(problemDetails);
            }
            return this.Ok();
        }
    }
}