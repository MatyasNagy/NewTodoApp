using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTodoApp
{
     public class User
    {

        public User(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
    }
}
