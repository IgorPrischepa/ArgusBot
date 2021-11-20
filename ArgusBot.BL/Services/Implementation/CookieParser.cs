using ArgusBot.BL.Services.Interfaces;
using System;

namespace ArgusBot.BL.Services.Implementation
{
    public class CookieParser : ICookieParser
    {
        public object ParseString<T>(string parsedString) where T : struct
        {
            switch (typeof(T).Name)
            {
                case "Guid":
                    {
                        if (Guid.TryParse(parsedString, out Guid parsedGuid))
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
