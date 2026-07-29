using PetShelter.Interfaces;
using PetShelter.Models;
using PetShelter.UI;

namespace PetShelter;

internal sealed class PetShelterApp
{
    private const int AnimalTableWidth = 105;
    private const int IdWidth = -5;
    private const int TypeWidth = -12;
    private const int NameWidth = -18;
    private const int AgeWidth = -7;
    private const int IntakeDateWidth = -22;
    private const int ExtraWidth = -29;
    private const int DailyCareCostWidth = 12;

    private static readonly string AnimalTableSeparator = new('-', AnimalTableWidth);
    private static readonly string MenuSeparator = new('-', 50);

    private readonly Dictionary<string, MenuOption> _menuOptions;
    private readonly List<Animal> _animals = [];

    private int _nextAnimalId = 1;
    private bool _isRunning = false;

    public PetShelterApp()
    {
        _menuOptions = new()
        {
            ["1"] = new("Add Dog", AddDog),
            ["2"] = new("Add Cat", AddCat),
            ["3"] = new("Add Bird", AddBird),
            ["4"] = new("List Animals", ListAnimals),
            ["5"] = new("Feed All", FeedAll),
            ["6"] = new("Speak All", SpeakAll),
            ["7"] = new("Adopt Animal", AdoptAnimal),
            ["8"] = new("Fly Birds", FlyBirds),
            ["9"] = new("Exit", Exit)
        };
    }

    private static string? ReadUserInput()
    {
        return Console.ReadLine()?.Trim();
    }

