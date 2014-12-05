using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace CommClient {
    public partial class Form1 : Form {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;
        String IPAddress;
        int ID = 0;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            
        }



        public void msg(string mesg) {
            richTextBox1.Text = richTextBox1.Text + Environment.NewLine + " >> " + mesg + "\n";
        }

        private void button1_Click(object sender, EventArgs e) {
            string mensaje = "";
            mensaje = mensajeBox.Text;

            serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(mensaje + "$");

            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Close$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void button2_Click(object sender, EventArgs e) {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Close$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10024];
            serverStream.Read(inStream, 0, inStream.Length);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            msg("Data from Server : " + returndata);

            button1.Enabled = false;
            button2.Enabled = false;
            //timer.Enabled = true;

            serverStream.Close();
            clientSocket.Close();
            label1.Text = "Server Desconectado ...";
        }

        private void button3_Click(object sender, EventArgs e) {
            IPAddress = textBox1.Text;
            //IPAddress = "127.0.0.1";

            try {
                clientSocket.Connect(IPAddress, 8888);
                msg("Client Started");
                label1.Text = "Client Socket Program - Server Connected ...";
            }
            catch {
                label1.Text = "No se pudo conectar";
                msg("No conexion");
            }

            button1.Enabled = true;
            button2.Enabled = true;
            
        }

        private void button4_Click(object sender, EventArgs e) {
            byte[] inStream = new byte[10024];
            try {
                serverStream = clientSocket.GetStream();
                serverStream.Read(inStream, 0, inStream.Length);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                msg("Data from Server : " + returndata);
            }
            catch (Exception) {
            }
        }

        private void timer_Tick(object sender, EventArgs e) {
            
            byte[] inStream = new byte[10024];
            try {
                serverStream = clientSocket.GetStream();
                serverStream.Read(inStream, 0, inStream.Length);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                msg("Data from Server : " + returndata);
            }
            catch (Exception) {
            }
        }
    }
}
