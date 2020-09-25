using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Coyote;
using Microsoft.Coyote.Actors;
using CoyoteTasks = Microsoft.Coyote.Tasks;

namespace InterfaceMockingActors
{
    [OnEventDoAction(typeof(WorkEvent), nameof(DoWork))]
    public class WorkerActor : Actor
    {
        IStorageService StorageService;

        protected override Task OnInitializeAsync(Event initialEvent)
        {
            this.StorageService = (initialEvent as WorkerInitializationEvent).StorageService;
            return base.OnInitializeAsync(initialEvent);
        }

        private async Task DoWork(Event e)
        {
            await StorageService.Put("owner", this.Id.Name);
            var ret = await StorageService.Get("owner");
            Console.WriteLine("Am I ({0}) still the owner: {1}", this.Id.Name, ret == this.Id.Name ? "Yes" : "No");
        }
    }

    class WorkerInitializationEvent : Event
    {
        public IStorageService StorageService;
    }

    class WorkEvent : Event { }

}
