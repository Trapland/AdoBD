using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoBD.Entity
{
    //Id, Surname, Name, Secname, Id_main_dep, Id_sec_dep, Id_chie
    public class Manager
    {
        public Guid Id { get; set; }
        public String Surname { get; set; }
        public String Name { get; set; }
        public String Secname { get; set; }
        public Guid Id_main_dep { get; set; }
        public Guid? Id_sec_dep { get; set; }
        public Guid? Id_chief { get; set; }
        public override string ToString()
        {
            return Id.ToString()[..5] + "... " + Surname + " " + Name + " " + Secname;
        }

    }
}