    private static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadUserInput();

            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }

            Console.WriteLine("The value cannot be empty.");
        }
    }

    private static int ReadNonNegativeInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadUserInput();

            if (int.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Please enter 0 or a positive whole number.");
        }
    }

    private static double ReadPositiveDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadUserInput();

            if (double.TryParse(input, out var value) && value > 0d)
            {
                return value;
            }

            Console.WriteLine("Please enter a positive number.");
        }
    }

    private static bool ReadBool(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = ReadUserInput();

            if (string.Equals(input, "y", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(input, "n", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Console.WriteLine("Please enter 'y' or 'n'.");
        }
    }

    private static (string name, int age) ReadBaseAnimalProperties()
    {
        var name = ReadRequiredString("Name: ");
        var age = ReadNonNegativeInt("Age: ");

        return (name, age);
    }

    private static void WriteMenuSeparator()
    {
        Console.WriteLine();
        Console.WriteLine(MenuSeparator);
    }

    private static void WriteAnimalTableHeader()
    {
        Console.WriteLine(
            $"{"Id", IdWidth}" +
            $"{"Type", TypeWidth}" +
            $"{"Name", NameWidth}" +
            $"{"Age", AgeWidth}" +
            $"{"IntakeDate", IntakeDateWidth}" +
            $"{"Extra", ExtraWidth}" +
            $"{"DailyCost", DailyCareCostWidth}"
        );

        Console.WriteLine(AnimalTableSeparator);
    }

    private static string ToYesOrNo(bool value)
    {
        return value ? "Yes" : "No";
    }

    private static string GetTypeSpecificAnimalExtraDetails(Animal animal)
    {
        return animal switch
        {
            Dog dog => $"Trained: {ToYesOrNo(dog.IsTrained)}",

            Cat cat => $"Indoor: {ToYesOrNo(cat.IsIndoor)}",

            Bird bird => $"Wing span: {bird.WingSpanCm:F2} cm",

            _ => string.Empty
        };
    }

    private static void WriteAnimalTableRow(Animal animal)
    {
        var extraDetails = GetTypeSpecificAnimalExtraDetails(animal);
        var intakeDate = animal.IntakeDate.ToString("yyyy-MM-dd HH:mm:ss");

        Console.WriteLine(
            $"{animal.Id, IdWidth}" +
            $"{animal.GetType().Name, TypeWidth}" +
            $"{animal.Name, NameWidth}" +
            $"{animal.Age, AgeWidth}" +
            $"{intakeDate, IntakeDateWidth}" +
            $"{extraDetails, ExtraWidth}" +
            $"{animal.DailyCareCost(), DailyCareCostWidth:F2}"
        );
    }

    private void WriteAnimalTable()
    {
        WriteAnimalTableHeader();

        foreach (var animal in _animals)
        {
            WriteAnimalTableRow(animal);
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== MAIN MENU ===");

        foreach (var (key, option) in _menuOptions)
        {
            Console.WriteLine($"{key}. {option.Title}");
        }

        Console.WriteLine();
        Console.Write("Select an option: ");
    }

    private void AddAnimal(Animal animal)
    {
        _animals.Add(animal);
        _nextAnimalId++;
    }

    private void AddDog()
    {
        Console.WriteLine("=== ADD DOG ===");
        Console.WriteLine();

        var (name, age) = ReadBaseAnimalProperties();
        var isTrained = ReadBool("Is the dog trained? (y/n): ");

        Console.WriteLine();

        try
        {
            var dog = new Dog(_nextAnimalId, name, age, isTrained);

            AddAnimal(dog);

            Console.WriteLine($"Dog {dog.Name} was registered with ID {dog.Id}.");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"The dog could not be added: {exception.Message}");
        }
    }

    private void AddCat()
    {
        Console.WriteLine("=== ADD CAT ===");
        Console.WriteLine();

        var (name, age) = ReadBaseAnimalProperties();
        var isIndoor = ReadBool("Is the cat an indoor cat? (y/n): ");

        Console.WriteLine();

        try
        {
            var cat = new Cat(_nextAnimalId, name, age, isIndoor);

            AddAnimal(cat);

            Console.WriteLine($"Cat {cat.Name} was registered with ID {cat.Id}.");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"The cat could not be added: {exception.Message}");
        }
    }

    private void AddBird()
    {
        Console.WriteLine("=== ADD BIRD ===");
        Console.WriteLine();

        var (name, age) = ReadBaseAnimalProperties();
        var wingSpanCm = ReadPositiveDouble("Wing span in centimeters: ");

        Console.WriteLine();

        try
        {
            var bird = new Bird(_nextAnimalId, name, age, wingSpanCm);

            AddAnimal(bird);

            Console.WriteLine($"Bird {bird.Name} was registered with ID {bird.Id}.");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"The bird could not be added: {exception.Message}");
        }
    }

    private void ListAnimals()
    {
        Console.WriteLine("=== ANIMALS ===");
        Console.WriteLine();

        if (_animals.Count == 0)
        {
            Console.WriteLine("No animals are currently registered.");
            return;
        }

        WriteAnimalTable();
    }

    private int FeedAnimals()
    {
        var fedAnimalCount = 0;

        foreach (var animal in _animals)
        {
            if (animal is IFeedable feedable)
            {
                feedable.Feed();
                fedAnimalCount++;
            }
        }

        return fedAnimalCount;
    }

    private void FeedAll()
    {
        Console.WriteLine("=== FEED ALL ===");
        Console.WriteLine();

        if (_animals.Count == 0)
        {
            Console.WriteLine("There are no animals to feed.");
            return;
        }

        var fedAnimalCount = FeedAnimals();

        Console.WriteLine();
        Console.WriteLine($"{fedAnimalCount} animal(s) were fed.");
    }

    private void SpeakAll()
    {
        Console.WriteLine("=== SPEAK ALL ===");
        Console.WriteLine();

        if (_animals.Count == 0)
        {
            Console.WriteLine("There are no animals in the shelter.");
            return;
        }

        foreach (var animal in _animals)
        {
            Console.Write($"{animal.Name}: ");
            animal.Speak();
        }
    }

    private void FlyBirds()
    {
        Console.WriteLine("=== FLY BIRDS ===");
        Console.WriteLine();

        var hasFlyable = false;

        foreach (var animal in _animals)
        {
            if (animal is IFlyable flyable)
            {
                flyable.Fly();
                hasFlyable = true;
            }
        }

        if (!hasFlyable)
        {
            Console.WriteLine("There are no birds that can fly.");
        }
    }

    private Animal? FindAnimalById(int id)
    {
        return _animals.FirstOrDefault(animal => animal.Id == id);
    }

    private void AdoptAnimal()
    {
        Console.WriteLine("=== ADOPT ANIMAL ===");
        Console.WriteLine();

        if (_animals.Count == 0)
        {
            Console.WriteLine("There are no animals available for adoption.");
            return;
        }

        var animalId = ReadNonNegativeInt("Animal ID: ");
        var animal = FindAnimalById(animalId);

        Console.WriteLine();

        if (animal is null)
        {
            Console.WriteLine("Animal not found.");
            return;
        }

        _animals.Remove(animal);
        Console.WriteLine($"{animal.GetType().Name} {animal.Name} has been adopted.");
    }

    private void Exit()
    {
        _isRunning = false;
        Console.WriteLine("The application is closing.");
    }

    public void Run()
    {
        _isRunning = true;
        Console.WriteLine("Pet Shelter Management System");

        while (_isRunning)
        {
            DisplayMenu();
            var selectedKey = ReadUserInput();

            if (string.IsNullOrEmpty(selectedKey))
            {
                Console.WriteLine("Please enter exactly one menu option.");
                WriteMenuSeparator();
                continue;
            }

            if (!_menuOptions.TryGetValue(selectedKey, out var option))
            {
                Console.WriteLine("Invalid menu option.");
                WriteMenuSeparator();
                continue;
            }

            Console.WriteLine();
            option.Execute();
            WriteMenuSeparator();
        }
    }
}
