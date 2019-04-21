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
        //TcpListener listener;
        bool isConnected;
        Thread listenThread;

        public double lon;
        public double Lon
        {
            set
            {
                if(lon!= value)
                {
                    lon = value;
                    NotifyPropertyChanged("Lon");
                }
            }
            get
            {
                return lon;
            }
        }

        public double lat;
        public double Lat
        {
            set
            {
                if (lat != value)
                {
                    Console.Write(lat);
                    lat = value;
                    NotifyPropertyChanged("Lat");
                }
            }
            get
            {
                return lat;
            }
        }

        public TcpListener listener
        {
            set;
            get;
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
            /*IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ApplicationSettingsModel.Instance.FlightServerIP),
       ApplicationSettingsModel.Instance.FlightInfoPort);
            listener = new TcpListener(ep);*/
            Int32 port = ApplicationSettingsModel.Instance.FlightInfoPort;
            IPAddress ip = IPAddress.Parse(ApplicationSettingsModel.Instance.FlightServerIP);
            IPEndPoint ep = new IPEndPoint(ip, port);
            listener = new TcpListener(ep);
            listener.Start();
            // Console.WriteLine("Waiting for client connections...");
            client = listener.AcceptTcpClient();
            Console.WriteLine("Info channel: Client connected");
            isConnected = true;
            Thread thread = new Thread(() => listenAndRead());
            thread.Start();
        }

        //this should be simluator sending me the info but i don't think it's working
        public void listenAndRead()
        {
             /*Byte[] bytes;
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
            listener.Stop();*/
             
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;
            NetworkStream stream = client.GetStream();
            int i;
            while (isConnected)
            {
                if (client.ReceiveBufferSize > 0)
                {
                    bytes = new byte[client.ReceiveBufferSize];
                    stream.Read(bytes, 0, client.ReceiveBufferSize);
                    string msg = Encoding.ASCII.GetString(bytes); //the message incoming
                    if (msg!=null && msg.Contains(","))
                    {
                        Lon = float.Parse(msg.Split(',')[0]);
                        Lat = float.Parse(msg.Split(',')[1]);
                    }
                }
                /*while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    data = data.ToUpper();
                    if (data.Contains(","))
                    {
                        Lon = float.Parse(data.Split(',')[0]);
                        Lat = float.Parse(data.Split(',')[1]);
                    }
                }*/

            }
            stream.Close();
            client.Close();
           // listener.Stop();
        }

        public void disconnect()
        {
            //will stop while loop
            isConnected = false;
            //client.Close();
            listener.Stop();
        }
    }
}