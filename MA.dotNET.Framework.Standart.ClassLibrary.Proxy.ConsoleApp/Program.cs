using System;

namespace MA.dotNET.Framework.Standart.ClassLibrary.Proxy.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Type host name = ");
            string hostName = Console.ReadLine();

            Console.Write("Type host port = ");
            int port = Convert.ToInt32(Console.ReadLine());

            Console.Write("Type proxy port = ");
            int proxyPort = Convert.ToInt32(Console.ReadLine());

            Server server = new Server(hostName, port, proxyPort);
            server.Start();
        }
    }
}
