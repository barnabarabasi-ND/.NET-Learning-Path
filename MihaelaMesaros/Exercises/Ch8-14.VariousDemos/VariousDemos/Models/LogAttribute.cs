
namespace VariousDemos.Models
{
    internal class LogAttribute : Attribute
    {
        public string Message { get; }

        public LogAttribute(string message)
        {
            Message = message;
        }
    }
}
