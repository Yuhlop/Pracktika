using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Prackticheskaya_2024.News
{
    [ApiController]
    [Route("news")]
    public class NewsController : Controller
    {
        private readonly DatabaseContext _context;
        public NewsController(DatabaseContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Выводит список новостей
        /// </summary>
        /// <returns>Список новостей</returns>
        /// <response code="200">Список новостей.</response>
        /// <response code="404">Ничего нет.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<NewsEntity>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Index()
        {
            return _context.News != null
            ? Ok(await _context.News.Include(n => n.Author).OrderByDescending(n =>
            n.CreatedAt).Take(100).ToListAsync())
            : NotFound();
        }
        /// <summary>
        /// Выводит новость по id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Новость по id</response>
        /// <response code="404">Ничего нет.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NewsEntity), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }
            var item = await _context.News.Include(n => n.Author).FirstOrDefaultAsync(n => n.Id == id);
            return item != null ? Ok(item) : NotFound();
        }
        /// <summary>
        /// Добавление новости
        /// </summary>
        /// <param name="item">Публикуемая статья</param>
        /// <response code="201">Новость опубликована</response>
        /// <response code="400">Некорректный запрос</response>
        [HttpPost]
        [ProducesResponseType(typeof(NewsEntity), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] NewsCreateDTO item)
        {
            if (ModelState.IsValid)
            {
                var created = new NewsEntity
                {
                    Title = item.Title,
                    Content = item.Content,
                    AuthorId = 1 //временное решение для указания авторства
                };
                _context.Add(created);
                await _context.SaveChangesAsync();
                //поиск автора в списке юзеров
                created.Author = await _context.Users.FirstAsync(u => u.Id == created.AuthorId);
                return CreatedAtAction(nameof(Details), new { id = created.Id }, created);
            }
            return BadRequest();
        }
        /// <summary>
        /// Изменение новости
        /// </summary>
        /// <param name="id">ID новости</param>
        /// <param name="item">Редактиремая статья</param>
        /// <response code="202">Новость изменена</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Запись не найдена</response>
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(NewsEntity), 202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Edit(int id, [FromBody] NewsCreateDTO item)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'DatabaseContext.news' is null.");
            }
            if (ModelState.IsValid)
            {
                if (item.Content == null && item.Title == null)
                {
                    return BadRequest();
                }
                try
                {
                    var editable = await _context.News.Include(n => n.Author).FirstAsync(n => n.Id == id);
                    if (editable == null)
                    {
                        return NotFound();
                    }
                    editable.Title = item.Title;
                    editable.Content = item.Content;
                    _context.Update(editable);
                    await _context.SaveChangesAsync();
                    return Accepted(editable);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _context.News.AnyAsync(n => n.Id == id)))
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
        /// Удаление новости
        /// </summary>
        /// <param name="id">ID записи</param>
        /// <response code="202">Новость удалена</response>
        /// <response code="404">Запись не найдена</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'DatabaseContext.news' is null.");
            }
            var item = await _context.News.FindAsync(id);
            if (item != null)
            {
                _context.News.Remove(item);
                await _context.SaveChangesAsync();
                return Accepted();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

