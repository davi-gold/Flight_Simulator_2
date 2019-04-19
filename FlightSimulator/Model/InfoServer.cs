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
        TcpListener listener;
        bool isConnected;
        Thread listenThread;

        public double lon
        {
            set
            {
                lon = value;
                NotifyPropertyChanged("lon");
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
                NotifyPropertyChanged("lat");
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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ApplicationSettingsModel.Instance.FlightServerIP),
       ApplicationSettingsModel.Instance.FlightInfoPort);
            listener = new TcpListener(ep);
            listener.Start();
            // Console.WriteLine("Waiting for client connections...");
            client = listener.AcceptTcpClient();
            Console.WriteLine("Info channel: Client connected");
            
            Thread thread = new Thread(() => listenAndRead(client, listener));
            thread.Start();
        }

        public void listenAndRead(TcpClient client, TcpListener listener)
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
                    if (msg.Contains(","))
                    {
                        lon = float.Parse(msg.Split(',')[0]);
                        lat = float.Parse(msg.Split(',')[1]);
                    }
                }
            }
            ns.Close();
            client.Close();
            listener.Stop();
        }

        public void disconnect()
        {
            //will stop while loop
            isConnected = false;
            client.Close();
            listener.Stop();
        }
    }
}