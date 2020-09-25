using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Coyote;
using Microsoft.Coyote.Actors;
using Microsoft.Coyote.IO;
using Microsoft.Coyote.SystematicTesting;

namespace InterfaceMockingActors
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            //var runtime = Microsoft.Coyote.Actors.RuntimeFactory.Create();
            //Execute(runtime);
            Test();
            //Console.ReadLine();
        }

        public static void Test()
        {
            var config = Configuration.Create().WithTestingIterations(100).WithVerbosityEnabled();
            var testingEngine = TestingEngine.Create(config, Execute);
            testingEngine.Run();
        }

        [Microsoft.Coyote.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime)
        {
            runtime.Logger = new ConsoleLogger();
            var storeId = runtime.CreateActor(typeof(StroageActor));
            var mockStorageService = new MockStorageService(storeId);

            var a = runtime.CreateActor(typeof(WorkerActor), new WorkerInitializationEvent { StorageService = mockStorageService });
            var b = runtime.CreateActor(typeof(WorkerActor), new WorkerInitializationEvent { StorageService = mockStorageService });
            runtime.SendEvent(a, new WorkEvent());
            runtime.SendEvent(b, new WorkEvent());
        }
    }

}
