using System;
using Microsoft.Coyote.Specifications;
using Microsoft.Coyote.Tasks;

namespace LivenessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        [Microsoft.Coyote.SystematicTesting.Test]
        public static async Task Test()
        {
            Specification.RegisterMonitor<LivenessMonitor>();

            var lck = AsyncLock.Create();
            int x = 0;

            var t1 = Task.Run(async () =>
            {
                while (true)
                {
                    using (await lck.AcquireAsync())
                    {
                        x++;
                    }
                }
            });

            var t2 = Task.Run(async () =>
            {
                while (true)
                {
                    using (await lck.AcquireAsync())
                    {
                        x++;
                    }
                }
            });

            await Task.WhenAll(t1, t2);
        }
    }

    class LivenessMonitor : Monitor
    {
        [Start]
        [Hot]
        class S : State { }
    }
}
