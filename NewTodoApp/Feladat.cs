using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTodoApp
{
    public class Feladat
    {
        public Feladat(string cim, string leiras, DateTime datum)
        {
            Cim = cim;
            Leiras = leiras;
            Datum = datum;
        }


        public string Cim { get; set; }
        public string Leiras { get; set; }
        public DateTime Datum { get; set; }


    }
}
