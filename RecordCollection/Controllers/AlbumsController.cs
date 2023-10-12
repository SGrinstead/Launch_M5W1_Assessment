using Microsoft.AspNetCore.Mvc;
using RecordCollection.DataAccess;
using RecordCollection.Models;

namespace RecordCollection.Controllers
{
    public class AlbumsController : Controller {

        private readonly RecordCollectionContext _context;
        private readonly Serilog.ILogger _logger;

        public AlbumsController(RecordCollectionContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var albums = _context.Albums.ToList();
            return View(albums);
        }

        [Route("/albums/{id:int}")]
        public IActionResult Show(int? id)
        {
            if(id is null)
            {
                _logger.Warning("Invalid id");
                return BadRequest();
            }

            var album = _context.Albums.FirstOrDefault(a => a.Id == id);

            if(album is null)
            {
				_logger.Warning("Album not found");
				return NotFound();
			}

            return View(album);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Album album)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid album model state, redirecting to new");
                return RedirectToAction(nameof(New));
            }
            _context.Albums.Add(album);
            _context.SaveChanges();

            _logger.Information("this is the create action");
            _logger.Information("new album saved to database");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("/albums/{id:int}")]
        public IActionResult Delete(int? id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);
            _context.Albums.Remove(album);
            _context.SaveChanges();

            _logger.Fatal($"Success! {album.Title} was removed from the database.");

            return RedirectToAction(nameof(Index));
        }
    }
}
