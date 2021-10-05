using System.Data;
using System.Data.SqlClient;

namespace XMLReading_WPF
{
    class LoadObj_SQL : LoadObj
    {
        private SqlConnection connection { get; set; }
        public LoadObj_SQL(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

            if (connection.State == ConnectionState.Open)
                IsReady = true;
            else
                IsReady = false;
        }
    }
}
