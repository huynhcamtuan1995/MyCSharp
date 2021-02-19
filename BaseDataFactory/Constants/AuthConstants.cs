using System;
using System.Collections.Generic;
using System.Text;

namespace BaseDataFactory.Constants
{
    public static class AuthConstants
    {

        public enum UserClaimType
        {
            USER_ID,
            USER_ROLE
        };

        public static string GetUserClaimType(this UserClaimType claim)
        {
            return claim switch
            {
                UserClaimType.USER_ID=> "USER:ID",
                UserClaimType.USER_ROLE=> "USER:ROLE",
                _ => string.Empty
            }; 
        }
    }
}
