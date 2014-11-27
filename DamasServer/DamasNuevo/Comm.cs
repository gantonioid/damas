using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcpServer;

namespace DamasNuevo {
    class Comm {
        //public delegate void invokeDelegate();
        TcpServer server;

        public Comm() {
            server = new TcpServer(); //in constructor (auto added if added as a component)
            openTcpPort(8888);

        }

        public void openTcpPort(int port) {
            server.Port = port;
            server.Open();
            displayTcpServerStatus();
        }

        public void closeTcpPort() {
            server.Close();
        }

        public void displayTcpServerStatus() {
            if (server.IsOpen) {
                Console.WriteLine("PORT OPEN");
            }
            else {
                Console.WriteLine("NOT OPEN");
            }
        }

        

        public void send(string data) {
            data = data.Substring(0, data.Length - 2);

            server.Send(data);
        }
    }
}
