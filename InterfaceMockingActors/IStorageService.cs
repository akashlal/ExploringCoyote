using System.Threading.Tasks;

namespace InterfaceMockingActors
{
    public interface IStorageService
    {
        Task<string> Get(string key);
        Task Put(string key, string value);
    }
}
