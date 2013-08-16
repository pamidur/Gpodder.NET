using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using GpodderLib;

namespace GpodderLibDevTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Login().Wait();
        }

        private static async Task Login()
        {
            var storage = new FileStream("gpodder.data", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            using (var client = await PodcastLibrary.Init(storage, "DevTool","test1","test1"))
            {
                var devs = await client.GetDevices();

                await Task.Delay(int.MaxValue);
            }
        }
    }
}
