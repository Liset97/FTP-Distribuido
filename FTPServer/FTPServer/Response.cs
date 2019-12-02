using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace FTPServer
{
   public class Response
    {
        public Response()
        {
            Data = new List<object>();
        }

        public string Code { get; set; }
        public string Text { get; set; }
        public bool ShouldQuit { get; set; }
        public List<object> Data { get; set; }
        public CultureInfo Culture { get; set; }

        public ResourceManager ResourceManager { get; set; }

        public Response SetData(params object[] data)
        {
            Data.Clear();
            Data.AddRange(data);

            return this;
        }

        public Response SetCulture(CultureInfo culture)
        {
            this.Culture = culture;

            return this;
        }

        public override string ToString()
        {
            if (this.Culture == null)
            {
                this.Culture = CultureInfo.CurrentCulture;
            }

            if (ResourceManager != null)
            {
                return string.Concat(Code, " ", string.Format(ResourceManager.GetString(Text, Culture), Data.ToArray()));
            }

            if (Text != null)
                return string.Concat(Code, " ", string.Format(Text, Data.ToArray()));
            else
                return Code;
        }
    }
}
