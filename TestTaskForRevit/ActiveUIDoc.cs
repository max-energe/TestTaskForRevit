using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForRevit
{
    public class ActiveUIDoc
    {
        private static UIApplication uiApp;
        public static Autodesk.Revit.UI.UIApplication UIApp
        {
            set { uiApp = value; }
        }
        public static Autodesk.Revit.DB.Document Doc
        {
            get { return uiApp.ActiveUIDocument.Document; }
        }
    }
}

