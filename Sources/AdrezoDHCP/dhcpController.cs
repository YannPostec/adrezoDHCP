using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dhcp;
using Newtonsoft.Json;
using System.Configuration;


namespace AdrezoDHCP
{
    public class Scope
    {
        public String ip;
        public Scope(String s) { this.ip = s; }
    }

    public class ScopeList
    {
        public List<Scope> scopes = new List<Scope>();
    }

    public class ScopeRange
    {
        public String start;
        public String end;
        public ScopeRange(String s, String e)
        {
            this.start = s;
            this.end = e;
        }
    }

    public class ScopeExcludeRangeList
    {
        public List<ScopeRange> excluderanges = new List<ScopeRange>();
    }

    public class Lease
    {
        public String ip;
        public String mac;
        public String stamp;
        public String name;
        public Lease(String i, String m, String s, String n)
        {
            this.ip = i;
            this.mac = m;
            this.stamp = s;
            this.name = n;
        }
    }

    public class ScopeLeaseList
    {
        public List<Lease> leases = new List<Lease>();
    }

    public class dhcpController : ApiController
    {
        private string dhcp_host = ConfigurationManager.AppSettings["dhcp_host"];

        [HttpGet]
        public String scopelist()
        {
            // Connect to DHCP Server
            var dhcpServer = DhcpServer.Connect(dhcp_host);
            ScopeList mylist = new ScopeList();
            // Retrieving scopes list and add each one to response
            foreach (var scope in dhcpServer.Scopes)
            {
                mylist.scopes.Add(new Scope(scope.Address.ToString()));
            }
            // Returning Json string from mylist omiting null values
            return JsonConvert.SerializeObject(mylist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpGet]
        public String range(String scope)
        {
            // Connect to DHCP Server
            var dhcpServer = DhcpServer.Connect(dhcp_host);
            Boolean bFound = false;
            String mys = "";
            String mye = "";
            // Browsing through scopes
            foreach (var myscope in dhcpServer.Scopes)
            {
                if (myscope.Address.ToString().Equals(scope))
                {
                    // Found the scope given in parameter
                    bFound = true;
                    // Store start and end of this scope range
                    mys = myscope.IpRange.StartAddress.ToString();
                    mye = myscope.IpRange.EndAddress.ToString();
                }
            }
            if (bFound)
            {
                // Creating range from previous start and end
                ScopeRange myrange = new ScopeRange(mys,mye);
                // Returning Json string from myrange omiting null values
                return JsonConvert.SerializeObject(myrange, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            } else
            {
                // Scope given in parameter was not found
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound,"Scope not found"));
            }
        }

        [HttpGet]
        public String exclude(String scope)
        {
            // Connect to DHCP Server
            var dhcpServer = DhcpServer.Connect(dhcp_host);
            Boolean bFound = false;
            ScopeExcludeRangeList mylist = new ScopeExcludeRangeList();
            // Browsing through scopes
            foreach (var searchscope in dhcpServer.Scopes)
            {
                if (searchscope.Address.ToString().Equals(scope))
                {
                    // Found the scope given in parameter
                    bFound = true;
                    // Browsing through exclude ranges for this scope
                    foreach (var myex in searchscope.ExcludedIpRanges)
                    {
                        // Adding start and end values from this exclude range
                        mylist.excluderanges.Add(new ScopeRange(myex.StartAddress.ToString(), myex.EndAddress.ToString()));
                    }
                }
            }

            if (bFound)
            {
                // Returning Json string from mylist omiting null values
                return JsonConvert.SerializeObject(mylist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            } else
            {
                // Scope given in parameter was not found
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Scope not found"));
            }
        }

        [HttpGet]
        public String reserve(String scope)
        {
            // Connect to DHCP Server
            var dhcpServer = DhcpServer.Connect(dhcp_host);
            Boolean bFound = false;
            ScopeLeaseList mylist = new ScopeLeaseList();
            // Browsing through scopes
            foreach (var searchscope in dhcpServer.Scopes)
            {
                if (searchscope.Address.ToString().Equals(scope))
                {
                    // Found the scope given in parameter
                    bFound = true;
                    // Browsing through reservations for this scope
                    foreach (var myresa in searchscope.Reservations)
                    {
                        // Retrieving client name and keeping only the host part
                        String myname = myresa.Client.Name;
                        int firstpoint = myname.IndexOf('.');
                        if (firstpoint > 0)
                        {
                            myname = myname.Substring(0, firstpoint);
                        }
                        // Limit client name to 20 chars
                        if (myname.Length > 20) 
                        {
                            myname = myname.Substring(0, 20);
                        }
                        // Reserve Mac Address doesn't have : to be removed
                        // Add this reservation with no expiration date (null), just ip/mac/name
                        mylist.leases.Add(new Lease(myresa.IpAddress.ToString(), myresa.HardwareAddress.ToString(), null, myname));
                    }
                }
            }

            if (bFound)
            {
                // Returning Json string from mylist omiting null values
                return JsonConvert.SerializeObject(mylist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            else
            {
                // Scope given in parameter was not found
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Scope not found"));
            }
        }

        [HttpGet]
        public String lease(String scope)
        {
            // Connect to DHCP Server
            var dhcpServer = DhcpServer.Connect(dhcp_host);
            Boolean bFound = false;
            ScopeLeaseList mylist = new ScopeLeaseList();
            // Browsing through scopes
            foreach (var searchscope in dhcpServer.Scopes)
            {
                if (searchscope.Address.ToString().Equals(scope))
                {
                    // Found the scope given in parameter
                    bFound = true;
                    // Get active DHCP client leases
                    var activeClients = searchscope.Clients
                        .Where(c => c.AddressState == DhcpServerClientAddressStates.Active)
                        .Where(c => c.Type == DhcpServerClientTypes.DHCP);

                    // Browsing through leases
                    foreach (var client in activeClients)
                    {
                        // Retrieving client name and keeping only the host part
                        String myname = client.Name;
                        int firstpoint = myname.IndexOf('.');
                        if (firstpoint > 0)
                        {
                            myname = myname.Substring(0, firstpoint);
                        }
                        // Limit client name to 20 chars
                        if (myname.Length > 20)
                        {
                            myname = myname.Substring(0, 20);
                        }
                        // Removing : from mac address string
                        String myhw = client.HardwareAddress.ToString();
                        myhw = myhw.Replace(":", "");
                        // Add this lease with all attributes ip/mac/expiration date/name
                        mylist.leases.Add(new Lease(client.IpAddress.ToString(), client.HardwareAddress.ToString(), client.LeaseExpires.ToString("yyyy-MM-dd HH:mm:ss"), myname));
                    }
                }
            }

            if (bFound)
            {
                // Returning Json string from mylist omiting null values
                return JsonConvert.SerializeObject(mylist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            else
            {
                // Scope given in parameter was not found
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Scope not found"));
            }
        }
    }
}
