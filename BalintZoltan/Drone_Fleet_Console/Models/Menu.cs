namespace Drone_Fleet_Console.Models;

public class Menu
{
    private List<string> _menuOptions = new List<string>();
    private string _caption = string.Empty;
    public void AddCaption(string text)
    {
        _caption = text;
    }

    public void AddOption(string option)
    {
        _menuOptions.Add(option);
    }
    public void DisplayMenu()
    {
        if (_menuOptions.Count > 0)
        {
            Console.Clear();
            Console.WriteLine($"********** - {_caption} - **********");
            for (int i = 0; i < _menuOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_menuOptions[i]}");
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
        return int.TryParse(Console.ReadLine(), out int result) ? result : 0;
    }
}

