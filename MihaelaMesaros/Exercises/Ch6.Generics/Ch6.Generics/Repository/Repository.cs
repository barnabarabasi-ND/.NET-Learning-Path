using Ch6.Generics.Interfaces;
using Ch6.Generics.Models;

namespace Ch6.Generics.Repository
{
    //generic class inherits from generic interface
    public class Repository<T> : IRepository<T> where T : Animal //must have the same constraint as the class
    {
        private readonly List<T> _animals = new();

        public void AddAnimal(T animal)
        {
            _animals.Add(animal);
        }

        //method overload
        public void AddAnimal(List<T> animals)
        {
            _animals.AddRange(animals);
        }

        public IReadOnlyList<T> GetAnimals() //using IReadOnlyList<T> when exposing the collection
        {
            return _animals.AsReadOnly(); //can read, only repository can modify
        }

    }
}
