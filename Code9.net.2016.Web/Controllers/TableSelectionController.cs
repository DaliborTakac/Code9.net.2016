using Code9.net._2016.Web.Models;
using Code9.net._2016.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Code9.net._2016.Web.Controllers
{
    public class TableSelectionController : Controller
    {
        const int numtables = 12;

        private IEnumerable<Table> GenerateTables()
        {
            var ret = new List<Table>();
            for (int i = 1; i <= numtables; ++i)
            {
                ret.Add(new Table()
                {
                    DisplayName = "Table #" + i,
                    Number = i
                });
            }
            return ret;
        }

        // GET: TableSelection
        public ActionResult Index()
        {
            var model = new TableSelectionModel();
            model.Tables = GenerateTables();
            model.Selected = model.Tables
                .Where(t => t.Number == Session.GetActiveTable())
                .FirstOrDefault();
            return PartialView(model);
        }

        // GET: TableSelection/SetTable/{ID}
        public ActionResult SetTable(int ID)
        {
            Session.SetActiveTable(ID);
            return RedirectToAction("Index", "Waiters");
        }
    }
}