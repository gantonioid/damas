using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;

namespace DamasNuevo
{
    //DIBUJA tablero en pantalla
    public partial class TableroVista : Form
    {
        internal int startX, startY;  //coordenadas de la esquina del tablero
        internal int cellWidth;       //tamaño de celda
        Tablero tablero;
        Computer jugador = new Computer();
        Computer oponentePrueba = new Computer();
        public delegate void invokeDelegate();
        bool gotData1 = false;
        string receivedData1 = "";

        bool ganar, perder, tablas, conexion = false;

        Thread juego;

        //iniciar aplicacion
        public TableroVista()
        {
            InitializeComponent();
            tablero = new Tablero();
            //Establecer Comunicación
            StartServer();
            timer.Enabled = true;
            Console.WriteLine("HELLOOOO");
            
            //Asignar turno
            jugador.color = 1;
            oponentePrueba.color = 2;
            
            //MANDAR MENSAJES A LOS JUGADORES         "color:1" al primero que se conecte,       "color:2" al otro
            juego = new Thread(new ThreadStart(jugar));
            
        }

        //Llamado on WM_PAINT
        protected override void OnPaint(PaintEventArgs ev)
        {
            Graphics g = ev.Graphics;
            Size d = ClientSize;
            int marginX;
            int marginY;
            int incValue;

            // Calculates the increments so that we can get a squared
            // board
            if (d.Width < d.Height)
            {
                marginX = 0;
                marginY = (d.Height - d.Width) / 2;

                incValue = d.Width / 8;
            }
            else
            {
                marginX = (d.Width - d.Height) / 2;
                marginY = 0;

                incValue = d.Height / 8;
            }

            startX = marginX;
            startY = marginY;
            cellWidth = incValue;

            drawBoard(g, marginX, marginY, incValue);
            drawPieces(g, marginX, marginY, incValue, ev);
        }

        //Dibujar tablero
        private void drawBoard(Graphics g, int marginX, int marginY, int incValue)
        {
            int pos;
            Brush cellColor;
            //Si queremos fondos a partir de imágenes------
            //Image image = new Bitmap("C:\\Users\\Antonio\\Downloads\\cuadro1.jpg");
            //Image image2 = new Bitmap("C:\\Users\\Antonio\\Downloads\\cuadro2.jpg");
            //---------------------------------------------
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    if ((x + y) % 2 == 0)
                        cellColor = new SolidBrush(Color.White);
                    //cellColor = new TextureBrush(image);
                    else
                    {
                        pos = y * 4 + (x + ((y % 2 == 0) ? -1 : 0)) / 2;

                        cellColor = new SolidBrush(Color.Black);
                        //cellColor = new TextureBrush(image2);
                    }
 
                    g.FillRectangle(cellColor, marginX + x * incValue, marginY + y * incValue, incValue - 1, incValue - 1);
                }
        }

