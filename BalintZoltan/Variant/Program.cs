using Variant.Models;
using Variant.Interfaces;

Dog dog = new Dog("Alpha");                          //  Dog --> Animal
Animal animal2 = dog;

List<Dog> dog2 = new List<Dog>();
// List<Animal> animals = dog2;                     // Compiler error
// animals.Add(new Cat());
// List<Dog>: Dog    Dog    Cat < --error

/**************************************
* covariant type parameter
**************************************/

List<Dog> dogList = new()
{
    new Dog("Beta dog"),
    new Dog("Gamma dog")
};

IEnumerable<Dog> dogs = dogList;

// dogs.Add(new Dog("Teta"));                       // Compiler error : dogs:IEnumerable no Add() method.

List<Cat> catList = new()
{
    new Cat("Alpha cat"),
    new Cat("Beta cat")
};

IEnumerable<Cat> cats = catList;

    //IEnumerable<Animal> animals = dogs;                 // IEnumerable<Dog> --> IEnumerable<Animal>
    // OR
    IEnumerable<Animal> animals = cats;               // IEnumerable<Cat> --> IEnumerable<Animal>

Console.WriteLine();
Console.WriteLine("Covariant Example");
Console.WriteLine();

foreach (Animal animal in animals)
{
    Console.WriteLine(animal.Name);
}
// foreach :
//
//IEnumerator<Animal> enumerator = animals.GetEnumerator();

//while (enumerator.MoveNext())
//{
//    Animal animal = enumerator.Current;
//    Console.WriteLine(animal.Name);
//}


/**************************************
* contravariant type parameter
**************************************/

Console.WriteLine();
Console.WriteLine("Contravariant Example");
Console.WriteLine();

IAnimalProcessor<Animal> animalProcessor = new AnimalProcessor();

// Contravariance
IAnimalProcessor<Dog> dogProcessor = animalProcessor;

dogProcessor.Process(new Dog("Alpha dog"));
dogProcessor.Process(new Dog("Beta dog "));


// Contravariance
IAnimalProcessor<Cat> catProcessor = animalProcessor;

catProcessor.Process(new Cat("Alpha cat"));
catProcessor.Process(new Cat("Beta cat"));

