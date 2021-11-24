using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface IQueryParser
    {
       Dictionary<string, string> ParseQueryString(IQueryCollection queryString);
    }
}
