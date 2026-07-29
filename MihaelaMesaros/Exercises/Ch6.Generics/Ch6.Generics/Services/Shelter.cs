using Ch6.Generics.Models;

namespace Ch6.Generics.Services
{
    public class Shelter<T> where T : Animal, IComparable<T> //added interface constraint for age comparition with CompareTo
    {
        public virtual bool IsAdoptable(T animal)
        {
            return false;
        }
    }
}
