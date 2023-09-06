
namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("first  user added line at beginning");
            welcome9173();
            welcome6297();
            Console.ReadKey();
            Console.WriteLine("goodBye girls, second user added line at end");
        }

        Console.WriteLine("final stage0");

        static partial void welcome6297();
        private static void welcome9173()
        {
            Console.WriteLine();
            Console.WriteLine("enter your name: ");
            string name = Console.ReadLine();
            string family = Console.ReadLine();
            Console.WriteLine("{0} {1}, welcome to my first application", name, family);
        }
    }
}