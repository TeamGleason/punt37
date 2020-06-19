using System.Net;
using System.Threading.Tasks;

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
