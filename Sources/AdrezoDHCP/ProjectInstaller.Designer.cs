namespace AdrezoDHCP
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AdrezoServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AdrezoServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AdrezoServiceProcessInstaller
            // 
            this.AdrezoServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.AdrezoServiceProcessInstaller.Password = null;
            this.AdrezoServiceProcessInstaller.Username = null;
            this.AdrezoServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.AdrezoServiceProcessInstaller_AfterInstall);
            // 
            // AdrezoServiceInstaller
            // 
            this.AdrezoServiceInstaller.Description = "Adrezo MS DHCP API Server";
            this.AdrezoServiceInstaller.DisplayName = "Adrezo DHCP API";
            this.AdrezoServiceInstaller.ServiceName = "AdrezoDhcpAPI";
            this.AdrezoServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.AdrezoServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.AdrezoServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AdrezoServiceProcessInstaller,
            this.AdrezoServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AdrezoServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AdrezoServiceInstaller;
    }
}