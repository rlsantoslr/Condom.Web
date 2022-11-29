using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Condom.Services.API.Sabesp
{
    public class SabespAPI
    {
        RestClient Client { get; set; }
        public SabespAPI()
        {
            Client = new RestClient("http://sabesp-api.herokuapp.com/");
        }

        public List<SabespData> GetInfo()
        {
            var request = new RestRequest();

            var result = Client.Execute(request, Method.Get);
            if (result.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<List<SabespData>>(result.Content);
            }
            else
            {
                return null;
            }
        }

    }


    public partial class SabespData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data")]
        public Datum[] Data { get; set; }

        public object GetInformation(string type)
        {
            object val = null;

            if (Data == null) return val;

            switch (type)
            {
                case "volume armazenado":
                    var d1 = Data.FirstOrDefault(x => x.Key.Contains("armazenado"));
                    if(d1 != null)
                    {
                        var raw = d1?.Value?.Replace("%", "")?.Replace(",", ".") ?? "";
                        float perc = 0;

                        float.TryParse(raw, style: System.Globalization.NumberStyles.Any, new CultureInfo("en-US"), out perc);

                        val = Math.Round(perc, 0);
                    }
                    break;
                case "pluviometria do dia":
                    var d = Data.FirstOrDefault(x => x.Key.Contains("pluviometria do dia"));
                    if(d != null)
                    {
                        val = d.Value;
                    }
                    break;
                case "pluviometria acumulada no mês":
                    d = Data.FirstOrDefault(x => x.Key.Contains("pluviometria acumulada no mês"));
                    if (d != null)
                    {
                        val = d.Value;
                    }
                    break;
                case "média histórica do mês":
                    d = Data.FirstOrDefault(x => x.Key.Contains("média histórica do mês"));
                    if (d != null)
                    {
                        val = d.Value;
                    }
                    break;
            }

            return val;
        }
    }

    public partial class Datum
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
