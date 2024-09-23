using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using VRageMath;

namespace SpaceLabAPI.Serializers
{
    public class Vector3Serializer : JsonConverter<Vector3>
    {
        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            Vector3 result = new Vector3();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return result;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException();

                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "X":
                        result.X = reader.GetSingle();
                        break;
                    case "Y":
                        result.Y = reader.GetSingle();
                        break;
                    case "Z":
                        result.Z = reader.GetSingle();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteNumber("Z", value.Z);
            writer.WriteEndObject();
        }
    }
}
