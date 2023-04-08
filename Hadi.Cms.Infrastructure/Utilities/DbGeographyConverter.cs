using Newtonsoft.Json;
using System;
using System.Data.Spatial;
using System.Globalization;

namespace Hadi.Cms.Infrastructure.Utilities
{
    public class DbGeographyConverter : JsonConverter
    {
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            if (value is DbGeography)
            {
                DbGeography geography = (DbGeography)value;

                writer.WriteValue(String.Format(new CultureInfo("en-us"), "{0}, {1}", geography.Latitude, geography.Longitude));
                return;
            }
            throw new JsonSerializationException("Expected DbGeography object value");
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonToken.String)
            {
                try
                {
                    string stringValue = (string)reader.Value;

                    if (String.IsNullOrWhiteSpace(stringValue))
                        return null;

                    string lat = stringValue.Split(',')[0];
                    string lng = stringValue.Split(',')[1];

                    return DbGeography.FromText(String.Format("POINT({0} {1})", lng, lat));
                }
                catch (Exception ex)
                {
                    throw new JsonSerializationException(String.Format("Error parsing DbGeography string: {0}", (string)reader.Value));
                }
            }

            throw new JsonSerializationException(String.Format("Unexpected token or value when parsing version. Token: {0}, Value: {1}", reader.TokenType, (string)reader.Value));
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DbGeography);
        }
    }
}
