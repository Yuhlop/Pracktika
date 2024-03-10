using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prackticheskaya_2024;
using Prackticheskaya_2024.News;

namespace Prackticheskaya_2024.Comments
{
    [ApiController]
    [Route("comments")]
    public class CommentsController : Controller
    {
        private readonly DatabaseContext _context;

        public CommentsController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Выводит список комментариев
        /// </summary>
        /// <returns>Список новостей</returns>
        /// <response code="200">Список комментариев.</response>
        /// <response code="404">Ничего нет.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CommentsEntity>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Index()
        {
              return _context.Comments != null ? 
                          View(await _context.Comments.ToListAsync()) :
                          Problem("Entity set 'DatabaseContext.Comments'  is null.");
        }

        /// <summary>
        /// Выводит список по Id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Комментарий по id</response>
        /// <response code="404">Ничего нет.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommentsEntity), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var commentsEntity = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentsEntity == null)
            {
                return NotFound();
            }

            return View(commentsEntity);
        }

        /// <summary>
        /// Создание комментария
        /// </summary>
        /// <param name="comment"></param>
        /// <response code="201">Комментарий опубликован</response>
        /// <response code="400">Некорректный запрос</response>
        [HttpPost]
        [ProducesResponseType(typeof(CommentsEntity), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CommentsCreateDTO comment)
        {
            if (ModelState.IsValid)
            {
                var created = new CommentsEntity
                {
                    Text = comment.Text,
                    UserId  = 1 //временное решение для указания авторства
                };
                _context.Add(created);
                await _context.SaveChangesAsync();
                created.User = await _context.Users.FirstAsync(u => u.Id == created.UserId);
                return CreatedAtAction(nameof(Details), new { id = created.Id }, created);
            }
            return BadRequest();
        }


        /// <summary>
        /// Редактирование комментария
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <response code="202">Комментарий изменен</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Комментарий не найден</response>
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(CommentsEntity), 202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Edit(int id, [Bind("Text")] CommentsCreateDTO comment)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'DatabaseContext.comments' is null.");
            }

            if (ModelState.IsValid)
            {
                if (comment.Text == null)
                {
                    return BadRequest();
                }

                try
                {
                    var editable = await _context.Comments.Include(n => n.Text).FirstAsync(n => n.Id == id);
                    if (editable == null)
                    {
                        return NotFound();
                    }
                    editable.Text = comment.Text;
                    _context.Update(editable);
                    await _context.SaveChangesAsync();
                    return Accepted(editable);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _context.Comments.AnyAsync(n => n.Id == id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                } 
            }
            return BadRequest();
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id"></param>
        /// <response code="202">Новость удалена</response>
        /// <response code="404">Запись не найдена</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'DatabaseContext.Comments'  is null.");
            }
            var commentsEntity = await _context.Comments.FindAsync(id);
            if (commentsEntity != null)
            {
                _context.Comments.Remove(commentsEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentsEntityExists(int id)
        {
          return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
