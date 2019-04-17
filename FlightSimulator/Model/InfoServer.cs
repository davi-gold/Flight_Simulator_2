using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.ViewModels;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace FlightSimulator.Model
{
    //class which connects connection for updating information
    class InfoServer : BaseNotify
    {
        //the client we're listening to
        TcpClient client;
        bool isConnected;
        Thread listenThread;

        public double lon
        {
            set
            {
                lon = value;
            }
            get
            {
                return lon;
            }
        }

        public double lat
        {
            set
            {
                lat = value;
            }
            get
            {
                return lat;
            }
        }

        private static InfoServer m_Instance = null;
        public static InfoServer Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new InfoServer();
                }
                return m_Instance;
            }
        }
        public InfoServer()
        {
            isConnected = false;
        }

        //server side connection
        public void connect()
        {
            //no need for try and catch because of assigment instructions
            TcpListener server = null;
            int port = ApplicationSettingsModel.Instance.FlightCommandPort;
            IPAddress ipAd = IPAddress.Parse(ApplicationSettingsModel.Instance.FlightServerIP);
            //creating server
            server = new TcpListener(ipAd, port);
            // Thread listenThread = new Thread(server.Start);
            server.Start();
            isConnected = true;
            Console.WriteLine("server connected");
            //tcp client for this server
            client = server.AcceptTcpClient();
            listenThread = new Thread(() =>
            {
                //NOT SURE IF I NEED THIS
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                listenAndRead();
            });
            listenThread.Start();
            //need to check when to close thread
        }

        public void listenAndRead()
        {
            Byte[] bytes;
            NetworkStream ns = client.GetStream();
            //reading from client
            while (isConnected)
            {
                if (client.ReceiveBufferSize > 0)
                {
                    bytes = new byte[client.ReceiveBufferSize];
                    ns.Read(bytes, 0, client.ReceiveBufferSize);
                    //msg = longitude-deg, latitude-deg 
                    string msg = Encoding.ASCII.GetString(bytes);
                    lon = float.Parse(msg.Split('-')[0]);
                    lon = float.Parse(msg.Split('-')[1]);
                }
            }
            ns.Close();
            client.Close();
        }

        public void disconnect()
        {
            //will stop while loop
            isConnected = false;
            client.Close();
        }
    }
}