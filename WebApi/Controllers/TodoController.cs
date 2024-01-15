using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using System.Runtime.InteropServices;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        // 의존성 주입 로거, DB컨텍스트
        private readonly ILogger<TodoController> _logger;
        private readonly DBContext _dbContext;

        public TodoController(ILogger<TodoController> logger, DBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoList(int userId)
        {
            try
            {
                var todoList = await _dbContext.Todos.
                    Where(t => t.UserId == userId).
                    Select(t => new
                    {
                        t.TodoNo,
                        t.TodoContent,
                        t.TodoStatus
                    }).
                    ToListAsync();

                return Ok(todoList);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PutTodo(Todo model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbContext.Todos.AddAsync(model);

                    if(await _dbContext.SaveChangesAsync() > 0)
                    {
                        return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{todoNo}/{userId}")]
        public async Task<IActionResult> DeleteTodo(int todoNo, int userId)
        {
            try
            {
                var todo = await _dbContext.Todos.FirstOrDefaultAsync(t => t.TodoNo == todoNo && t.UserId == userId);

                if(todo == null)
                {
                    return BadRequest();
                }

                _dbContext.Todos.Remove(todo);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTodo(Todo model)
        {
            try
            {
                var todo = await _dbContext.Todos.FirstOrDefaultAsync(t => t.TodoNo == model.TodoNo && t.UserId == model.UserId);

                if (todo == null)
                {
                    return BadRequest();
                }

                if(model.TodoContent != null)
                {
                    todo.TodoContent = model.TodoContent;
                }
                if(model.TodoStatus != -1)
                {
                    todo.TodoStatus = model.TodoStatus;
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
