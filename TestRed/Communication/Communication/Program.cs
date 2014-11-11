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
        public static bool Halt = false;

        static void Main(string[] args) {
            string OwnIP;
            IPAddress ipaddr;
            TcpListener serverSocket = null;
            TcpClient clientSocket = null;
            int counter = 0;
            bool Init = false;

            while (!Init) {
                Console.WriteLine(" >> " + "Escriba la ip del servidor en la red.");
                OwnIP = Console.ReadLine();
                try {
                    ipaddr = IPAddress.Parse(OwnIP);
                    serverSocket = new TcpListener(ipaddr, 8888);
                    clientSocket = default(TcpClient);
                    serverSocket.Start();
                    Console.WriteLine(" >> " + "Server Started -- Presione Q para Terminar");
                    Init = true;
                    
                }
                catch {
                    Console.WriteLine(" >> " + "Error en la IP");
                }
            }

            //Variable de tecla para terminar el programa 
            //Thread HaltThread = new Thread(CloseProg);
            //HaltThread.Start();

            if (serverSocket != null) {
                while (!Halt) {
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

        public static void CloseProg(){
            ConsoleKeyInfo keyinfo;
            do {
                keyinfo = Console.ReadKey();
                //Console.WriteLine(keyinfo.Key + " was pressed");
            }
            while (keyinfo.Key != ConsoleKey.Q);
            Halt = true;
        }
    }

    //Class to handle each client request separatly
    public class handleClinet {
        TcpClient clientSocket;
        string clNo;
        private volatile bool stop;
        Thread[] ctThread = new Thread[25];
        

        public void Stop() {
             stop = true;
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
                        serverResponse = "Server to client(" + clNo + "):  Bye Bye! " + clNo + "\n";
                        this.Stop();
                    }
                    else {
                        Console.WriteLine(" >> " + "From client:   " + clNo + " >_ " + dataFromClient);
                        rCount = Convert.ToString(requestCount);
                        serverResponse = "Server to clinet(" + clNo + ") " + rCount + "\n";
                    }
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                }
                catch (Exception ex) {
                    Console.WriteLine(" >> Error de lectura" );
                    this.Stop();
                }
            }
        }
    }
}