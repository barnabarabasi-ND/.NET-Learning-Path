namespace Ch6.Generics.Services
{
    public class AnimalFactory<T> where T : new() //constraint used creating generic objects, without knowing the type of object at compile time; the constructor in object class must exist
    {
        public T CreateAnimal()
        {
            return new T();
        }
    }
}
