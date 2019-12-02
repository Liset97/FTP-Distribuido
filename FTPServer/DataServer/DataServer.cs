using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace DataServer
{
    public class DataServer
    {
        private ConcurrentQueue<string> _fileQueue;

        private UdpClient _listener;

        private IPEndPoint _endPoint;

        public DataServer(int port)
        {
            _fileQueue = new ConcurrentQueue<string>();
            _endPoint = new IPEndPoint(IPAddress.Any, port);
            _listener = new UdpClient(_endPoint);
        }

        public void Start()
        {
            var thread1 = new Thread(ListenRequest);
            var thread2 = new Thread(DataUpdate);
        }

        public void ListenRequest()
        {
            while (true)
            {
                var result = _listener.ReceiveAsync().Result;
                var host = result.RemoteEndPoint;
                var buffer = result.Buffer;
                var connection = new DataConnection(host, buffer, _fileQueue);
                ThreadPool.QueueUserWorkItem(connection.ProcessingRequest);
            }
        }

        public void DataUpdate()
        {
            // Aki hay q desarrollar lo de los protocolos de Kademlia y Chord
        }

    }
}
