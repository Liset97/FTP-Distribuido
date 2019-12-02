using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Text;

namespace DataServer
{
    public class DataConnection
    {
        private IPEndPoint _host;

        private ConcurrentQueue<string> _fileQueue;

        private byte[] _buffer;

        public DataConnection(IPEndPoint host, byte[] buffer, ConcurrentQueue<string> fileQueue) 
        {
            _host = host;
            _buffer = buffer;
            _fileQueue = fileQueue;
        }

        public void ProcessingRequest(object host)
        {
            //Procesamiento del Request

        }
    }
}
