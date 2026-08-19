
namespace VariousDemos.Models
{
    [AttributeUsage(AttributeTargets.Class)] // | AttributeTargets.Method, AllowMultiple = true
    internal class DescriptionAnimalAttribute : Attribute
    {
        public string Text { get; }

        public DescriptionAnimalAttribute(string text)
        {
            Text = text;
        }
    }
}
