using System;

namespace ArgusBot.BL.Services
{
    public static class Extensions
    {
        public static bool VerifyNotNull(this object checkedObject, string message = "", bool throwException = true)
        {
            if (checkedObject == null)
            {
                if (throwException)
                    throw new NullReferenceException(message);
                return false;
            }
            return true;

        }
    }
}
