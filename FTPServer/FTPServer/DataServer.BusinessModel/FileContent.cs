using System;

namespace DataServer.BusinessModel
{
    public class FileContent
    {
        public Guid Key { get; set; }

        public string Name { get; set; }

        public DateTime UpdateDate { get; set; }

        public byte[] Content { get; set; }

    }
}
