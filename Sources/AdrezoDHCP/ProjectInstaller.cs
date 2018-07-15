using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Security.Cryptography.X509Certificates;

namespace AdrezoDHCP
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void AdrezoServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            var serviceInstaller = sender as ServiceInstaller;
            // Start the service after it is installed.
            if (serviceInstaller != null && serviceInstaller.StartType == ServiceStartMode.Automatic)
            {
                var serviceController = new ServiceController(serviceInstaller.ServiceName);
                serviceController.Start();
            }
        }

        private void AdrezoServiceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
