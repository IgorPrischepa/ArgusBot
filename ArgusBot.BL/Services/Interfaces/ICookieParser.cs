using System;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface ICookieParser
    {
        public object ParseString<T>(string parsedString) where T : struct;
    }
}
