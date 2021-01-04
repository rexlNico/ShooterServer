using System;
using System.Threading;

namespace ShooterServer
{
    class Program
    {
        private static Thread threadConsole;
        static void Main(string[] args)
        {
            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();
            NetworkConfig.InitNetwork();
            NetworkConfig.socket.StartListening(5555, 5, 1);
            Console.WriteLine("Network initialized!");
        }

        private static void ConsoleThread()
        {
            while (true)
            {

            }
        }

    }
}
