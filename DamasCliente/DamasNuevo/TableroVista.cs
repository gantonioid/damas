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

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;
        String IPAddress;

        bool ganar, perder, tablas, conexion = false;
        bool gotMessage = false;

        Thread juego = null;

        //iniciar aplicacion
        public TableroVista()
        {
            InitializeComponent();
            tablero = new Tablero();

            //Establecer Comunicación
            //
            IPAddress = "127.0.0.1";
            
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
            byte[] inStream = new byte[10024];
            string returndata = "";

            string[] Jugador2Str = null;
            string[] Jugador2params = null;
            int posini, posfin;
            
            string mensaje = "";

            while (!ganar && !perder && !tablas && !conexion)
            {
                //CICLO-------
                //Esperar turno
                while (jugador.color != tablero.getTurno())
                {
                    tablero.setTurno(1);
                }
                //Generar jugada, tirar y actualizar tablero
                tablero = jugador.play(this.tablero);
                //Volver a pintar
                Invalidate();
                Thread.Sleep(500);

                //enviar al servidor --- "jugador.listaMovimientos(0)"
                //Envio de mensaje
                movimiento = jugador.getListaMovimientos()[0];
                data = "mover:" + movimiento.getPosIni() + "," + movimiento.getPosFin();
                SendMessage(data);
                Console.WriteLine(data);

                //Turno 2
                tablero.setTurno(2);
                Console.WriteLine("turno 2");

                mensaje = getMessage();
                //PARSEAR MENSAJE 
                Jugador2Str = mensaje.Split(':');
                Console.WriteLine("Cliente: " + Jugador2Str[0]);
                Console.WriteLine("Comando: " + Jugador2Str[1]);
                Jugador2params = Jugador2Str[2].Split(',');
                Console.WriteLine("Param1: " + Jugador2params[0]);
                Console.WriteLine("Param2: " + Jugador2params[1]);

                posini = Int32.Parse(Jugador2params[0]);
                posfin = Int32.Parse(Jugador2params[1]);

                Movimiento movimiento2 = new Movimiento(posini, posfin);

                tablero = oponentePrueba.play(this.tablero, movimiento2);        //---------------------Sólo para probar el juego, QUITAR ESTO!!!

                //Volver a pintar
                Invalidate();
                Thread.Sleep(500);
                //Esperar movimiento del rival
                //FIN CICLO-----
            }
        }

        //jugar como rojo
        public void jugar2() {
            Movimiento movimiento = null;
            string data = "";
            byte[] inStream = new byte[10024];
            string returndata = "";

            string[] Jugador1Str = null;
            string[] Jugador1params = null;
            int posini, posfin;

            string mensaje = "";

            while (!ganar && !perder && !tablas && !conexion) {
                //CICLO-------
                
                //Turno 2
                tablero.setTurno(1);
                Console.WriteLine("turno 1");

                mensaje = getMessage();
                //PARSEAR MENSAJE 
                Jugador1Str = mensaje.Split(':');
                Console.WriteLine("Cliente: " + Jugador1Str[0]);
                Console.WriteLine("Comando: " + Jugador1Str[1]);
                Jugador1params = Jugador1Str[2].Split(',');
                Console.WriteLine("Param1: " + Jugador1params[0]);
                Console.WriteLine("Param2: " + Jugador1params[1]);

                posini = Int32.Parse(Jugador1params[0]);
                posfin = Int32.Parse(Jugador1params[1]);

                Movimiento movimiento1 = new Movimiento(posini, posfin);

                tablero = jugador.play(this.tablero, movimiento1);        //---------------------Sólo para probar el juego, QUITAR ESTO!!!

                //Volver a pintar
                Invalidate();
                Thread.Sleep(500);
                //
                //FIN CICLO-----

                //Esperar turno
                tablero.setTurno(2);
                //Generar jugada, tirar y actualizar tablero
                tablero = oponentePrueba.play(this.tablero);
                //Volver a pintar
                Invalidate();
                Thread.Sleep(500);
                //enviar al servidor --- "jugador.listaMovimientos(0)"
                //Envio de mensaje
                movimiento = oponentePrueba.getListaMovimientos()[0];
                data = "mover:" + movimiento.getPosIni() + "," + movimiento.getPosFin();
                SendMessage(data);
                Console.WriteLine(data);

            }
        }

        

        private void TableroVista_FormClosed(object sender, FormClosedEventArgs e) {
            juego.Abort();
            juego.Interrupt();
            serverStream.Flush();
            serverStream.Close();
        }
        //Communicacion

        public void SendMessage(string message) {
            serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);

            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public string getMessage() {
            byte[] inStream = new byte[10024];
            string returndata = "";
            serverStream = clientSocket.GetStream();
            serverStream.Read(inStream, 0, inStream.Length);
            returndata = System.Text.Encoding.ASCII.GetString(inStream);
            //MAnejando el EOF
            returndata = returndata.Substring(0, returndata.IndexOf("\0"));


            return returndata;
        }

        private void Jugar(object sender, EventArgs e) {

            try {
                clientSocket.Connect(IPAddress, 8888);
            }
            catch {
            }

            jugador.color = 1;
            oponentePrueba.color = 2;

            string mensaje = getMessage();

            if (mensaje == "Color:Blanco") {
                juego = new Thread(new ThreadStart(jugar));
            }
            else {
                juego = new Thread(new ThreadStart(jugar2));
            }

            mensaje = getMessage();

            if (mensaje == "Jugar") {
                juego.Start();
            }
        }

    }
}
