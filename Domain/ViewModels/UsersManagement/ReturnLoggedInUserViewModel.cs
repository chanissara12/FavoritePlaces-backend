using System;
using System.Collections.Generic;
using System.Text;
using Domain.Database;

namespace Domain.ViewModels.UsersManagement
{
    public class ReturnLoggedInUserViewModel
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public List<string> roles { get; set; }
    }
}
