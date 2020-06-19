using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Makaretu.Dns;

namespace Punt37
{
    class ListenForPunt
    {
        // Needs netsh http add urlacl url="http://+:63737/" user=everyone
        public static void Listen()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:63737/");
            listener.Start();

            var sd = new ServiceDiscovery();

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up) continue;
                if (ni.GetIPProperties().GatewayAddresses.Count == 0) continue;

                foreach(var ua in ni.GetIPProperties().UnicastAddresses)
                {
                    var service = new ServiceProfile(Environment.MachineName, "_punt37._tcp", 63737, new List<IPAddress> { ua.Address });
                    sd.Advertise(service);
                }
            }

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var context = listener.GetContext();
                    Task.Factory.StartNew(() => ProcessRequest(context));
                }
            });
        }

        private static void ProcessRequest(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "PUNT37":

                    if (Reboot.ForceReboot())
                    {
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "Rebooting";
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        context.Response.StatusDescription = "Failure Rebooting";
                    }
                    context.Response.Close();
                    break;

                case "GET":

                    context.Response.StatusDescription = "Never Punt";
                    context.Response.StatusCode = 200;
                    context.Response.Close();
                    break;

                default:

                    context.Response.StatusCode = 404;
                    context.Response.Close();
                    break;
            }
        }
    }
}
