using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wp10_caliburnApp.View;
using wp10_employeesApp;

namespace wp10_caliburnApp
{
    public class Bootstrapper: BootstrapperBase
    {
        public Bootstrapper()
        { 
            Initialize();   // Caliburn MVVM 초기화
        }

        protected async override void OnStartup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(sender, e);
            await DisplayRootViewForAsync<MainWindow>();
        }
    }
}
