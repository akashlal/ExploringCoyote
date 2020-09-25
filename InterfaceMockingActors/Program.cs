using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Coyote;
using Microsoft.Coyote.Actors;
using CoyoteTasks = Microsoft.Coyote.Tasks;

namespace InterfaceMockingActors
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            var runtime = Microsoft.Coyote.Actors.RuntimeFactory.Create();
            Execute(runtime);
            Console.ReadLine();
        }

        [Microsoft.Coyote.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime)
        {
            var storeId = runtime.CreateActor(typeof(StroageActor));
            var mockStorageService = new MockStorageService(storeId);

            var a = runtime.CreateActor(typeof(WorkerActor), new WorkerInitializationEvent { StorageService = mockStorageService });
            var b = runtime.CreateActor(typeof(WorkerActor), new WorkerInitializationEvent { StorageService = mockStorageService });
            runtime.SendEvent(a, new WorkEvent());
            runtime.SendEvent(b, new WorkEvent());
        }
    }

}
