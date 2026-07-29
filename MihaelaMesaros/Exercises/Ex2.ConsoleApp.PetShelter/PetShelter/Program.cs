
using PetShelter.Interfaces;
using PetShelter.Models;

int initialId = 1; //initialize first Id

List<Animal> listAnimals = new();

//initialize for tests
//AddAnimal(new Dog() { Name = "Spike", Age = 5, IntakeDate = new DateTime(2026, 1, 1), IsTrained = false });
//AddAnimal(new Cat() { Name = "Missy", Age = 7, IntakeDate = new DateTime(2025, 12, 2), IsIndoor = true });
//AddAnimal(new Bird() { Name = "Coco", Age = 10, IntakeDate = new DateTime(2026, 3, 25), WingSpanCm = 100 });
//AddAnimal(new Reptile() { Name = "Snake", Age = 7, IntakeDate = new DateTime(2026, 2, 16), IsVenomous = false });

//AddAnimal(new Dog() { Name = "Becky", Age = 2, IntakeDate = new DateTime(2026, 5, 1), IsTrained = true });
//AddAnimal(new Cat() { Name = "Kitty", Age = 11, IntakeDate = new DateTime(2025, 11, 2), IsIndoor = false });
//AddAnimal(new Bird() { Name = "Ricky", Age = 20, IntakeDate = new DateTime(2026, 1, 20), WingSpanCm = 30 });


//manage animals through a menu
while (true)
{
    Console.WriteLine("\nChoose option");
    Console.WriteLine("1) Add Dog");
    Console.WriteLine("2) Add Cat");
    Console.WriteLine("3) Add Bird");
    Console.WriteLine("4) List Animals");
    Console.WriteLine("5) Feed All");
    Console.WriteLine("6) Speak All");
    Console.WriteLine("7) Total daily care cost");
    Console.WriteLine("8) Adopt (by Id)");
    Console.WriteLine("9) Fly Birds");
    Console.WriteLine("10) Search/filter by type/name");
    Console.WriteLine("0) Exit");
    

    string? option = Console.ReadLine();

    if (option != null)
    {
        option = option.Trim();
        switch (option)
        {
            case "1":
                AddAnimal(ReadAnimal(typeof(Dog)));
                break;
            case "2":
                AddAnimal(ReadAnimal(typeof(Cat)));
                break;
            case "3":
                AddAnimal(ReadAnimal(typeof(Bird)));
                break;
            case "4":
                DisplayListAnimals();
                break;
            case "5":
                FeedAll();
                break;
            case "6":
                SpeakAll();
                break;
            case "7":
                CalculateTotalDailyCareCost();
                break;
            case "8":
                AdoptAnimal();
                break;
            case "9":
                FlyBirds();
                break;
            case "10":
                SearchAnimal();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid option");
                break;
        }
    }
}


#region Methods implementation

void AddAnimal(Animal animal)
{
    animal.Id = initialId++; //increment Id for new animal
    listAnimals.Add(animal);
}

/// <summary>
/// Read values from input and creates new animal object.
/// </summary>
/// <param name="typeAnimal">Type of object: Dog, Cat, Bird.</param>
/// <returns>New Animal object.</returns>
Animal ReadAnimal(Type typeAnimal)
{
    string? input;

    Console.WriteLine("Name: ");
    string name;
    while (true)
    {
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Name is required.");
            continue;
        }
        name = input.Trim();
        break;
    }

    Console.WriteLine("Age: ");
    int age;
    while (true)
    {
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Age is required.");
            continue;
        }
        if (!int.TryParse(input, out age) || age < 0 || age > 100)
        {
            Console.WriteLine("Age must be number 0-100.");
            continue;
        }
        break;
    }

    if (typeAnimal == typeof(Dog))
    {
        Console.WriteLine("Is trained (y/n): ");
        bool isTrained;
        while (true)
        {
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Is trained is required.");
                continue;
            }
            if (!input.Equals("y", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("n", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }
            isTrained = input.Equals("y", StringComparison.InvariantCultureIgnoreCase);
            break;
        }

        return new Dog()
        {
            Name = name,
            Age = age,
            IsTrained = isTrained
        };
    }
    else if (typeAnimal == typeof(Cat))
    {
        Console.WriteLine("Is indoor (y/n): ");
        bool isIndoor;
        while (true)
        {
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Is indoor is required.");
                continue;
            }
            if (!input.Equals("y", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("n", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }
            isIndoor = input.Equals("y", StringComparison.InvariantCultureIgnoreCase);

            break;
        }

        return new Cat()
        {
            Name = name,
            Age = age,
            IsIndoor = isIndoor
        };
    }
    else if (typeAnimal == typeof(Bird))
    {
        Console.WriteLine("Wing span cm: ");
        int wingSpanCm;
        while (true)
        {
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Wing span cm is required.");
                continue;
            }
            if (!int.TryParse(input, out wingSpanCm) || wingSpanCm < 0 || wingSpanCm > 10000)
            {
                Console.WriteLine("Invalid input for wing span cm (0-10000).");
                continue;
            }
            break;
        }

        return new Bird()
        {
            Name = name,
            Age = age,
            WingSpanCm = wingSpanCm
        };
    }

    throw new ArgumentException("Unsupported animal type.");
}

