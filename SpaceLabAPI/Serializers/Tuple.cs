using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpaceLabAPI.Serializers
{
    public class TupleDDSerializer : JsonConverter<Tuple<double, double>>
    {
        public override Tuple<double, double> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            if (reader.TokenType != JsonTokenType.StartObject && reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            double X = 0, Y = 0;

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                X = reader.GetDouble();
                reader.Read();
                Y = reader.GetDouble();
                reader.Read();
                return new Tuple<double, double>(X, Y);
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;
                
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException();

                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "Item1":
                        X = reader.GetDouble();
                        break;
                    case "Item2":
                        Y = reader.GetDouble();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new Tuple<double, double>(X,Y);
        }

        public override void Write(Utf8JsonWriter writer, Tuple<double, double> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Item1);
            writer.WriteNumberValue(value.Item2);
            writer.WriteEndArray();
        }
    }
}
