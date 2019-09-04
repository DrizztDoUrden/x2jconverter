using Ganss.Excel;
using Newtonsoft.Json;

namespace X2JCore
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Bin
    {
        [JsonProperty("data")]
        public BinData Data => new BinData
        {
            Id = Id,
            Code = Code,
            LocationId = LocationId,
            Name = Name,
            WarehouseClass = WarehouseClass,
        };

        [Column("RWS Location   (Foreign ID)"), JsonProperty("location_erp_id")]
        public string LocationId { get; set; }

        [Column("Location Bin ID")]
        public string Id { get; set; }

        [Column("Code")]
        public string Code { get; set; }

        [Column("Name SV")]
        public string Name { get; set; }

        [Column("Warehouse Class Code")]
        public string WarehouseClass { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    internal struct BinData
    {
        [JsonProperty("erp_id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("location_erp_id")]
        public string LocationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type_name")]
        public string WarehouseClass { get; set; }
    }
}
