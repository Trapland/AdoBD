using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoBD.Entity
{
    public class Departments
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public override string ToString()
        {
            return Id.ToString()[..5] + "... " + Name ;
        }
    }
}
