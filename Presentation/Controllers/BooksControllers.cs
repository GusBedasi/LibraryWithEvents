using Domain;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("core/v1/books")]
    public class BooksControllers : ControllerBase
    {
        private readonly IRabbitProducer<Book> _producer;

        public BooksControllers(IRabbitProducer<Book> producer) => (_producer) = producer;

        [HttpPost]
        public IActionResult CreateBook([FromBody] CreateBookRequest request)
        {
            var book = new Book()
            {
                Title = request.Title
            };

            _producer.Publish(book);

            return Ok(book);
        }
    }
}
