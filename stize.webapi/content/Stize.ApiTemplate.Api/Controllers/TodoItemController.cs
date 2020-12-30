using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stize.ApiTemplate.Business.Models.ToDoItem;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Entities;
using Stize.ApiTemplate.Domain.Specifications.ToDoItem;
using Stize.DotNet.Delta;
using Stize.Hosting.AspNetCore.Extensions;
using Stize.Hosting.AspNetCore.Model;
using Stize.Mapping.Service;

namespace Stize.ApiTemplate.Api.Controllers
{
    [ApiController]
    [Route("ToDoList/{todoListId}/TodoItem")]
    public class TodoItemController : ControllerBase
    {
        private readonly IMappingService<EntityDbContext> service;
        private readonly ITodoItemService todoItemService;

        public TodoItemController(IMappingService<EntityDbContext> service, ITodoItemService todoItemService)
        {
            this.service = service;
            this.todoItemService = todoItemService;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAsync(int todoListId, CancellationToken cancellationToken = default)
        {
            var result = await this.todoItemService.GetAllAsync<ToDoItemModel>(todoListId, cancellationToken);
            return this.Ok(result.Result);
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchAsync(int todoListId, [FromBody] PageDescriptorModel page, CancellationToken cancellationToken = default)
        {
            var models = await this.todoItemService.SearchAsync<ToDoItemModel>(todoListId, page, cancellationToken);
            return this.PagedJsonResult(models, page.Envelope ?? false);
        }

        [HttpGet(template: "{id}")]
        public async Task<IActionResult> GetAsync(int todoListId, int id, CancellationToken cancellationToken = default)
        {
            var model = await this.service.FindOneAsync<ToDoItemModel, ToDoItem, int>(id, cancellationToken);
            if (model == null)
            {
                return this.NotFound();
            }

            if (model.ToDoListId != todoListId)
            {
                return this.NotFound();
            }
            return this.Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(int todoListId, CreateToDoItemModel model, CancellationToken cancellationToken = default)
        {
            if (todoListId != model.ToDoListId)
            {
                return this.BadRequest("ToDoList ID are different in the route and the model");
            }
            var id = await this.service.AddAsync<CreateToDoItemModel, ToDoItem, int>(model, cancellationToken);
            return this.CreatedAtAction("GetAsync", new { todoListId = todoListId, id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int todoListId, int id, UpdateToDoItemModel model, CancellationToken cancellationToken = default)
        {
            if (id != model.Id)
            {
                return this.BadRequest("ToDoItem ID are different in the route and the model");
            }

            var exists = await this.service.AnyAsync(new ToDoItemByIdSpecification(todoListId, id), cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }
            await this.service.ApplyChangesAsync<UpdateToDoItemModel, ToDoItem, int>(model, cancellationToken);

            var result = await this.service.FindOneAsync<ToDoItemModel, ToDoItem, int>(id, cancellationToken);
            return this.Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(int todoListId, int id, Delta<UpdateToDoItemModel> model, CancellationToken cancellationToken = default)
        {
            if (id != model.GetInstance()?.Id)
            {
                return this.BadRequest("ToDoItem ID are different in the route and the model");
            }

            var exists = await this.service.AnyAsync(new ToDoItemByIdSpecification(todoListId, id), cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }

            await this.service.PatchAsync<UpdateToDoItemModel, ToDoItem, int>(model, cancellationToken);

            var result = await this.service.FindOneAsync<ToDoItemModel, ToDoItem, int>(id, cancellationToken);
            return this.Ok(result);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteAsync(int todoListId, int id, CancellationToken cancellationToken = default)
        {
            var exists = await this.service.AnyAsync(new ToDoItemByIdSpecification(todoListId, id), cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }

            await this.todoItemService.CompleteAsync(id, cancellationToken);

            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int todoListId, int id, CancellationToken cancellationToken = default)
        {
            var exists = await this.service.AnyAsync(new ToDoItemByIdSpecification(todoListId, id), cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }

            await this.service.RemoveAsync<ToDoItem, int>(id, cancellationToken);
            return this.Ok();
        }
    }
}