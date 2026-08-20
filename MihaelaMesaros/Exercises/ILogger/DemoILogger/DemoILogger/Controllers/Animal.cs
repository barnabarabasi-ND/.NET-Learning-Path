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

        //[HttpGet("getdog")]
        //public IActionResult GetDog()
        //{
        //    Animal animal = new Dog
        //    {
        //        Name = "Rex",
        //        Age = 5,
        //        IsTrained = true
        //    };

        //    string result = _animalService.MakeAnimalSpeak(animal);

        //    return Ok(result);
        //}

        //[HttpGet("getcat")]
        //public IActionResult GetCat()
        //{
        //    Animal animal = new Cat
        //    {
        //        Name = "Kitty",
        //        Age = 3,
        //        IsIndoor = true
        //    };

        //    string result = _animalService.MakeAnimalSpeak(animal);

        //    return Ok(result);
        //}
    }
}