using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace MA.dotNET.Framework.Standart.ClassLibrary.Proxy
{
    public class Server : IDisposable
    {
        #region Constructors
        public Server(string hostname, int port, int proxyServerPort)
        {
            this.ProxyServerPort = proxyServerPort;
            this.Listener = new TcpListener(IPAddress.Any, ProxyServerPort);

            this.Hostname = hostname;
            this.Port = port;
        }
        #endregion

        #region Variables
        public int ProxyServerPort { get; private set; }

        public string Hostname { get; private set; }
        public int Port { get; private set; }

        /// <summary>
        /// Listener for read the original client
        /// </summary>
        public TcpListener Listener { get; private set; }

        internal List<Client> clients = new List<Client>();
        public Client[] Clients
        {
            get
            {
                lock (this.clients)
                {
                    return this.clients.ToArray();
                }
            }
        }

        internal bool disposed = false;
        #endregion

        #region Methods
        public void Start()
        {
            this.Listener.Start();

            while (disposed == false)
            {
                Client newClient = new Client(this);
                newClient.start();

                lock (this.clients)
                {
                    this.clients.Add(newClient);
                }
            }
        }

        public void Dispose()
        {
            this.disposed = true;

            lock (clients)
            {
                foreach (var client in clients)
                {
                    client.dispose();
                }
            }

            try
            { this.Listener.Stop(); }
            catch { }
        }
        #endregion
    }
}
