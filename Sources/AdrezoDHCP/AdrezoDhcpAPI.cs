using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Configuration;

namespace AdrezoDHCP
{
    public partial class AdrezoDhcpAPI : ServiceBase
    {
        private HttpSelfHostServer _server;
        private HttpSelfHostConfiguration config;
        private const string EventSource = "AdrezoDhcpAPI";
        
        public AdrezoDhcpAPI()
        {
            InitializeComponent();
            // Create Event source
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, "Application");
            }
        }

        protected override void OnStart(string[] args)
        {
            // Get variables from config file
            string myhost = ConfigurationManager.AppSettings["web_host"];
            string myport = ConfigurationManager.AppSettings["web_port"];
            bool myauth = Convert.ToBoolean(ConfigurationManager.AppSettings["web_auth"]);
            bool myssl = Convert.ToBoolean(ConfigurationManager.AppSettings["web_ssl"]);
            // Create API config/routes
            if (myssl)
            {
                config = new MyHttpsSelfHostConfiguration("https://" + myhost + ":" + myport);
            } else
            {
                config = new HttpSelfHostConfiguration("http://" + myhost + ":" + myport);
            }

            config.Routes.MapHttpRoute(
                name: "API",
                routeTemplate: "{controller}/{action}/{scope}",
                defaults: new { scope = RouteParameter.Optional }
               );
            if (myauth)
            {
                config.Filters.Add(new BasicAuthenticationAttribute());
            }

            _server = new HttpSelfHostServer(config);
            // Opening service
            EventLog.WriteEntry(EventSource, "Opening Adrezo DHCP API Service on http://" + myhost + ":" + myport);
            _server.OpenAsync().Wait();
        }

        protected override void OnStop()
        {
            // Closing service
            _server.CloseAsync().Wait();
            _server.Dispose();
        }
    }
}
