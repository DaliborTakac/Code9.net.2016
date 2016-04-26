using Code9.net._2016.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Utilities
{
    public static class SessionManager
    {
        const string SELECTED_TABLE_KEY = "SelectedTable";

        public static void SetActiveTable(this HttpSessionStateBase session, int tableNum)
        {
            session[SELECTED_TABLE_KEY] = tableNum;
        }

        public static int GetActiveTable(this HttpSessionStateBase session)
        {
            var table = session[SELECTED_TABLE_KEY];
            if (table == null)
            {
                return -1;
            } else
            {
                return (int)table;
            }
        }
    }
}