using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using VRageMath;

namespace SpaceLabAPI.Serializers
{
    public class Vector3DSerializer : JsonConverter<Vector3D>
    {
        public override Vector3D Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            Vector3D result = new Vector3D();

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
                        result.X = reader.GetDouble();
                        break;
                    case "Y":
                        result.Y = reader.GetDouble();
                        break;
                    case "Z":
                        result.Z = reader.GetDouble();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Vector3D value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteNumber("Z", value.Z);
            writer.WriteEndObject();
        }
    }
}
