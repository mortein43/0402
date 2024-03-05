namespace _0402;

public abstract class Animal
{
    private static int totalCount = 0;
    private static readonly object totalCountLock = new object();

    public static int TotalCount
    {
        get
        {
            lock (totalCountLock)
            {
                return totalCount;
            }
        }
    }

    public abstract void MakeSound();

    public Animal()
    {
        lock (totalCountLock)
        {
            totalCount++;
        }
    }

    public virtual void Dispose()
    {
        lock (totalCountLock)
        {
            totalCount--;
        }
    }

    public delegate void PrintCountFunc();
    public static Dictionary<string, PrintCountFunc> PrintCountFuncs = new Dictionary<string, PrintCountFunc>();
    private static readonly object printCountFuncsLock = new object();

    public static void RegisterPrintCountFunc(string className, PrintCountFunc func)
    {
        lock (printCountFuncsLock)
        {
            PrintCountFuncs[className] = func;
        }
    }

    public static void PrintTotalCount()
    {
        Console.WriteLine($"Total Animals: {TotalCount}");
        lock (printCountFuncsLock)
        {
            foreach (var item in PrintCountFuncs)
            {
                item.Value();
            }
        }
    }
}

public class Dog : Animal
{
    private static int dogCount = 0;
    private static readonly object dogCountLock = new object();

    public static int DogCount
    {
        get
        {
            lock (dogCountLock)
            {
                return dogCount;
            }
        }
    }

    public Dog()
    {
        lock (dogCountLock)
        {
            dogCount++;
        }
    }

    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }

    public override void Dispose()
    {
        base.Dispose();
        lock (dogCountLock)
        {
            dogCount--;
        }
    }

    public static void PrintDogCount()
    {
        Console.WriteLine($"Total Dogs: {DogCount}");
    }

    public static void RegisterPrintDogCountFunc()
    {
        Animal.RegisterPrintCountFunc("Dog", PrintDogCount);
    }
}

public class Cat : Animal
{
    private static int catCount = 0;
    private static readonly object catCountLock = new object();

    public static int CatCount
    {
        get
        {
            lock (catCountLock)
            {
                return catCount;
            }
        }
    }

    public Cat()
    {
        lock (catCountLock)
        {
            catCount++;
        }
    }

    public override void MakeSound()
    {
        Console.WriteLine("Meow!");
    }

    public override void Dispose()
    {
        base.Dispose();
        lock (catCountLock)
        {
            catCount--;
        }
    }

    public static void PrintCatCount()
    {
        Console.WriteLine($"Total Cats: {CatCount}");
    }

    public static void RegisterPrintCatCountFunc()
    {
        Animal.RegisterPrintCountFunc("Cat", PrintCatCount);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Dog.RegisterPrintDogCountFunc();
        Cat.RegisterPrintCatCountFunc();
        Animal.PrintTotalCount();

        Thread dogThread = new Thread(() =>
        {

            for (int i = 0; i < 5; i++)
            {
                Animal dog = new Dog();
                Thread.Sleep(100);
                //dog.Dispose();
                Thread.Sleep(100);
            }
            Animal.PrintTotalCount();
        });

        Thread catThread = new Thread(() =>
        {

            for (int i = 0; i < 5; i++)
            {
                Animal cat = new Cat();
                Thread.Sleep(100);
                //cat.Dispose();
                Thread.Sleep(100);
            }
            Animal.PrintTotalCount();
        });

        dogThread.Start();
        catThread.Start();

        Thread.Sleep(1000);
        Animal.PrintTotalCount();
    }
}
