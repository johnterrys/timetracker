using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTimeSheet {
    public class SQLCode {
        private string sql;
        /// <summary>
        /// SQL that returns the value of the entire data string in the DB
        /// </summary>
        /// <returns>the sql select statement</returns>
        public string ViewUser() {
            try {
                sql = "SELECT * FROM dbo.People";
                return sql;
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }
    }
}