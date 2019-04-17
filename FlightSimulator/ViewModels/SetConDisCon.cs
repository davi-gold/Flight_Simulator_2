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
                return _connect ?? (_connect = new CommandHandler(() => commandClicked()));
            }

        }
        //calling connect in model
        public void commandClicked()
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
    }
}
