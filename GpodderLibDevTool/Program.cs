using System;
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

            Console.WriteLine("enter login/password");

            using (var client = await GpodderClient.Init(storage, "DevTool", Console.ReadLine(), Console.ReadLine()))
            {
                var devs = await client.DevicesService.QueryDevices();
                var top = await client.DirectoryService.QueryTopPodcasts(10);
                //var toptags
                //    = await client.DirectoryService.QueryTopTags(10);

                //var pft = await client.DirectoryService.QueryPodcastsForTag("1Arts", 5);

                var podc = await client.DirectoryService.QueryPodcastData(new Uri("http://ypp.rpod.ru/rss.xml"));
                var epis = await client.DirectoryService.QueryEpisodeData(new Uri("http://rpod.ru/get/300774/268258/original/ypp712.mp3"), podc.Url);

            }
        }
    }
}
