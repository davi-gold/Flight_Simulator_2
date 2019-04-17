using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace FlightSimulator.Model
{
    class ClientCommands
    {
        TcpClient tcpClient;
        Thread clientThread;
        bool isConnected;

        private static ClientCommands m_Instance = null;
        public static ClientCommands Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ClientCommands();
                }
                return m_Instance;
            }
        }
        public ClientCommands()
        {
            isConnected = false;
        }

        public void connect()
        {
            tcpClient = new TcpClient();
            int port = ApplicationSettingsModel.Instance.FlightCommandPort;
            IPAddress ipAd = IPAddress.Parse(ApplicationSettingsModel.Instance.FlightServerIP);
            tcpClient.Connect(ipAd, port);
            Console.WriteLine("client connected");
        }
    }
}
