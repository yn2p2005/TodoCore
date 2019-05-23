using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExLibrary;
using TodoApi.Services; 

namespace TodoApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApiServiceWrapper _apiServiceWrappper; 
        private readonly TodoContext _context;
        public TodoController(TodoContext context, ApiServiceWrapper apiServiceWrappper)
        {
            _context = context;
            _apiServiceWrappper = apiServiceWrappper; 

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.TodoItems.Add(new TodoItem { Name = "Item2" });
                _context.SaveChanges();
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<TodoItem>>> GetTodotItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItemById(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
                return NotFound();

            return todoItem;
        }

        [Route("hello")]
        [HttpGet]
        public async Task<ActionResult<string>> GetHelloWorld()
        {
            return await _apiServiceWrappper.getApiHelloService();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItemById), new { id = item.Id }, item);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodoItem(long id, TodoItem item)
        {
                if(id != item.Id)
                return BadRequest(); 

                _context.Entry(item).State  = EntityState.Modified;
                await _context.SaveChangesAsync(); 

                return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoItem(long id)
        {
          var todoItem = await _context.TodoItems.FindAsync(id); 
          if(todoItem == null)
          return NotFound();  

            _context.TodoItems.Remove(todoItem); 
            await _context.SaveChangesAsync(); 

            return NoContent(); 

        }

    }
}