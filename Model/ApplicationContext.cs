using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Model
{
    public class ApplicationContext 
    {
        public DbSet<Status_type> StatusTypes { get; set; }

        private string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=5f5a3a;Database=TestDb;";
        
        public void GetData(string sqlCommand)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(conn_param))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while(dr.HasRows)
                {
                    var result = dr.GetName(0);
                    var result1 = dr.GetName(1);
                    while(dr.Read())
                    {
                        var result2 = dr.GetInt32(0); 
                        var result3 = dr.GetString(1); 
                    }
                    dr.NextResult();
                }
            }
        }
    }
}
