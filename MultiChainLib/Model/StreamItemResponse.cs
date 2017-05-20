using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChainLib
{
    public class StreamItemResponse
    {
        [JsonProperty("publishers")]
        public string[] Publishers { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("Confirmations")]
        public string Confirmations { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }
    }
}
