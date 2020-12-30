using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Entities;
using Stize.ApiTemplate.Domain.Specifications;
using Stize.DotNet.Delta;
using Stize.Hosting.AspNetCore.Extensions;
using Stize.Hosting.AspNetCore.Model;
using Stize.Mapping.Service;

namespace Stize.ApiTemplate.Api.Controllers
{
    [ApiController]
    [Route("ToDoList")]
    public class ToDoListController : Controller
    {
        private readonly IMappingService<EntityDbContext> service;
        private readonly IToDoListService toDoListService;

        public ToDoListController(IMappingService<EntityDbContext> service, IToDoListService toDoListService)
        {
            this.service = service;
            this.toDoListService = toDoListService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
        {
            var models = await this.service.GetAllAsync<ToDoListModel, ToDoList>(cancellationToken);
            return this.Ok(models);
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchAsync([FromBody]PageDescriptorModel page, CancellationToken cancellationToken = default)
        {
            var models = await this.toDoListService.SearchAsync(page, cancellationToken);
            return this.PagedJsonResult(models, page.Envelope ?? false);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await this.service.FindOneAsync<ToDoListModel, ToDoList, int>(id, cancellationToken);
            if (model == null)
            {
                return this.NotFound();
            }
            return this.Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateToDoListModel model, CancellationToken cancellationToken = default)
        {
            var id = await this.service.AddAsync<CreateToDoListModel, ToDoList, int>(model, cancellationToken);
            return this.CreatedAtAction("GetAsync", new { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateToDoListModel model, CancellationToken cancellationToken = default)
        {
            var exists = await this.service.AnyAsync(new EntityByIdSpecification<ToDoList>(id), cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }
            await this.service.ApplyChangesAsync<UpdateToDoListModel, ToDoList, int>(model, cancellationToken);

            var result = await this.service.FindOneAsync<ToDoListModel, ToDoList, int>(id, cancellationToken);
            return this.Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] Delta<UpdateToDoListModel> model, CancellationToken cancellationToken = default)
        {
            var exists = await this.service.AnyAsync(new EntityByIdSpecification<ToDoList>(id), cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }

            await this.service.PatchAsync<UpdateToDoListModel, ToDoList, int>(model, cancellationToken);

            var result = await this.service.FindOneAsync<ToDoListModel, ToDoList, int>(id, cancellationToken);
            return this.Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var exists = await this.service.AnyAsync(new EntityByIdSpecification<ToDoList>(id), cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }

            await this.service.RemoveAsync<ToDoList, int>(id, cancellationToken);
            return this.Ok();
        }
    }
}