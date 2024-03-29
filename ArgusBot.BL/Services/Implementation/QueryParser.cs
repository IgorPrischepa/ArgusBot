﻿using ArgusBot.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ArgusBot.BL.Services.Implementation
{
    public class QueryParser : IQueryParser
    {
        public Dictionary<string, string> ParseQueryString(IQueryCollection queryString)
        {
            var dict = new Dictionary<string, string>();
            queryString.VerifyNotNull("Query string cannot be null!");
            if (queryString.Count > 0)
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
