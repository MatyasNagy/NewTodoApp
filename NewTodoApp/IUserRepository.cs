using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTodoApp
{
    public interface IUserRepository
    {
        void UserHozzaad(User user);
        List<User> UserekBeolvasas();
    }
}
