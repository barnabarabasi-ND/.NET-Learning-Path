using Ch6.Generics.Models;

namespace Ch6.Generics.Services
{
    //generic inheritance + generic override
    public class ShelterCat<T> : Shelter<T> where T : Cat
    {
        public override bool IsAdoptable(T animal)
        {
            return (animal.Kg < 10);
        }
    }
}
