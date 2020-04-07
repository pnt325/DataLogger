using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace new_chart_delections.gui_new
{
    public class Connection
    {

    }

    public class DistObject
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }


    public class Uart
    {
        #region PROPERTY
        private SerialPort serialPort;

        public string PortName { get; set; } = "";
        public int BaudRate { get; set; } = 9600;  // default value.

        public event EventHandler Disconnected;
        #endregion

        #region METHOD

        #endregion
        public Uart()
        {
            serialPort = new SerialPort();
        }

        private Queue<string> recvBuffer = new Queue<string>();
        private Queue<DistObject> storeBuffer = new Queue<DistObject>();

        private void SerialReceiving()
        {
            bool isError = false;

            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();

            string msg = "";
            while (true)
            {
                try
                {
                    msg = serialPort.ReadLine();
                }
                catch
                {
                    isError = true;
                    break;
                }

                string[] datas = msg.Split(':');

                if (datas.Length != 2)
                {
                    continue;
                }

                datas[1] = datas[1].Trim('\r');
                try
                {
                    object value = float.Parse(datas[1]);

                    if (!Program.MemoryManage.Address.ContainsKey(datas[0]))
                    {
                        continue;
                    }

                    int address = Program.MemoryManage.Address[datas[0]];
                    Program.MemoryManage.Write(address, value);

                }
                catch { }
            }

            if (isError)
            {
                if (Disconnected != null)
                {
                    Disconnected(null, null);
                }
            }
        }

        public bool Connect()
        {
            serialPort.PortName = PortName;
            serialPort.BaudRate = BaudRate;

            try
            {
                serialPort.Open();
                Thread thReceive = new Thread(new ThreadStart(SerialReceiving));
                thReceive.IsBackground = true;
                thReceive.Start();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Disconnect()
        {
            serialPort.Close();
        }

        public bool Status
        {
            get { return serialPort.IsOpen; }
        }

    }
}
