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

            using (var client = await GpodderClient.Init(storage, "DevTool",))
            {
                var devs = await client.DevicesService.QueryDevices();
            }
        }
    }
}
