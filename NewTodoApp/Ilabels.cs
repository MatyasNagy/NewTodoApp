using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NewTodoApp
{


    public interface Ilabels
    {
        List<string> Lista();
        void Listahozad(string srt);
        void Listabolelvesz(string srt);
        void LabelFaRendez(List<string> labelekString, List<string> feladatokString);
    }
}
