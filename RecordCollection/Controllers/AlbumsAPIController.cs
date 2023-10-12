using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordCollection.DataAccess;

namespace RecordCollection.Controllers
{
    [Route("api/albums")]
    [ApiController]
    public class AlbumsAPIController : ControllerBase
    {
        private readonly RecordCollectionContext _context;
		private readonly Serilog.ILogger _logger;

		public AlbumsAPIController(RecordCollectionContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult GetAll()
        {
            var albums = _context.Albums.ToList();
            return new JsonResult(albums);
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);

            if(album is null)
            {
                _logger.Warning("Album not found");
                return NotFound();
            }

            return new JsonResult(album);
        }

        [HttpDelete("{id}")]
        public void DeleteOne(int id)
        {
            var album = _context.Albums.FirstOrDefault(a => a.Id == id);

			if (album is null)
			{
				_logger.Warning("Album not found");
			}
            else
            {
				_context.Albums.Remove(album);
				_context.SaveChanges();
				_logger.Fatal($"Success! {album.Title} was removed from the database.");
			}
		}
	}
}
