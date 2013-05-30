using System.IO;
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
            var storage = new FileStream("gpodder.data", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            var client = new PodcastLibrary("DevTool",storage);

            await client.Init();

            return await client.Login();
        }
    }
}
