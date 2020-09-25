using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceMockingActors
{
    public class SimplerMockStorageService : IStorageService
    {
        Dictionary<string, string> Store;

        public SimplerMockStorageService()
        {
            this.Store = new Dictionary<string, string>();
        }

        public Task<string> Get(string key)
        {
            Microsoft.Coyote.Tasks.Task.ExploreContextSwitch();
            if (Store.ContainsKey(key))
            {
                return Task.FromResult(Store[key]);
            }
            else
            {
                return Task.FromResult<string>(null);
            }
        }

        public Task Put(string key, string value)
        {
            Microsoft.Coyote.Tasks.Task.ExploreContextSwitch();
            Store[key] = value;
            return Task.CompletedTask;
        }
    }

}
