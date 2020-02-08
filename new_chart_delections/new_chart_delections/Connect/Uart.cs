using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace new_chart_delections.Connect
{
    public class Uart
    {
        SerialPort serialPort;

        public event EventHandler Disconnected;

        public Uart()
        {
            serialPort = new SerialPort();
        }

        public bool Connect(string portName, int baudRate)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }

            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;

            try
            {
                serialPort.Open();
            }
            catch
            {
                return false;
            }

            Thread thReceiveData = new Thread(() =>
            {
                while (serialPort.IsOpen)
                {
                    if(serialPort.BytesToRead > 0)
                    {
                        try
                        {
                            // TODO add protocol to receive data
                        }
                        catch
                        {
                            break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                Disconnected?.Invoke(this, null);
            });
            thReceiveData.IsBackground = true;
            //thReceiveData.Priority = ThreadPriority.AboveNormal;
            thReceiveData.Start();

            return true;
        }

        public void Disconnect()
        {
            serialPort.Close();
        }
    }
}
