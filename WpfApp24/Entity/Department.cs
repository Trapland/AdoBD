using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoBD.Entity
{
    public class Department
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Department()
        {
            Name = null!;
        }
        public Department(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
        }
        public override string ToString()
        {
            return Id.ToString()[..5] + "... " + Name ;
        }
    }
}
