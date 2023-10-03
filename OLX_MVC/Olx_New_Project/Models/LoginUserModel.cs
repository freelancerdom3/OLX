using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Olx_New_Project.Models
{
    public class LoginUserModel
    {
        public int LoginUserId { get; set; }
        public string LoginOtp { get; set; }

        public DateTime ExpirationLoginTime { get; set; }
        public int userIdByLoginUser { get; set; }
    }
}