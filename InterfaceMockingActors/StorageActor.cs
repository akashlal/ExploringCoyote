using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Coyote;
using Microsoft.Coyote.Actors;
using CoyoteTasks = Microsoft.Coyote.Tasks;

namespace InterfaceMockingActors
{

    public class GetEvent : Event
    {
        public string Key;
        public CoyoteTasks.TaskCompletionSource<string> ResponseTcs;

        public GetEvent(string key, CoyoteTasks.TaskCompletionSource<string> responseTcs)
        {
            this.Key = key;
            this.ResponseTcs = responseTcs;
        }
    }

    public class PutEvent : Event
    {
        public string Key;
        public string Value;
        public PutEvent(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    [OnEventDoAction(typeof(GetEvent), nameof(GetHandler))]
    [OnEventDoAction(typeof(PutEvent), nameof(PutHandler))]
    public class StroageActor : Actor
    {
        Dictionary<string, string> Store = new Dictionary<string, string>();

        private void GetHandler(Event ev)
        {
            var key = (ev as GetEvent).Key;
            var eg = this.CurrentEventGroup as AwaitableEventGroup<string>;

            if (Store.ContainsKey(key))
            {
                eg.SetResult(Store[key]);
            }
            else
            {
                eg.SetResult(null);
            }
        }

        private void PutHandler(Event ev)
        {
            var key = (ev as PutEvent).Key;
            var value = (ev as PutEvent).Value;

            Store[key] = value;
        }
    }

}
