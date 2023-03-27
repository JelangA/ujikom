using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace espepe
{
    class koneksi
    {
        public MySqlConnection GetKon()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "server=localhost;username=root;password=;database=spp;";
            return conn;
        }
    }
}
