using DemoILogger.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoILogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalService _animalService;

        public AnimalsController(AnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpGet("{id}")]
        public IActionResult GetAnimal(int id)
        {
            var animal = _animalService.GetAnimalById(id);

            if (animal == null)
            {
                return NotFound($"Animal with Id {id} was not found.");
            }

            var speak = _animalService.MakeAnimalSpeak(animal);

            return Ok(new
            {
                animal.Id,
                animal.Name,
                animal.Age,
                Speak = speak
            });
        }

    }
}