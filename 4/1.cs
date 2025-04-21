using System;

class Animal
{
    public string Name;
    public int Age;

    public Animal(string name , int age)
    {
        Name = name;
        Age = age;
    }

    public virtual void MakeSound()
    {
        Console.WriteLine("sound sound");
    }
}

class Dog : Animal
{
    public Dog(string name , int age) : base(name ,age)
    {}

    public override void MakeSound()
    {
        Console.WriteLine("hup hup");
    }
}

class Cat : Animal
{
    public Cat(string name , int age):base(name,age) 
    {}

    public override void MakeSound()
    {
        Console.WriteLine("mio mio");
    }

    public virtual void Groom()
    {
        Console.WriteLine("clean cat");
    }
}

class PersianCat : Cat
{
    public PersianCat(string name , int age) : base(name,age)
    {}

    public override void MakeSound()
    {
        Console.WriteLine("pmio pmio");
    }

    public override void Groom()
    {
        Console.WriteLine("p clean cat");
    }
}

class Program
{
    public static void Main(string[] args)
    {
        Animal[] animals = new Animal[]
        {
            new Animal("animal" , 20),
            new Dog("dog" , 22),
            new Cat("cat" , 24),
            new PersianCat("persian cat", 26)
        };

        foreach (var a in animals)
        {
            a.MakeSound();
            if (a is Cat cat)
            {
                cat.Groom();
            }
        }
    }
}
