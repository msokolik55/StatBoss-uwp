using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App1.Classes.DBClasses
{
    public class DBTeam
    {
        public int nID;
        public int nIDSeason;
        public string sShortName;
        public string sName;

        public DBTeam()
        {
        }

        public DBTeam(int id, int nidseason, string sshortname, string sname)
        {
            this.nID = id;
            this.nIDSeason = nidseason;
            this.sShortName = sname;
            this.sName = sname;
        }

        public void FillList(List<DBTeam> ListAllItems, string sWhere, string sOrder, bool bASC)
        {
            if (sWhere != "") { sWhere = " AND sName LIKE '%" + sWhere + "%'"; }
            if (sOrder != "")
            {
                sOrder = " ORDER BY " + sOrder;
                sOrder += bASC == true ? " ASC" : " DESC";
            }

            string sCommand = "SELECT * FROM tbl_teams WHERE nIDSeason='" + StatBoss.Classes.MainVariables.NIDActualSeason + "'" + sWhere + sOrder;
            SqliteDataReader query = DataAccess.QueryDB(sCommand);

            while (query.Read())
            {
                var item = new DBTeam
                {
                    nID = query.GetInt32(query.GetOrdinal("nID")),
                    sShortName = query.GetString(query.GetOrdinal("sCategoryName")),
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
                    sCommand = "INSERT INTO tbl_teams (nID, nIDSeason, sCategoryName, sName)" +
                              " VALUES('" + nID + "', '" + nIDSeason + "', '" + sShortName + "', '" + sName + "')";
                    break;

                case "edit":
                    sCommand = "UPDATE tbl_teams SET sCategoryName='" + sShortName + "', " +
                                                    "sName='" + sName + "' " +
                                "WHERE nID = '" + nID + "' AND nIDSeason = '" + nIDSeason + "'";

                    break;
            }

            DataAccess.ExecDB(sCommand);
        }
    }
}
