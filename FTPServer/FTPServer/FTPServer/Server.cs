using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using FTPServer.DAL;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FTPServer
{
    public class Server : IDisposable
    {
        private static readonly object _listLock = new object();

        private ILog _log = LogManager.GetLogger(typeof(Server));

        private List<ClientConnection> _state;

        private List<TcpListener> _listeners;

        private DateTime _startTime;

        private Timer _timer;

        private bool _disposed = false;
        private bool _disposing = false;
        private bool _listening = false;
        private List<IPEndPoint> _localEndPoints;
        private string _logHeader;

        public Server(string logHeader = null)
            : this(IPAddress.Any, 21, logHeader)
        {
        }

        public Server(int port, string logHeader = null)
            : this(IPAddress.Any, port, logHeader)
        {
        }

        public Server(IPAddress ipAddress, int port, string logHeader = null)
            : this(new IPEndPoint[] { new IPEndPoint(ipAddress, port) }, logHeader)
        {
        }

        public Server(IPEndPoint[] localEndPoints, string logHeader = null)
        {
            _localEndPoints = new List<IPEndPoint>(localEndPoints);
            _logHeader = logHeader;
           
        }

        public void Start()
        {
            if (_disposed)
                throw new ObjectDisposedException("AsyncServer");

            _log.Info(_logHeader);
            _state = new List<ClientConnection>();
            _listeners = new List<TcpListener>();

            foreach (var localEndPoint in _localEndPoints)
            {
                TcpListener listener = new TcpListener(localEndPoint);

                try
                {
                    listener.Start();
                }
                catch (SocketException)
                {
                    Dispose();

                    throw new Exception("The current local end point is currently in use. Please specify another IP or port to listen on.");
                }

                listener.BeginAcceptTcpClient(HandleAcceptTcpClient, listener);

                _listeners.Add(listener);
            }

            _listening = true;

            OnStart();
        }

        public void Stop()
        {
            _log.Info("# Stopping Server");
            _listening = false;

            foreach (var listener in _listeners)
            {
                listener.Stop();
            }

            _listeners.Clear();

            OnStop();
        }

        protected void OnStart()
        {
            _startTime = DateTime.Now;

            _timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

            _timer.Start();
        }

        protected virtual void OnStop()
        {

            if (_timer != null)
                _timer.Stop();
        }

        protected virtual void OnConnectAttempt()
        {
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            OnConnectAttempt();

            TcpListener listener = result.AsyncState as TcpListener;

            if (_listening)
            {
                listener.BeginAcceptTcpClient(HandleAcceptTcpClient, listener);

                TcpClient client = listener.EndAcceptTcpClient(result);

                var connection = new ClientConnection();

                connection.Disposed += new EventHandler<EventArgs>(AsyncClientConnection_Disposed);

                connection.HandleClient(client);

                lock (_listLock)
                    _state.Add(connection);
            }
        }

        private void AsyncClientConnection_Disposed(object sender, EventArgs e)
        {
            // Prevent removing if we are disposing of this object. The list will be cleaned up in Dispose(bool).
            if (!_disposing)
            {
                ClientConnection connection = (ClientConnection)sender;

                lock (_listLock)
                    _state.Remove(connection);
            }
        }

        public void Dispose()
        {
            
            _disposing = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_timer != null)
                _timer.Dispose();
            _disposing = true;

            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();

                    lock (_listLock)
                    {
                        foreach (var connection in _state)
                        {
                            if (connection != null)
                                connection.Dispose();
                        }

                        _state = null;
                    }
                }
            }

            _disposed = true;
        }
    }
}
