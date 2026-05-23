using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hostel_Management_System.Database
{
    public static class DB
    {
        private static readonly string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=Hostel_Management_System;
              Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}