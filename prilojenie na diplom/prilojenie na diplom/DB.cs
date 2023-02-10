using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace prilojenie_na_diplom
{
 class DB
    {
        SqlConnection sqlconnection = new SqlConnection(@"Data Source=desktop-ijsu8hs;
Initial Catalog=BD prilojenie; Integrated Security=True");
        public void openConnection()
        {
            if (sqlconnection.State == System.Data.ConnectionState.Closed)
            {
                sqlconnection.Open();
            }
        }
        public void closeConnection()
        {
            if (sqlconnection.State == System.Data.ConnectionState.Open)
            {
                sqlconnection.Close();
            }
        }
        public SqlConnection getConnection()
        {
            return sqlconnection;
        }

    }
}
