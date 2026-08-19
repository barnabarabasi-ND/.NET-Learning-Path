
using VariousDemos.Services;

namespace VariousDemos.Models
{
    public static class AnimalExtensions
    {
        public static string SpeakLouder(this Animal animal)
        {
            return animal.Speak().ToUpper();
        }
    }
}
