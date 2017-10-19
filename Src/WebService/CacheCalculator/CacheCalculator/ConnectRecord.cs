using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.DB
{
    public class ConnectRecord
    {
        /// <summary>
        /// ConnectRecord is the constructor for ConnectRecord
        /// </summary>
        public ConnectRecord(string host, short port, string db, string user, string pw)
        {
            host_ = host;
            port_ = port;
            db_ = db;
            user_ = user;
            pw_ = pw;
        }

        private string host_;
        private short port_ = 0;
        private string db_;
        private string user_;
        private string pw_;

        /// <summary>
        /// GetConnectString return the proper connection string for a specific provider based on the content
        /// of the ConnectRecord. For unknown providers it will return an empty string
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string GetConnectString(string provider)
        {
            if (provider == "SQLEXPRESS")
            {
                return
                    "Server = " + host_ + "\\SQLEXPRESS; " +
                    "Database = " + db_ + "; " +
                    "User Id = " + user_ + "; " +
                    "Password = " + pw_ + ";";
            }

            return "";
        }
    }
}
