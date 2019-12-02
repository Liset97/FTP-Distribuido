using Microsoft.Extensions.Configuration;
using System;

namespace DataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = Environment.CurrentDirectory.ToString();
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
