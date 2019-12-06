using System;
using System.Collections.Generic;
using System.Text;

namespace FTPServer.BusinessModel
{
    public class File
    {
        public Guid Key { get; set; }

        public string Name { get; set; }

        public DateTime UpdateDate { get; set; }

        public Directory Parent { get; set; }

        public Guid ContentKey { get; set; }
    }
}
