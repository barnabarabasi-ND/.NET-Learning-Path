using DemoILogger.Models;

namespace DemoILogger.Services
{
    public class AnimalService
    {
        private readonly ILogger<AnimalService> _logger;

        private readonly List<Animal> _animals = new()
        {
            new Dog{Id = 1,Name = "Rex",Age = 5,IsTrained = true},
            new Cat{Id = 2,Name = "Kitty",Age = 3,IsIndoor = true},
            new Dog{Id = 3,Name = "Spike",Age = -2,IsTrained = false}
        };


        public AnimalService(ILogger<AnimalService> logger)
        {
            _logger = logger;
        }

        public Animal? GetAnimalById(int id)
        {
            _logger.LogInformation("Searching for animal with Id {Id}", id);

            Animal? animal = _animals.FirstOrDefault(a => a.Id == id);

            if (animal == null)
            {
                _logger.LogWarning("Animal with Id {Id} was not found", id);

                return null;
            }

            if (animal.Age < 0)
            {
                _logger.LogWarning("Animal {Name} has an invalid age: {Age}", animal.Name, animal.Age);
            }

            //structured logging, not string.format, no string interpolation
            _logger.LogInformation("Animal {Name} with Id {Id} was found", animal.Name, animal.Id);

            return animal;
        }

        public string MakeAnimalSpeak(Animal animal)
        {
            try
            {
                _logger.LogInformation("Animal {Name} is about to speak", animal.Name);

                if (animal.Age < 0)
                {
                    throw new ArgumentException($"Animal age cannot be negative: {animal.Age}");
                }

                string result = animal.Speak();

                _logger.LogInformation("Animal {Name} said: {Sound}", animal.Name, result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing animal {Name} with Id {Id}", animal.Name, animal.Id);
                throw;

                //_logger.LogError("Error for animal {Name}: {ErrorMessage}", animal.Name, ex.Message);
                //return "Could not make animal speak";

            }
        }
    }
}
