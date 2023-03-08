using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdoBD.DAL
{
    internal class ManagerApi
    {
        private readonly SqlConnection _connection;
        public ManagerApi(SqlConnection connection)
        {
            _connection = connection;
        }
        public List<Entity.Manager> GetAll(bool includeDeleted = false)
        {
            List<Entity.Manager> list = new();
            try
            {
                string query = "SELECT * FROM Managers d";
                if (!includeDeleted) query += " WHERE d.DeleteDt IS NULL";
                using SqlCommand cmd = new(query, _connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new(reader));
                }
            }
            catch (Exception exception)
            {
                App.Logger.Log(MethodBase.GetCurrentMethod().ToString());
            }
            return list;
        }
    }
}
