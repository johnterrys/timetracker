using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Services;

namespace StudentTimeSheet {
    public class DBConnect {

        private string connect;
        /// <summary>
        /// Constructor that only stores the connection string
        /// </summary>
        public DBConnect() {
            connect = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
        }
        /// <summary>
        /// The DB call that should (cross fingers) update the database value to 
        /// either alive(1) or dead(0)
        /// </summary>
        /// <param name="sql">the sql string that is retrieved from the SQLCode.cs</param>
        
        public void UpdateDB(string sql) {
            using (SqlConnection cn = new SqlConnection(connect)) {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();

                cmd.ExecuteNonQuery();

            }
        }


        /// <summary>
        /// The DB call that should (cross fingers) update the database value to 
        /// either alive(1) or dead(0)
        /// </summary>
        /// <param name="sql">the sql string that is retrieved from the SQLCode.cs</param>
        public int getCount(string sql) {
            using (SqlConnection cn = new SqlConnection(connect)) {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                rdr.Read();
                return (int)rdr[0];
            }
        }
        /// <summary>
        /// The DB call that should return the current value of the entire string
        /// held in the data column
        /// </summary>
        /// <param name="sql">the sql string that is retrieved from the SQLCode.cs</param>
        /// <returns>the query results</returns>
        public string readDB(string sql) {
            using (SqlConnection cn = new SqlConnection(connect)) {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                rdr.Read();
                return rdr[0].ToString();
            }
        }
    }

}
