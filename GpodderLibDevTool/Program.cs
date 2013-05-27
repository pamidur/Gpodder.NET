using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using GpodderLib;

namespace GpodderLibDevTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var loginResult = Login().Result;
        }

        static async Task<bool> Login()
        {
            var storage = IsolatedStorageFile.GetUserStoreForAssembly();

            var client = new PodcastLibrary("DevTool",storage);
            return await client.Login();
        }
    }
}
