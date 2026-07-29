using Ch6.Generics.Interfaces;
using Ch6.Generics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ch6.Generics.Repository
{
    internal class VariantRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : Animal
    {
        private readonly List<T> _animals = new();

        public void AddAnimal(T animal)
        {
            _animals.Add(animal);
        }

        public IReadOnlyList<T> GetAnimals() //using IReadOnlyList<T> when exposing the collection
        {
            return _animals.AsReadOnly(); //can read, only repository can modify
        }


    }
}
