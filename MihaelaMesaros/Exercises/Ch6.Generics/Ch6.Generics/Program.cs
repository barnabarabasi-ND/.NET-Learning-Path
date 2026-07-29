using Ch6.Generics.Helpers;
using Ch6.Generics.Interfaces;
using Ch6.Generics.Models;
using Ch6.Generics.Services;
using Ch6.Generics.Repository;


Console.WriteLine("Ch.6 Generics");
Console.WriteLine("---------------------------");

//generic class + generic objects + generic method DisplayTypeValue
var demo = new GenericDemo<int>(15);
Console.WriteLine(Utils.DisplayTypeValue(demo.Value));

//Reflection permits to get information about the class at runtime
var classType = demo.GetType();
Console.WriteLine("");
Console.WriteLine($"Class name: {classType.Name}");
Console.WriteLine($"IsGenericType: {classType.IsGenericType}");
Console.WriteLine($"GetGenericArguments: {string.Join(", ", classType.GetGenericArguments().Select(x => x.Name))}");
Console.WriteLine($"GetMethods: {string.Join(", ", classType.GetMethods().Select(m => m.Name))}");
Console.WriteLine($"GetProperties: {string.Join(", ", classType.GetProperties().Select(p => p.Name))}");

var someString = new GenericDemo<string>("ABC");
Console.WriteLine(Utils.DisplayTypeValue(someString.Value));

var someDouble = new GenericDemo<double>(12.345);
Console.WriteLine(Utils.DisplayTypeValue(someDouble.Value));

var someIntList = new List<int> { 1, 2, 3 };
Console.WriteLine(Utils.DisplayTypeValue<int>(someIntList));

var someStringArray = new[] { "aaa", "bbb", "ccc" };
Console.WriteLine(Utils.DisplayTypeValue<string>(someStringArray));


//generic method AddValues
Console.WriteLine("---------------------------");
Console.WriteLine("Pairs");

var pair1 = new Pair<int, int>(1, 2);
Console.WriteLine($"<{pair1.Value1}, {pair1.Value2}>");
int sumPairs1 = pair1.AddValues<int>((val1, val2) => pair1.Value1 + pair1.Value2);
Console.WriteLine($"int+int=int  ---   {sumPairs1}");

var pair2 = new Pair<int, List<int>>(10, new() {1,2,3});
Console.WriteLine($"<{pair2.Value1}, {string.Join(",", pair2.Value2)}>");
List<int> sumPairs2 = pair2.AddValues<List<int>>((val1, val2) => { val2.Add(val1); return val2; });
Console.WriteLine($"int+list=list   ---   {Utils.DisplayTypeValue<int>(sumPairs2)}");

var pair3 = new Pair<double, bool>(3.45, true);
Console.WriteLine($"<{pair3.Value1}, {pair3.Value2}>");
string sumPairs3 = pair3.AddValues<string>((val1, val2) => $"{val1} -> {val2}");
Console.WriteLine($"double+bool=string   ---   {sumPairs3}");

Console.WriteLine("Compare:");
Console.WriteLine($"1=2 {Utils.CompareValues(1, 2)}");
Console.WriteLine($"abc=abc {Utils.CompareValues("abc", "abc")}");
Console.WriteLine($"12.34=45.67 {Utils.CompareValues(12.34, 45.67)}");



//generic inheritance
Console.WriteLine("---------------------------");


var shelterDog = new ShelterDog<Dog>();
var shelterCat = new ShelterCat<Cat>();

var animal = new Animal("Some animal", 3);
//Console.WriteLine($"{animal.Name} {animal.Speak()}");

var dog = new Dog("Dog Spike", 2, true);
//Console.WriteLine($"{dog.Name} {dog.Speak()}");

var cat = new Cat("Cat Kitty", 5, 9.5);
//Console.WriteLine($"{cat.Name} {cat.Speak()}");

var animals = new List<Animal>() { animal, dog, cat };
foreach (var a in animals.OfType<Dog>())
{
    Console.WriteLine($"{a.Name} - {a.Speak()} - IsAdoptable: {shelterDog.IsAdoptable(a)} ");
}
foreach (var a in animals.OfType<Cat>())
{
    Console.WriteLine($"{a.Name} - {a.Speak()} - IsAdoptable: {shelterCat.IsAdoptable(a)}");
}

Console.WriteLine("---------------------------");
IRepository<Dog> repoDog = new Repository<Dog>();
repoDog.AddAnimal(dog);
repoDog.AddAnimal(new Dog("Dog Bruno", 10, false));
Utils.DisplayAnimals(repoDog.GetAnimals());

IRepository<Cat> repoCat = new Repository<Cat>();
repoCat.AddAnimal(cat);
repoCat.AddAnimal(new Cat("Cat Missy", 1, 0.7));

