using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public DateTime? DeleteDt { get; set; }
        public Manager()
        {
            Id = Guid.NewGuid();
            Surname = null!;
            Name = null!;
            Secname = null!;
        }
        public Manager(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Surname = reader.GetString("Surname");
            Name = reader.GetString("Name");
            Secname = reader.GetString("Secname");
            Id_main_dep = reader.GetGuid("Id_main_dep");
            Id_sec_dep = reader.GetValue("Id_sec_dep") == DBNull.Value ? null : reader.GetGuid("Id_sec_dep");
            Id_chief = reader.IsDBNull("Id_chief") ? null : reader.GetGuid("Id_chief");
            DeleteDt = reader.IsDBNull("DeleteDt") ? null : reader.GetDateTime("DeleteDt");
        }
        public override string ToString()
        {
            return Id.ToString()[..5] + "... " + Surname + " " + Name + " " + Secname + " " + Id_main_dep.ToString() + " " + Id_sec_dep.ToString() + " " + Id_chief.ToString();
        }

    }
}
