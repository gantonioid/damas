using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Pruebacomunicacion
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public String text_to_send; 

        public Form1()
        {
            InitializeComponent();

            //Para obtener mi IP
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());

            foreach(IPAddress address in localIP) 
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    txtServerIP.Text = address.ToString();
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

      
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(txtServerPort.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync(); //para empezar a recibir datos
            backgroundWorker2.WorkerSupportsCancellation = true; //Poder cancelar este mensaje


        }

        //recibir datos 
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    receive = STR.ReadLine();
                    this.txtMessages.Invoke(new MethodInvoker(delegate() { txtMessages.AppendText("You: " + receive + "\n"); }));
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }

        //enviar datos
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                STW.WriteLine(text_to_send);
                this.txtMessages.Invoke(new MethodInvoker(delegate() { txtMessages.AppendText("Me: " + text_to_send + "\n"); }));
            }
            else
            {
                MessageBox.Show("Send Failed");
            }
            backgroundWorker2.CancelAsync();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IP_End=new IPEndPoint(IPAddress.Parse(txtClientIP.Text), int.Parse(txtClientPort.Text));

            try
            {
                client.Connect(IP_End);
                if (client.Connected)
                {
                    txtMessages.AppendText("Connected to Server"+"\n");
                    STW = new StreamWriter(client.GetStream());
                    STR = new StreamReader(client.GetStream());
                    STW.AutoFlush = true;

                    backgroundWorker1.RunWorkerAsync(); //para empezar a recibir datos
                    backgroundWorker2.WorkerSupportsCancellation = true; //Poder cancelar este mensaje
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message.ToString());
            }
        
        
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text != "")
            {
                text_to_send = txtMessage.Text;
                backgroundWorker2.RunWorkerAsync();

            }

            txtMessage.Text = "";
        }
    }
}
