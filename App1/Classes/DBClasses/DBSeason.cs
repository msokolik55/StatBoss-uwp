using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBSeason
    {
        public int nID;
        public string sName;
        public DateTime dInserted;
        public DateTime dUpdated;

        public DBSeason()
        {
        }

        public DBSeason(int id, string sname, DateTime dinserted, DateTime dupdated)
        {
            this.nID = id;
            this.sName = sname;
            this.dInserted = dinserted;
            this.dUpdated = dupdated;
        }

        public void FillList(List<DBSeason> ListAllItems, string sWhere, string sOrder, bool bASC = true)
        {
            if (sWhere != "") { sWhere = " WHERE sName LIKE '%" + sWhere + "%'"; }
            if (sOrder != "")
            {
                sOrder = " ORDER BY " + sOrder;
                sOrder += bASC == true ? " ASC" : " DESC";
            }

            string sCommand = "SELECT * FROM tbl_seasons" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var item = new DBSeason
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
                    sCommand = "INSERT INTO tbl_seasons (nID, sName, dInserted) VALUES('" + nID + "', '" + sName + "', datetime('now'))";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_seasons SET sName='" + sName + "', dUpdated=datetime('now') WHERE nID = " + nID; ;
                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
