
namespace MiniDeepThought.Util
{
    public static class ConsoleHelpers
    {
        /// <summary>
        /// Validates string input and returns valid value. Returns null if input string is not valid.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Valid string or null if not valid.</returns>
        public static string? GetValidInputString(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input is required.");
                return null;
            }
            input = input?.Trim();

            return input;
        }

        /// <summary>
        /// Validates integer input and returns valid value. Returns null if integer input is not valid.
        /// </summary>
        /// <param name="input">Input integer.</param>
        /// <returns>Valid integer or null if not valid.</returns>
        public static int? GetValidInputInt(string? input)
        {
            string? strInput = GetValidInputString(input);
            if (strInput == null)
                return null;

            int intInput;
            if (!int.TryParse(strInput, out intInput))
            {
                Console.WriteLine("Input must be an integer number.");
                return null;
            }
            return intInput;

        }

    }
}
