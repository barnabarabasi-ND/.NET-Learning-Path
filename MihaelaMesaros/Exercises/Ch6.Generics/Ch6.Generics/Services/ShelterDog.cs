using Ch6.Generics.Models;

namespace Ch6.Generics.Services
{
    //generic inheritance + generic override
    public class ShelterDog<T> : Shelter<T> where T : Dog
    {
        public override bool IsAdoptable(T animal)
        {
            return (animal.IsTrained == true);
        }
    }
}
