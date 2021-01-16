using System;
using System.Threading;

namespace ShooterServer
{
    class Program
    {
        private static Thread threadConsole;
        public static Database database;
        static void Main(string[] args)
        {
            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();
            CommandManager.InitCommandManager();
            database = new Database();
            NetworkConfig.InitNetwork();
            NetworkConfig.socket.StartListening(Int32.Parse(args[0]), 5, 1);
            Console.WriteLine("Network initialized!");
            //long bin = DateTime.Now.ToBinary();
        }

        private static void ConsoleThread()
        {
            while (true)
            {
                string input = Console.ReadLine();
                CommandManager.ExecuteCommand(input);
            }
        }

    }
}
