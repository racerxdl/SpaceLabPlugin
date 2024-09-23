using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using VRageMath;

namespace SpaceLabAPI.Serializers
{
    public class QuaternionSerializer : JsonConverter<Quaternion>
    {
        public override Quaternion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            Quaternion quaternion = new Quaternion();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return quaternion;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException();

                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "X":
                        quaternion.X = reader.GetSingle();
                        break;
                    case "Y":
                        quaternion.Y = reader.GetSingle();
                        break;
                    case "Z":
                        quaternion.Z = reader.GetSingle();
                        break;
                    case "W":
                        quaternion.W = reader.GetSingle();
                        break;
                }
            }

            return quaternion;
        }

        public override void Write(Utf8JsonWriter writer, Quaternion value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteNumber("Z", value.Z);
            writer.WriteNumber("W", value.W);
            writer.WriteEndObject();
        }
    }
}
