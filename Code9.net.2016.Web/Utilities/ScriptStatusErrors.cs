using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Utilities
{
    public enum ScriptStatusErrors
    {
        OK = 0,
        MISSING_WORKER,
        NONEXISTING_ORDER,
        ORDER_ALREADY_FULFILLED,
        NO_TABLE_ORDERS
    }

    public class ScriptStatus
    {
        public ScriptStatusErrors status { get; set; }
        public string message { get; set; }

        public ScriptStatus()
        {
            status = ScriptStatusErrors.OK;
            message = "ok";
        }

        public ScriptStatus(ScriptStatusErrors status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public static ScriptStatus OK { get { return new ScriptStatus(); } }
    }
}