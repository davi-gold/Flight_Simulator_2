using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using System.Windows.Input;
using System.Threading;

namespace FlightSimulator.ViewModels
{
    class SetConDisCon
    {
        private ICommand _connect;
        public ICommand connect
        {
            get
            {
                return _connect ?? (_connect = new CommandHandler(() => connectClicked()));
            }

        }
        //calling connect in model
        public void connectClicked()
        {
            //if is already connected, need to close 
            //the previous connection and create a new connections

            //else
            new Thread(() =>
            {
                //creating server side connection
                InfoServer.Instance.connect();
                //need to add connect for client side
            }).Start();
        }

        private ICommand _disconnect;
        public ICommand disconnect
        {
            get
            {
                return _disconnect ?? (_disconnect = new CommandHandler(() => disconnectClicked()));
            }
        }

        public void disconnectClicked()
        {
            new Thread(() =>
            {
                //closing server side connection
                InfoServer.Instance.disconnect();
                //need to add disconnect for client side
            }).Start();
        }      
    }
}
