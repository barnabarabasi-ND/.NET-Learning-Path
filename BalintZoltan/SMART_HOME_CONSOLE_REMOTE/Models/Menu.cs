public class Menu()
{
    public List<string> MenuOptions = new List<string>();
    public int choice;
    public string caption = string.Empty;
    public void AddCaption(string text)
    {
        caption = text;
    }
    public void AddOption(string option)
    {
        MenuOptions.Add(option);
    }
    public void DisplayMenu()
    {
        Console.WriteLine("Please select an option:");
        if (MenuOptions.Count > 0)
        {
            Console.Clear();
            Console.WriteLine($"********** - {caption} - **********");
            for (int i = 0; i < MenuOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {MenuOptions[i]}");
            }
            Console.Write("Select: ");
        }
        else
        {
            Console.WriteLine("No options available.");
        }
    }
    public int ReadChoice()
    {
        choice = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
        return choice;
    }
}
