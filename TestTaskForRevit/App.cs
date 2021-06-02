using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace TestTaskForRevit
{
    public class App : IExternalApplication
    {
        static AddInId m_appId = new AddInId(new Guid(
      "356CDA5A-E6C5-4c2f-A9EF-B3222116B8C9"));

        static string ExecutingAssemblyPath = System.Reflection.Assembly
      .GetExecutingAssembly().Location;


        public Result OnStartup(UIControlledApplication application)
        {
            AddMenu(application);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        private void AddMenu(UIControlledApplication app)
        {
            RibbonPanel rvtRibbonPanel = app.CreateRibbonPanel("Настройка отображения");

            PushButtonData buttonData = new PushButtonData("Layout", "Планировка", ExecutingAssemblyPath, $"TestTaskForRevit.{nameof(ApartmentLayoutVisibilityPlagin)}");

            buttonData.LargeImage = GetEmbeddedImage("TestTaskForRevit.Resources.LayoutOfApartments_32.png");

            var item = rvtRibbonPanel.AddItem(buttonData);

            PushButton optionsBtn = item as PushButton;
            optionsBtn.ItemText = "Планировка";
            optionsBtn.ToolTip = "Настройка видимости планировки квартир";

        }
        static BitmapSource GetEmbeddedImage(string name)
        {
            try
            {
                Assembly a = Assembly.GetExecutingAssembly();
                Stream s = a.GetManifestResourceStream(name);
                return BitmapFrame.Create(s);
            }
            catch
            {
                return null;
            }
        }
    }
}
