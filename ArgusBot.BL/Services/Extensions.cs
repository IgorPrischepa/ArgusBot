﻿using System;

namespace ArgusBot.BL.Services
{
    public static class Extensions
    {
        public static bool VerifyNotNull(this object checkedObject, string message = "")
        {
            if (checkedObject == null)
            {
                throw new NullReferenceException(message);
            }
            return true;
        }
    }
}
