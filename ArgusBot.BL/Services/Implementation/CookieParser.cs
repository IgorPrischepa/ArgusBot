using ArgusBot.BL.Services.Interfaces;
using System;

namespace ArgusBot.BL.Services.Implementation
{
    public class CookieParser : ICookieParser
    {
        //public T ParseEnum<T>(string parsedString) where T: struct
        //{
        //    if (string.IsNullOrEmpty(parsedString)) throw new ArgumentNullException("Parsed string cannot be null or empty!");
        //    if(Enum.TryParse(parsedString, out T parsedValue))
        //    {
        //        return parsedValue;
        //    }
        //    throw new InvalidOperationException($"Cannot convert a string instance {parsedString} to {typeof(T)}");
        //}

        //public Guid ParseGuidId(string guidString)
        //{
        //    if (string.IsNullOrEmpty(guidString)) throw new ArgumentNullException("Parsed string cannot be null or empty!");
        //    if(Guid.TryParse(guidString,out Guid userId))
        //    {
        //        return userId;
        //    }
        //    throw new InvalidOperationException($"Cannot convert a string instance {guidString} to {typeof(Guid)}");
        //}
        public object ParseString<T>(string parsedString) where T : struct
        {
            switch (typeof(T).Name)
            {
                case "Guid":
                    {
                        if(Guid.TryParse(parsedString, out Guid parsedGuid))
                        {
                            return parsedGuid;
                        }
                        break;
                    }
            }
            throw new InvalidOperationException($"Cannot parse {parsedString} into {typeof(T).Name}");
        }

    }
}
