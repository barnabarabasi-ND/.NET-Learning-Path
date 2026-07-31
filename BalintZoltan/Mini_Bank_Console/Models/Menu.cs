namespace MiniBankConsole.Models;

public class Menu()
{

    // field : _<field_name>
    public List<string> MenuOptions = new List<string>();
    public int choice;      // property sau metoda statica 
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
        Console.WriteLine("Please select an option:"); // nu are sens
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

