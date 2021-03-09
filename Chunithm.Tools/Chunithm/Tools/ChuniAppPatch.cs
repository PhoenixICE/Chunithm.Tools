using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chunithm.Tools
{
    public struct ChuniAppPatch
    {
        public ChuniAppPatch(ChuniAppPatchType chuniAppPatchType, int offset, byte[] off, byte[] on)
        {
            ChuniAppPatchType = chuniAppPatchType;
            Offset = offset;
            On = on;
            Off = off;
        }

        public ChuniAppPatchType ChuniAppPatchType { get; set; }

        [JsonConverter(typeof(IntToHexConverter))]
        public int Offset { get; set; }
        [JsonConverter(typeof(ByteArrayToHexArrayConverter))]
        public byte[] On { get; set; }
        [JsonConverter(typeof(ByteArrayToHexArrayConverter))]
        public byte[] Off { get; set; }
    }

    public class IntToHexConverter : JsonConverter<int>
    {
        public override int Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                int.Parse(reader.GetString().Remove(0, 2), System.Globalization.NumberStyles.HexNumber);

        public override void Write(
            Utf8JsonWriter writer,
            int intValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue("0x" + intValue.ToString("X"));
    }

    public class ByteArrayToHexArrayConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                reader.GetString().Split(',').Select(x => byte.Parse(x.Remove(0, 2), System.Globalization.NumberStyles.HexNumber)).ToArray();

        public override void Write(
            Utf8JsonWriter writer,
            byte[] byteArrayValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(string.Join(",", byteArrayValue.Select(x => "0x" + x.ToString("X"))));
    }
}