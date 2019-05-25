using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBPosition
    {
        public int nID;
        public string sName;
        public DateTime dInserted;
        public DateTime dUpdated;

        public DBPosition()
        {
        }

        public DBPosition(int id, string sname, DateTime dinserted, DateTime dupdated)
        {
            this.nID = id;
            this.sName = sname;
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        public void FillList(List<DBPosition> ListAllItems, string sWhere, string sOrder)
        {
            if (sWhere != "") { sWhere = " WHERE sName LIKE '%" + sWhere + "%'"; }

            string sCommand = "SELECT * FROM tbl_positions" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var iposition = new DBPosition
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    sName = query.GetString(query.GetOrdinal("sName"))
                };

                ListAllItems.Add(iposition);
            }
        }

        public void ChangeDB(string action)
        {
            string sCommand = "";

            switch (action)
            {
                case "add":
                    sCommand = "INSERT INTO tbl_positions (nID, sName, dInserted) VALUES('" + nID + "', '" + sName + "', datetime('now'))";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_positions SET sName='" + sName + "', dUpdated=datetime('now') WHERE nID = " + nID;
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
