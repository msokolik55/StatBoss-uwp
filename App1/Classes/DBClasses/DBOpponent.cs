using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBOpponent
    {
        public int nID;
        public int nIDSeason;
        public string sName;
        public DateTime dInserted;
        public DateTime dUpdated;

        public DBOpponent()
        {
        }

        public DBOpponent(int id, int nidseason, string sname, DateTime dinserted, DateTime dupdated)
        {
            this.nID = id;
            this.nIDSeason = nidseason;
            this.sName = sname;
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        public void FillList(List<DBOpponent> ListAllItems, string sWhere, string sOrder)
        {
            string sCommand = "SELECT * FROM tbl_opponents WHERE nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var item = new DBOpponent
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    sName = query.GetString(query.GetOrdinal("sName"))
                };

                ListAllItems.Add(item);
            }
        }

        public void ChangeDB(string action)
        {
            string sCommand = "";

            switch (action)
            {
                case "add":
                    sCommand = "INSERT INTO tbl_opponents (nID, nIDSeason, sName, dInserted) VALUES('" + nID + "', '" + StatBoss.Classes.MainVariables.NIDActualSeason + "', '" + sName + "', datetime('now'))";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_opponents SET sName='" + sName + "', dUpdated=datetime('now') WHERE nID = " + nID + " AND nIDSeason = " + StatBoss.Classes.MainVariables.NIDActualSeason;
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
