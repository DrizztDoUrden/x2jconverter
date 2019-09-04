using Ganss.Excel;
using Newtonsoft.Json;
using System.IO;

namespace X2JCore
{
    public class Converter
    {
        public static string ConverLocations(Stream from, bool indent = false)
        {
            var locations = new ExcelMapper(from).Fetch<Location>();
            return JsonConvert.SerializeObject(locations, GetFormatting(indent));
        }

        public static string ConvertBins(Stream from, bool indent = false)
        {
            var bin = new ExcelMapper(from).Fetch<Bin>();
            return JsonConvert.SerializeObject(bin, GetFormatting(indent));
        }

        private static Formatting GetFormatting(bool indent) => indent ? Formatting.Indented : Formatting.None;
    }
}
