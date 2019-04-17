using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class JoystickSlidersViewModel
    {
        public float throttle
        {
            set
            {
                ClientCommands.Instance.sendStream("set controls/flight/throttle " + value);
            }
        }

        public float rudder
        {
            set
            {
                ClientCommands.Instance.sendStream("set controls/flight/rudder " + value);
            }
        }

        public float aileron
        {
            set
            {
                ClientCommands.Instance.sendStream("set controls/flight/aileron " + value);
            }
        }

        public float elevator
        {
            set
            {
                ClientCommands.Instance.sendStream("set controls/flight/elevator " + value);
            }
        }

    }
}
