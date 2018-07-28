using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MA.dotNET.Framework.Standart.ClassLibrary.Proxy
{
    public class Client
    {
        #region Constructors
        internal Client(Server server)
        {
            this.Server = server;

            this.ProxyClient = new TcpClient();
        }
        #endregion

        #region Variables
        public Server Server { get; private set; }

        /// <summary>
        /// Original Server from listener
        /// </summary>
        public TcpClient OriginalClient { get; private set; }
        public NetworkStream OriginalClientNetworkStream { get; private set; }

        /// <summary>
        /// Client for connect to wanted connection
        /// </summary>
        public TcpClient ProxyClient { get; private set; }
        public NetworkStream ProxyClientNetworkStream { get; private set; }

        private bool _Disposed = false;

        public Thread OriginalClientThread { get; private set; }
        public Thread ProxyClientThread { get; private set; }

        private bool _CheckConnectionAndDisposedForOriginalClient
        {
            get
            {
                if (this._Disposed == true)
                    return false;

                lock (this.OriginalClient)
                {
                    if (this.OriginalClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (this.OriginalClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        private bool _CheckConnectionAndDisposedForProxyClient
        {
            get
            {
                if (this._Disposed == true)
                    return false;

                lock (this.ProxyClient)
                {
                    if (this.ProxyClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (this.ProxyClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
        #endregion

        #region Events
        public delegate void ClosedConnection(Client sender);
        public event ClosedConnection OnClosedConnection;
        #endregion

        #region Methods
        internal void start()
        {
            this.OriginalClient = this.Server.Listener.AcceptTcpClient();
            this.OriginalClientNetworkStream = this.OriginalClient.GetStream();

            this.ProxyClient.Connect(this.Server.Hostname, this.Server.Port);
            this.ProxyClientNetworkStream = this.ProxyClient.GetStream();

            #region ListenerThread
            this.OriginalClientThread = new Thread(() =>
            {
                try
                {
                    while (this._CheckConnectionAndDisposedForOriginalClient == true)
                    {
                        if (this.OriginalClientNetworkStream.DataAvailable == true)
                        {
                            lock (this.OriginalClientNetworkStream)
                            {
                                lock (this.ProxyClientNetworkStream)
                                {
                                    byte[] buffer = new byte[524288]; // read in chunks of 512KB
                                    int bytesRead;
                                    while (this._CheckConnectionAndDisposedForOriginalClient && this.OriginalClientNetworkStream.DataAvailable && (bytesRead = this.OriginalClientNetworkStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        this.ProxyClientNetworkStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                this.dispose();
            });
            this.OriginalClientThread.Start();
            #endregion

            #region ProxyClientThread
            this.ProxyClientThread = new Thread(() =>
            {
                try
                {
                    while (this._CheckConnectionAndDisposedForProxyClient == true)
                    {
                        if (this.ProxyClientNetworkStream.DataAvailable == true)
                        {
                            lock (this.ProxyClientNetworkStream)
                            {
                                lock (this.OriginalClientNetworkStream)
                                {
                                    byte[] buffer = new byte[524288]; // read in chunks of 512KB
                                    int bytesRead;
                                    while (this._CheckConnectionAndDisposedForProxyClient && this.ProxyClientNetworkStream.DataAvailable && (bytesRead = this.ProxyClientNetworkStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        this.OriginalClientNetworkStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                this.dispose();
            });
            this.ProxyClientThread.Start();
            #endregion
        }

        internal void dispose()
        {
            this._Disposed = true;
            lock (this.Server.clients)
            {
                if (this.Server.clients.Count > 0)
                    this.Server.clients.Remove(this);
            }

            if (OnClosedConnection != null)
                OnClosedConnection(this);

            try { this.OriginalClient.Close(); }
            catch { }
            try { this.ProxyClient.Close(); }
            catch { }

            try { this.OriginalClientThread.Abort(); }
            catch { }
            try { this.ProxyClientThread.Abort(); }
            catch { }
        }
        #endregion
    }
}
