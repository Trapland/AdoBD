using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace AdoBD.DAL
{
    internal class DepartmentApi
    {
        private readonly SqlConnection _connection;

        public DepartmentApi(SqlConnection connection)
        {
            _connection = connection;
        }
        public bool Add(Entity.Department department)
        {
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = $"INSERT INTO Departments (Id, Name) VALUES( @id, @name)";
                cmd.Parameters.AddWithValue("@id", department.Id);
                cmd.Parameters.AddWithValue("@name", department.Name);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                String msg = DateTime.Now.ToString() + ": " +
                    this.GetType().Name + " " +
                    System.Reflection.MethodBase.GetCurrentMethod()?.Name + " " +
                    ex.Message;
                App.Logger.Log(msg);
                return false;
            }
        }
        public bool Delete(Entity.Department department)
        {
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = $"Update Departments SET Name = 'Empty' WHERE Id = '{department.Id}'";
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                String msg = DateTime.Now.ToString() + ": " +
                    this.GetType().Name + " " +
                    System.Reflection.MethodBase.GetCurrentMethod()?.Name + " " +
                    ex.Message;
                App.Logger.Log(msg);
                return false;
            }
        }
        public bool Update(Entity.Department department)
        {
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = $"Update Departments SET Name = N'{department.Name}' WHERE Id = '{department.Id}'";
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                String msg = DateTime.Now.ToString() + ": " +
                    this.GetType().Name + " " +
                    System.Reflection.MethodBase.GetCurrentMethod()?.Name + " " +
                    ex.Message;
                App.Logger.Log(msg);
                return false;
            }
        }
        public List<Entity.Department> GetAll()
        {
            var list = new List<Entity.Department>();
            try
            {
                using SqlCommand cmd = new("SELECT * FROM Departments") { Connection = _connection };
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new(reader));
                }
            }
            catch (Exception ex)
            {
                // TODO: LOG
                String msg = DateTime.Now.ToString() + ": " +
                    this.GetType().Name + " " + 
                    System.Reflection.MethodBase.GetCurrentMethod()?.Name + " " + 
                    ex.Message;
                App.Logger.Log(msg,"Server");
                throw new Exception(msg);
            }
            return list;
        }
    }
}
