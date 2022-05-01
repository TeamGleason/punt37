using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punt37
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool useHttps = parseArgs(args);
            Application.Run(new Main(useHttps));
        }

        static bool parseArgs(string[] args)
        {
            bool useHttps = false;
            foreach (string arg in args)
            {
                if (arg == "--https")
                {
                    useHttps = true;
                }
            }
            return useHttps;
        }
    }
}
