using System;
using System.Collections;
using System.Collections.Generic;

namespace FTPServer.BusinessModel
{
    public class Directory
    {
        public Guid Key { get; set; }

        public string Name { get; set; }

        public DateTime UpdateDate { get; set; }

        public ICollection<Directory> Directories { get; set; }

        public ICollection<File> Files { get; set; }

        public Directory Parent { get; set; }

    }
}