//generic method overload - adding list
repoCat.AddAnimal(new List<Cat>() { 
    new Cat("List Cat 1", 5, 3), 
    new Cat("List Cat 2", 7, 13) 
});

Utils.DisplayAnimals(repoCat.GetAnimals());

IRepository<Animal> repoAnimal = new Repository<Animal>();
repoAnimal.AddAnimal(animal);
repoAnimal.AddAnimal(dog);
repoAnimal.AddAnimal(cat);
Utils.DisplayAnimals(repoAnimal.GetAnimals());


Console.WriteLine("---------------------------");
Console.WriteLine("Using new() for creating new objects");

var dogFactory = new AnimalFactory<Dog>();
Dog newDog = dogFactory.CreateAnimal();

var catFactory = new AnimalFactory<Cat>();
Cat newCat = catFactory.CreateAnimal();

IRepository<Animal> repoAnimalF = new Repository<Animal>();
repoAnimalF.AddAnimal(newDog);
repoAnimalF.AddAnimal(newCat);

Utils.DisplayAnimals(repoAnimalF.GetAnimals());


//Variant generic interfaces
Console.WriteLine("---------------------------");
Console.WriteLine("Variant generic interfaces");

Console.WriteLine("");
Console.WriteLine("Covariance:");

//IEnumerable<out T>
List<Dog> listDogs = new() { new Dog("Dog 111", 5, true), new Dog("Dog 222", 1, false) };
IEnumerable<Animal> listAnimals = listDogs;
Utils.DisplayAnimals(listAnimals);

//IReadOnlyList<out T>
IReadOnlyList<Dog> listDogs2 = new List<Dog>() { new Dog("Dog 333", 5, true), new Dog("Dog 444", 1, false) };
IReadOnlyList<Animal> listAnimals2 = listDogs2;
Utils.DisplayAnimals(listAnimals);

//IEnumerator<out T>
IEnumerator<Dog> enumDogs = listDogs.GetEnumerator();
IEnumerator<Animal> enumAnimals = enumDogs;
while (enumAnimals.MoveNext())
{
    Animal currentAnimal = enumAnimals.Current;
    Console.WriteLine($"{currentAnimal.Name} - {currentAnimal.Speak()}");
}


//--------
var repoDogs = new VariantRepository<Dog>();
repoDogs.AddAnimal(new Dog("Dog Rex", 5, true));

var repoCats = new VariantRepository<Cat>();

//covariant type parameter, allows more derived type (Dog is more specific) can be assigned to less derived type (Animal is more general)
IReadRepository<Dog> irrDogs = repoDogs;
IReadRepository<Animal> irrAnimals = irrDogs; //covariance

Utils.PrintAnimals(irrAnimals);

//or all animals:
List<IReadRepository<Animal>> repoAllAnimals = new()
{
    repoDogs,
    repoCats
};
foreach (var repo in repoAllAnimals)
{
    Utils.PrintAnimals(repo);
}


Console.WriteLine("");
Console.WriteLine("Contravariance:");

//Action<in T>
Action<Animal> actionDisplayAnimal = animal =>
{
    Console.WriteLine($"{animal.Name} - {animal.Speak()}");
};
Action<Dog> actionDisplayDog = actionDisplayAnimal; //contravariance
actionDisplayDog(new Dog("Dog Rex (Action)", 5, true));

//Func<in T, out TResult>
Func<Animal, string> funcDisplayAnimal = animal => $"{animal.Name} - {animal.Speak()}";

Func<Dog, string> funcDisplayDog = funcDisplayAnimal; //contravariance

Console.WriteLine(funcDisplayDog(new Dog("Dog Rex (Func)", 5, true)));



//---
var repoAnimals = new VariantRepository<Animal>();
IReadRepository<Animal> repoReadAnimals = repoAnimals;

//contravariant type parameter
IWriteRepository<Animal> iwrAnimals = repoAnimals;
IWriteRepository<Cat> iwrCats = iwrAnimals; //contravariance: receives Animal, so it can receive Cat
IWriteRepository<Dog> iwrDogs = iwrAnimals;

iwrCats.AddAnimal(new Cat("Cat Kitty", 3, 2));
iwrDogs.AddAnimal(new Dog("Dog Rex", 3, false));
iwrDogs.AddAnimal(new Dog("Dog Spike", 7, true));

Utils.PrintAnimals(repoReadAnimals);




//Generic structures
Console.WriteLine("---------------------------");
Console.WriteLine("Generic structures");

var pair11 = new PairStruct<int, int>(11, 22);
PairStruct<int, int> pair11Copy;
pair11Copy = pair11;
Console.WriteLine($"Copy: <{pair11Copy.Value1}, {pair11Copy.Value2}>");

int sumPair11Copy = pair11Copy.AddValues<int>((val1, val2) => pair11Copy.Value1 + pair11Copy.Value2);
Console.WriteLine($"int+int=int  ---   {sumPair11Copy}");