/// <summary>
/// Displays list of animals.
/// </summary>
/// <param name="filter">Optional; if present, it filters the list by type d/c/b/r or name.</param>
void DisplayListAnimals(string? filter = null)
{
    var filteredAnimals = listAnimals;

    //if filter/search applies, filter list
    if (!string.IsNullOrWhiteSpace(filter))
    {
        filteredAnimals = filteredAnimals.Where(x =>
            (x.GetType() == typeof(Dog) && filter.Equals("d", StringComparison.InvariantCultureIgnoreCase))
            || (x.GetType() == typeof(Cat) && filter.Equals("c", StringComparison.InvariantCultureIgnoreCase))
            || (x.GetType() == typeof(Bird) && filter.Equals("b", StringComparison.InvariantCultureIgnoreCase))
            || (x.GetType() == typeof(Reptile) && filter.Equals("r", StringComparison.InvariantCultureIgnoreCase))
            || x.Name.Equals(filter, StringComparison.InvariantCultureIgnoreCase)
        ).ToList();
    }

    //iterate filtered list
    foreach (Animal animal in filteredAnimals)
    {
        Console.WriteLine($"\nType: {animal.GetType().Name}");
        Console.WriteLine($"Id: {animal.Id}");
        Console.WriteLine($"Name: {animal.Name}");
        Console.WriteLine($"Age: {animal.Age}");
        if (animal.IntakeDate > DateTime.MinValue)
        {
            Console.WriteLine($"Intake date: {animal.IntakeDate.ToString("dd.MM.yyyy")}");
        }
        Console.WriteLine($"Daily care cost: {animal.DailyCareCost():F2}"); //sau animal.DailyCareCost().ToString("F2")

        if (animal is Dog dog)
        {
            Console.WriteLine($"Is trained: {dog.IsTrained}");
        }
        else if (animal is Cat cat)
        {
            Console.WriteLine($"Is indoor: {cat.IsIndoor}");
        }
        else if (animal is Bird bird)
        {
            Console.WriteLine($"Wing span cm: {bird.WingSpanCm}");
        }

    }
}

/// <summary>
/// Returns number of fed animals, only fedable animals.
/// </summary>
void FeedAll()
{
    int noFedAnimals = 0;
    foreach (Animal animal in listAnimals)
    {
        if (animal is IFeedable feedable)
        {
            feedable.Feed();
            noFedAnimals++;
        }
    }
    Console.WriteLine($"Number of fed animals: {noFedAnimals}");
}

void SpeakAll()
{
    foreach (Animal animal in listAnimals)
    {
        animal.Speak();
    }
}

/// <summary>
/// Removes the animal specified by Id from animals list, it is considered as adopted.
/// </summary>
void AdoptAnimal()
{
    int adoptedId;

    Console.WriteLine("Enter Id of animal to be adopted:");
    while (true)
    {
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Id is required.");
            continue;
        }
        input = input.Trim();

        if (!int.TryParse(input, out adoptedId) || adoptedId <= 0 || adoptedId > int.MaxValue)
        {
            Console.WriteLine("Id is not valid.");
            continue;
        }
        if (!listAnimals.Any())
        {
            Console.WriteLine("List of animals is empty.");
            continue;
        }
        if (!listAnimals.Any(x => x.Id == adoptedId)) //keep this as main validation Id existence, to be able to enter Id again
        {
            Console.WriteLine("Animal not found.");
            continue;
        }
        break;
    }


    //remove from list if adopted
    //listAnimals.RemoveAll(x => x.Id == adoptedId);

    //or search first
    var adoptedAnimal = listAnimals.FirstOrDefault(x => x.Id == adoptedId);
    if (adoptedAnimal == null)
    {
        Console.WriteLine("Animal not found (null).");
    }
    else
    {
        listAnimals.Remove(adoptedAnimal);
        Console.WriteLine($"{adoptedAnimal.GetType().Name} {adoptedAnimal.Name} has been adopted.");
    }
}

void FlyBirds()
{
    foreach (var animal in listAnimals)
    {
        if (animal is IFlyable flyable)
        {
            flyable.Fly();
        }
    }
}

/// <summary>
/// Get input type/name filter and display filtered list of animals.
/// </summary>
void SearchAnimal()
{
    Console.WriteLine("\nChoose an option or enter animal name:");
    Console.WriteLine("d) Dog");
    Console.WriteLine("c) Cat");
    Console.WriteLine("b) Bird");
    Console.WriteLine("r) Reptile");

    string? input;
    while (true)
    {
        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Filter option or search text is required.");
            continue;
        }
        input = input.Trim();
        break;
    }

    DisplayListAnimals(input);
}

/// <summary>
/// Returns the total cost for all animals.
/// </summary>
void CalculateTotalDailyCareCost()
{
    decimal totalDailyCareCost = listAnimals.Sum(x => x.DailyCareCost());
    Console.WriteLine($"Total: {totalDailyCareCost:F2}");
}
#endregion