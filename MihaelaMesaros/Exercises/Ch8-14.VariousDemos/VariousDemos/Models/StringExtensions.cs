
namespace VariousDemos.Models
{
    public static class StringExtensions //static class!
    {
        /// <summary>
        /// Capitalizes the first letter of the string.
        /// This is an extension method for the string class. 
        /// Permits adding new methods to existing types without modifying them and without inheriting from them. 
        /// The first parameter of the method specifies the type it extends, preceded by 'this'.
        /// Useful when we can't or don't want to modify the original class, such as when working with built-in types or third-party libraries.
        /// </summary>
        /// <param name="text">The string to capitalize.</param>
        /// <returns>The capitalized string.</returns>
        public static string Capitalize(this string text) //static method with 1st parameter with this!
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return char.ToUpper(text[0]) + text.Substring(1);
        }

    }
}
