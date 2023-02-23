using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoBD.Entity
{
    public class Sale   // ORM for table Sales
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime SaleDt { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Sale()
        {
            Id = Guid.NewGuid();
            Quantity = 1;
            SaleDt = DateTime.Now;
        }
        public Sale(SqlDataReader reader)
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id"));
            Quantity = reader.GetInt32("Quantity");
            ProductId = reader.GetGuid("ProductId");
            ManagerId = reader.GetGuid("ManagerId");
            SaleDt = reader.GetDateTime("SaleDt");
            DeleteDt = reader.GetValue("DeleteDt") == DBNull.Value
                         ? null
                         : reader.GetDateTime("DeleteDt");
            /* [Id]       
             * [SaleDt]   
             * [ProductId]
             * [Quantity] 
             * [ManagerId]
             * [DeleteDt] 
             */
        }
    }
}
