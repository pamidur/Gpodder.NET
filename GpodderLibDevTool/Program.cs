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
            var client = new GpodderClient("DevTool");
            return await client.LoginAsync();
        }
    }
}
