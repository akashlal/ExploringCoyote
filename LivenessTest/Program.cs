using System;
using System.Runtime.CompilerServices;
using Microsoft.Coyote;
using Microsoft.Coyote.IO;
using Microsoft.Coyote.Random;
using Microsoft.Coyote.Specifications;
using Microsoft.Coyote.SystematicTesting;
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

            await Task.Run(() =>
            {
                int cnt = 0;

                while (true)
                {
                    Task.ExploreContextSwitch();
                    if (cnt == 100)
                    {
                        cnt = 0;
                        Specification.Monitor<LivenessMonitor>(new UpEvent());
                    }
                    else
                    {
                        cnt++;
                    }
                }
            });
        }

    }

    class UpEvent : Event { }
    class DownEvent : Event { }

    class LivenessMonitor : Monitor
    {
        int NumReplicas = 0;

        [Start]
        [Hot]
        [OnEventDoAction(typeof(UpEvent), nameof(OnUp))]
        [OnEventDoAction(typeof(DownEvent), nameof(OnDown))]
        class NotEnough : State { }

        [Cold]
        [OnEventDoAction(typeof(UpEvent), nameof(OnUp))]
        [OnEventDoAction(typeof(DownEvent), nameof(OnDown))]
        class Enough : State { }

        void OnUp()
        {
            NumReplicas++;

            if(NumReplicas >= 3)
            {
                this.RaiseGotoStateEvent<Enough>();
            }
            else
            {
                this.RaiseGotoStateEvent<NotEnough>();
            }
        }

        void OnDown()
        {
            NumReplicas++;

            if (NumReplicas >= 3)
            {
                this.RaiseGotoStateEvent<Enough>();
            }
            else
            {
                this.RaiseGotoStateEvent<NotEnough>();
            }
        }

    }
}
