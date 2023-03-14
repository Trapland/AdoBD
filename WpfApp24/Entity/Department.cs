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
        public DateTime? DeleteDt { get; set; }
        public Department()
        {
            Id = Guid.NewGuid();
            Name = null!;
            DeleteDt= null!;
        }
        public Department(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            DeleteDt = reader.IsDBNull("DeleteDt") ? null : reader.GetDateTime("DeleteDt");
        }
        public override string ToString()
        {
            return Id.ToString()[..5] + "... " + Name ;
        }
    }
}
