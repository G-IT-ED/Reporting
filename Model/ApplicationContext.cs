using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reporting.Model
{
    public class ApplicationContext 
    {
        private string conn_param = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=5f5a3a;Database=TestDB;";
        
        public int GetData(string sqlCommand)
        {
            int result = 0;
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(conn_param))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = dr.GetInt32(0);
                        }
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить данные из БД!", ex.Message);
            }
            
            return result;
        }

        internal List<DateData> GetDateData(string sqlCommand)
        {
            List<DateData> result = new List<DateData>();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(conn_param))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result.Add(new DateData(dr.GetDateTime(0), dr.GetDateTime(1),dr.GetInt32(2))); 
                        }
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить данные из БД!", ex.Message);
            }

            return result;
        }

        internal int GetCountCulled(List<DateData> dateList)
        {
            int result = 0;
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(conn_param))
                {
                    conn.Open();
                    foreach(var data in dateList)
                    {
                        var sqlCommand = @"SELECT   
                          1
                        FROM
                          main.object_status AS o
                          WHERE o.crt_date >= '"+data.DateStart+
                          "' AND o.crt_date <= '"+data.DateFinish+
                          "' AND o.status_type_id = 2 " +
                          " AND o.object_id = " + data.IdObject;
                    NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                        NpgsqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                result+=dr.GetInt32(0);
                            }
                        }
                        dr.Close();
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить данные из БД!", ex.Message);
            }

            return result;
        }

        internal int GetCountMetal(List<DateData> dateList)
        {
            int result = 0;
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(conn_param))
                {
                    conn.Open();
                    foreach (var data in dateList)
                    {
                        var sqlCommand = @"SELECT   
                          o.status_type_id
                        FROM
                          main.object_status AS o
                          WHERE o.crt_date >= '" + data.DateStart +
                          "' AND o.crt_date <= '" + data.DateFinish +
                          "' AND o.object_id = " + data.IdObject;
                        NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                        NpgsqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            var count = 0;
                            while (dr.Read())
                            {
                                if (dr.GetInt32(0) == 1)
                                    count = 1;
                                else
                                    count = 0;
                            }
                            result += count;
                        }
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить данные из БД!", ex.Message);
            }

            return result;
        }

        internal int GetCountDeathMetal(List<DateData> dateList)
        {
            int result = 0;
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(conn_param))
                {
                    conn.Open();
                    foreach (var data in dateList)
                    {
                        var sqlCommand = @"SELECT   
                          o.status_type_id
                        FROM
                          main.object_status AS o
                          WHERE o.crt_date >= '" + data.DateStart +
                          "' AND o.crt_date <= '" + data.DateFinish +
                          "' AND o.object_id = " + data.IdObject;
                        NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                        NpgsqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            var count = 0;
                            while (dr.Read())
                            {
                                count++;
                            }
                            result += (count==0)?0:1;
                        }
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить данные из БД!", ex.Message);
            }

            return result;
        }
    }
}
