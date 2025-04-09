using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace at1gsd
{
    internal class Conecte : IDisposable
    {
        private readonly SqlConnection con;
        private readonly string DataBase = "gerenciamento_de_senha"; // Nome correto do banco

        public Conecte()
        {
            string stringConnection = $@"Data Source={Environment.MachineName};Initial Catalog={DataBase};Integrated Security=True;";

            con = new SqlConnection(stringConnection);
        }

        public void OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        public void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        public SqlConnection ReturnConnection()
        {
            return con;
        }

        public void Dispose()
        {
            CloseConnection();
            con.Dispose();
        }
    }
}
