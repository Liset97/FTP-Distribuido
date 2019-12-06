using System;
using System.Net;

namespace FTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            using (var s = new Server(new[] { new IPEndPoint(IPAddress.Any, 21), new IPEndPoint(IPAddress.IPv6Any, 21) }))
            {
                s.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }

            return;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           Console.WriteLine("Fatal Error: {0}", (e.ExceptionObject as Exception).Message);
        }
    }
}
