using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DataLogging
{
    class Serial
    {
        // Port connect variable
        string port;
        int baud;
        SerialPort serialPort;

        // Const value config
        public const int OPEN_FAIL_BY_OTHER_SW = 1;
        public const int OPEN_FAILE = 2;
        public const int OPEN_SUCCESS = 3;

        // Thread
        Thread thReceiveData;

        // Ref function
        Func<int, int> funcReceiveData;

        public Serial(string _port, int _baud)
        {
            port = _port;
            baud = _baud;
        }

        public void SetBaud(int _baud)
        {
            this.baud = _baud;
        }

        public void SetPort(string _port)
        {
            this.port = _port;
        }

        int Connect()
        {
            serialPort.BaudRate = this.baud;
            serialPort.PortName = this.port;

            if (serialPort.IsOpen)
                return OPEN_FAIL_BY_OTHER_SW;  // serial is open by another  software
            try
            {
                serialPort.Open();
            }
            catch (Exception)
            {
                return OPEN_FAILE;   // can not open serialport
            } 
            return OPEN_SUCCESS;   // open serialport successed
        }

        void DisConnect()
        {
            try
            {
                serialPort.Close();
            }
            catch (Exception)
            {
                return;
            }
        }

        bool GetState()
        {
            return serialPort.IsOpen;
        }
        
    }
}
