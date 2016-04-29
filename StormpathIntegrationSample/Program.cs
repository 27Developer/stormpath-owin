using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormpathIntegrationSample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var hostAddress = args.Length > 0 ? args[0] : "http://localhost:8080";

            using (WebApp.Start<Startup>(url: hostAddress))
            {
                Console.WriteLine("Server started at {0}... press ENTER to shut down", hostAddress);
                Console.ReadLine();
            }
        }
    }
}
