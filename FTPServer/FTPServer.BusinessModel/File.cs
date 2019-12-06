using System;
using System.Collections.Generic;
using System.Text;

namespace FTPServer.BusinessModel
{
    public class File
    {
        public Guid Key { get; set; }

        public string Name { get; set; }

        public string PathName { get; set; }

        public DateTime UpdateDate { get; set; }

        public string FileType { get; set; }

        public string Length { get; set; }

        public Directory Parent { get; set; }

        public Guid ContentKey { get; set; }

        public bool IsDeleted { get; set; }

    }
}
