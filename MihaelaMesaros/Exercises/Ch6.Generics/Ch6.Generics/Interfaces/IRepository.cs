using Ch6.Generics.Models;

namespace Ch6.Generics.Interfaces
{
    //generic interface
    public interface IRepository<T> where T : Animal //type constraint: T must be Animal or derived from Animal
    {
        void AddAnimal(T animal);
        void AddAnimal(List<T> animals); //method overload for adding list of animals

        IReadOnlyList<T> GetAnimals(); //can read, only repository can modify

        //method does not have to be generic, if uses same type T of the generic interface
        //IReadOnlyList<T> GetAnimals<T>();

    }
}
