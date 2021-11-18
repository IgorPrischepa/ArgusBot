using ArgusBot.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ArgusBot.BL.Services.Implementation
{
    public class QueryParser : IQueryParser
    {
        public SortedDictionary<string, string> ParseQueryString(IQueryCollection queryString)
        {
            var dict = new SortedDictionary<string, string>();
            if (queryString.VerifyNotNull("Query string instance cannot be null!") && queryString.Count > 0)
            {
                foreach (var param in queryString)
                {
                    if (!string.IsNullOrEmpty(param.Key) && !string.IsNullOrEmpty(param.Value))
                    {
                        dict.Add(param.Key, param.Value);
                    }
                }
            }
            else throw new InvalidOperationException("Query string is invalid!");
            return dict;
        }
    }
}
