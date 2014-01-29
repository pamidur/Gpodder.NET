using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GpodderLib.Utils
{
    class JsonContent : StringContent
    {
        private readonly object _graph;

        public JsonContent(object graph):base("asa")
        {
            _graph = graph;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            throw new System.NotImplementedException();
        }
       

        //protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        //{
        //    await Task.Run(() =>
        //        {
        //            var graphSerializer = new DataContractJsonSerializer(_graph.GetType());
        //            graphSerializer.WriteObject(stream, _graph);
        //        });
        //}

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
