using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Communication {
    class Program {
        static void Main(string[] args) {
            IPAddress ipaddr = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ipaddr, 8888);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true) {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }

    //Class to handle each client request separatly
    public class handleClinet {
        TcpClient clientSocket;
        string clNo;
        private volatile bool stop;
        Thread[] ctThread = new Thread[8];
        

        public void Stop() {
             stop = true;
        }

        public void killthis() {

        }

        public void startClient(TcpClient inClientSocket, string clineNo) {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            ctThread[Convert.ToInt32(clineNo)] = new Thread(new ThreadStart(doChat));
            ctThread[Convert.ToInt32(clineNo)].Start();
        }

        public void doChat() {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10024];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            while (stop == false) {
                try {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    if (dataFromClient == "Close") {
                        Console.WriteLine(" << " + "Close Client --- " + clNo);
                        serverResponse = "Server to clinet(" + clNo + "):  Bye Bye! " + rCount;
                        this.Stop();
                    }
                    else {
                        Console.WriteLine(" >> " + "From client-" + clNo + dataFromClient);
                        rCount = Convert.ToString(requestCount);
                        serverResponse = "Server to clinet(" + clNo + ") " + rCount;
                    }
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                }
                catch (Exception ex) {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }
    }
}