        //Dibujar las fichas
        private void drawPieces(Graphics g, int marginX, int marginY, int incValue, PaintEventArgs ev)
        {
            int x, y;
            Brush pieceColor;

            for (int i = 0; i < 32; i++)
                try
                {
                    if (tablero.getFicha(i) != null)
                    {
                        //Color de la ficha
                        if (tablero.getFicha(i).getColor() == 2)
                            pieceColor = new SolidBrush(Color.Red);
                        else
                            pieceColor = new SolidBrush(Color.White);
                        //Dibujar
                        y = i / 4;
                        x = (i % 4) * 2 + (y % 2 == 0 ? 1 : 0);
                        g.FillEllipse(pieceColor, 0 + marginX + x * incValue, 0 + marginY + y * incValue,
                                    incValue - 1 - 2 * 0, incValue - 1 - 2 * 0);

                        //Revisar si está coronada
                        if (tablero.getFicha(i).getCoronada())
                        {
                            //Dibujarle corona... imagen de corona
                            string path = Path.GetFullPath(@"res");
                            path.Replace("bin\\Debug", "");
                            string file = "\\corona.png";
                            Image image = Image.FromFile(path+file);
                            RectangleF rect = new RectangleF(0 + marginX + x * incValue + (incValue / 4),
                                                            0 + marginY + y * incValue + (incValue / 4),
                                                            incValue - 1 - 2 * 0 - (incValue / 2),
                                                            incValue - 1 - 2 * 0 - (incValue / 2));
                            ev.Graphics.DrawImage(image, rect);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudieron dibujar las piezas\n"+ex.Message,"Error");
                }
        }

        public void jugar()
        {
            Movimiento movimiento = null;
            string data = "";
            string[] Jugador1Str = null;
            string[] Jugador1params = null;
            int posini, posfin;

            while (!ganar && !perder && !tablas && !conexion)
            {
                //DECIRLE AL JUGADOR 1 QUE JUEGUE
                tablero.setTurno(1);

                //ESPERAR MENSAJE 
                while (gotData1 == false)
                {
                    Thread.Sleep(100);
                    //revisar si ya recibio el mensaje, actualizar receivedData
                }
                gotData1 = false;


                //PARSEAR MENSAJE
                Jugador1Str = receivedData1.Split(':');
                Console.WriteLine("Comando: " + Jugador1Str[0]);
                Console.WriteLine("Mensaje: " + Jugador1Str[1]);
                Jugador1params = Jugador1Str[1].Split(',');
                Console.WriteLine("Param1: " + Jugador1params[0]);
                Console.WriteLine("Param2: " + Jugador1params[1]);

                posini = Int32.Parse(Jugador1params[0]);
                posfin = Int32.Parse(Jugador1params[1]);

                //HACER OBJETO TIPO "MOVIMIENTO" CON LA INFO DEL MENSAJE
                Movimiento movimiento1 = new Movimiento(posini, posfin);

                //DECIRLE A "COMPUTER" QUE ANALICE LOS MOVIMIENTOS LEGALES


                //SI EL MOVIMIENTO DEL MENSAJE ESTA EN LA LISTA, APLICAR MOVIMIENTO CON COMPUTER.MOVE(MOVIMIENTO), para que se actualice el tablero del servidor
                //jugador.move(movimiento1);

                //Volver a pintar
                Invalidate();
                //Thread.Sleep(3000);

                //CAMBIAR TURNO Y DECIRLE AL JUGADOR 2 QUE JUEGE, /////////VOLVER A HACER TODO PARA EL JUGADOR 2
                tablero.setTurno(2);
                //Generar jugada, tirar y actualizar tablero
                tablero = jugador.play(this.tablero, movimiento1);

                //envio de mensaje de confirmacion
                comm.Send("OK", 0);
                
                

                //Envio de mensaje
                movimiento = jugador.getListaMovimientos()[0];
                data = "mover:" + movimiento.getPosIni() + "," + movimiento.getPosFin();
                //comm.Send(data);

                tablero = oponentePrueba.play(this.tablero);        //---------------------Sólo para probar el juego, QUITAR ESTO!!!

                //Volver a pintar
                Invalidate();
                //Thread.Sleep(500);
                //Esperar movimiento del rival
                //FIN CICLO-----
            }
        }


        //Metodos dedicados para la comunicacion
        private void StartServer() {
            comm.Port = 8888;
            comm.Open();
        }

        public void displayTcpServerStatus() {
            if (comm.IsOpen) {
                Console.WriteLine("PORT OPEN");
            }
            else {
                Console.WriteLine("NOT OPEN");
            }
        }


        protected byte[] readStream(TcpClient client) {
            NetworkStream stream = client.GetStream();
            if (stream.DataAvailable) {
                byte[] data = new byte[client.Available];

                int bytesRead = 0;
                try {
                    bytesRead = stream.Read(data, 0, data.Length);
                }
                catch (IOException) {
                }

                if (bytesRead < data.Length) {
                    byte[] lastData = data;
                    data = new byte[bytesRead];
                    Array.ConstrainedCopy(lastData, 0, data, 0, bytesRead);
                }
                return data;
            }
            return null;
        }

        public void logData(bool sent, string text) {
            //txtLog.Text += "\r\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt") + (sent ? " SENT:\r\n" : " RECEIVED:\r\n");
            txtLog.Text += text;
            txtLog.Text += "\r\n";
            if (txtLog.Lines.Length > 500) {
                string[] temp = new string[500];
                Array.Copy(txtLog.Lines, txtLog.Lines.Length - 500, temp, 0, 500);
                txtLog.Lines = temp;
            }

            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        public string getData(string message) {
            gotData1 = true;
            receivedData1 = message;

            return message;
        }

        private void comm_OnConnect(tcpServer.TcpServerConnection connection) {
            invokeDelegate setText = () => {
                lblConnected.Text = comm.Connections.Count.ToString();
                juego.Start();
            };

            Invoke(setText);
        }

        private void comm_OnDataAvailable(tcpServer.TcpServerConnection connection) {
            byte[] data = readStream(connection.Socket);

            if (data != null) {
                string dataStr = Encoding.ASCII.GetString(data);

                invokeDelegate del = () => {
                    logData(false, dataStr);
                    getData(dataStr);
                };
                Invoke(del);

                data = null;
            }
        }

        private void timer_Tick(object sender, EventArgs e) {
            invokeDelegate setText = () => lblConnected.Text = comm.Connections.Count.ToString();

            Invoke(setText);

            //send("holl");
        }

        private void TableroVista_FormClosed(object sender, FormClosedEventArgs e) {
            comm.Close();
            juego.Abort();
        }

        private void send(string data) {
            data = data.Substring(0, data.Length - 2);
            comm.Send(data);
        }

        private void button1_Click(object sender, EventArgs e) {
            string data = "hh";
            //data = data.Substring(0, data.Length - 2);
            comm.Send(data);
        }
    }
}
