using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTodoApp
{
    public interface IFeladatRepository
    {

        List<Feladat> FeladatBeolvasas();
        void Listahozad(Feladat feladat, User user);
        List<string> Megtekint(string fa);
    }
}
