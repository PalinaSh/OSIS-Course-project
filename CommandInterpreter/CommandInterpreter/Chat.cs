using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommandInterpreter
{
    class Chat
    {
        private string _address = "localhost";
        private int _localPort;
        private int _remotePort;
        private string _name;
        public bool IsReceive { get; set; } = false;

        public Chat(int localPort, int remotePort, string name)
        {
            _localPort = localPort;
            _remotePort = remotePort;
            _name = name;
            IsReceive = true;
        }

        public void SendMessage()
        {
            using (UdpClient client = new UdpClient())
            {
                Send($"{_name} joined chat", client);

                bool process = true;
                while (process)
                {
                    var message = Console.ReadLine();
                    if (message == "exit")
                    {
                        Send($"{_name} lefted chat", client);
                        IsReceive = false;
                        process = false;
                        return;
                    }

                    Send($"{_name}: {message}", client);
                }
            }
        }

        private void Send(string message, UdpClient client)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Send(data, data.Length, _address, _remotePort);
        }

        public void ReceiveMessage(object obj)
        {
            using (UdpClient receiveClient = new UdpClient(_localPort))
            {
                IPEndPoint ip = null;
                try
                {
                    while (IsReceive)
                    {
                        byte[] data = receiveClient.Receive(ref ip);
                        string message = Encoding.Unicode.GetString(data);
                        if (IsReceive)
                            Console.WriteLine(message);
                    }
                }
                catch (Exception)
                {
                    if (!IsReceive)
                        return;
                    throw;
                }
            }
        }
    }
}
