using Ganss.Excel;
using Newtonsoft.Json;

namespace X2JCore
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Location
    {
        [Column("RWS Location ID"), JsonProperty("erp_id")]
        public string Id { get; set; }

        [Column("Code"), JsonProperty("code")]
        public string Code { get; set; }

        [Column("Name"), JsonProperty("name")]
        public string Name { get; set; }

        [Column("LOV RWS Location Type  ")]
        public string LocationType { get; set; }
        [JsonProperty("type_name")]
        public string JsonLocationType => GetFirstWord(LocationType);

        private static string GetFirstWord(string line)
        {
            if (line == null)
                return null;

            var index = line.IndexOfAny(new[] { ' ', '\t'});
            return index == -1 ? line : line.Substring(0, index);
        }
    }
}
