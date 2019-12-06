using System;
using System.Collections.Generic;
using System.Text;

namespace FTPServer
{
    public class Command
    {
        public string Code { get; set; }
        public List<string> Arguments { get; set; }
        public string Raw { get; set; }
        public string RawArguments { get; set; }
    }
}
