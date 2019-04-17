using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

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

        public void sendStream(string line)
        {
            string[] stream = line.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            clientThread = new Thread(() => sendStream(stream));
            clientThread.Start();
        }
        public void sendStream(string[] message)
        {
            if (isConnected)
            {
                NetworkStream ns = tcpClient.GetStream();
                foreach(string str in message)
                {
                    string setting = str+"\r\n";
                    byte[] buff = Encoding.ASCII.GetBytes(setting);
                    ns.Write(buff, 0, buff.Length);
                    Thread.Sleep(2000);
                }
            }
            
        }

        public void disconnect()
        {
            isConnected = false;
            tcpClient.Close();
        }
    }
}
