using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Coyote;
using Microsoft.Coyote.Actors;
using CoyoteTasks = Microsoft.Coyote.Tasks;

namespace InterfaceMockingActors
{
    public class MockStorageService : IStorageService
    {
        ActorId storeId;

        public MockStorageService(ActorId store)
        {
            this.storeId = store;
        }

        public async Task<string> Get(string key)
        {
            var eg = new AwaitableEventGroup<string>();
            this.storeId.Runtime.SendEvent(storeId, new GetEvent(key, null), eg);            
            await eg;
            return eg.Task.Result;
        }

        public Task Put(string key, string value)
        {
            this.storeId.Runtime.SendEvent(storeId, new PutEvent(key, value));
            return Task.CompletedTask;
        }
    }

}
