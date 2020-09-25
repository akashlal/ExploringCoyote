using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestingTasks
{
    class Program
    {
        static int x = 0;

        static async Task Bar(int i)
        {
            Console.WriteLine("1");
            await Task.Delay(10);
            Console.WriteLine("2");
            await Task.Delay(10);
            Console.WriteLine("3");
            await Task.Delay(10);
            Console.WriteLine("4");
        }

        static async Task Foo(int i)
        {
            await Bar(i);
        }

        static void Main(string[] args)
        {
            var t = new Task<Task>(async () => { await Foo(0); });
            t.Start();
            t.Unwrap().Wait();            
            Console.WriteLine("Done");
        }
    }
}
