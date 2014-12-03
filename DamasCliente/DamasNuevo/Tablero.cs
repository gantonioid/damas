using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNuevo
{
    class Tablero
    {
        //las casillas del tablero
        //los valores significan el tipo de pieza en la casilla
        private Casilla[] casillas { get; set; }


        //Número de piezas de cada jugador
        private int whitePieces;
        private int blackPieces;

        //Turno
        private int turno;

        public Tablero()
        {
            casillas = new Casilla[32];
            clearBoard();
        }

        public Casilla[] getCasillas()
        {
            return casillas;
        }

        public void setCasillas(Casilla[] casillas)
        {
            this.casillas = casillas;
        }

        //Reset del tablero, nuevas piezas, posiciones por defecto
        public void clearBoard()
        {
            whitePieces = 12;
            blackPieces = 12;

            turno = 1;

            //variables auxiliares para llenar el tablero
            Ficha aux;
            int[] vecinos;

            //blancas-------------------------------------
            //0
            aux = new Ficha(0, 1, false, false);
            vecinos =  new int[4] {-1,-1,4,5};
            casillas[0] = new Casilla(aux, vecinos);
            //1
            aux = new Ficha(1, 1, false, false);
            vecinos = new int[4] {-1,-1,5,6};
            casillas[1] = new Casilla(aux, vecinos);
            //2
            aux = new Ficha(2, 1, false, false);
            vecinos = new int[4] {-1,-1,6,7};
            casillas[2] = new Casilla(aux, vecinos);
            //3
            aux = new Ficha(3, 1, false, false);
            vecinos = new int[4] {-1,-1,7,-1};
            casillas[3] = new Casilla(aux, vecinos);
            //4
            aux = new Ficha(4, 1, false, false);
            vecinos = new int[4] {-1,0,-1,8};
            casillas[4] = new Casilla(aux, vecinos);
            //5
            aux = new Ficha(5, 1, false, false);
            vecinos = new int[4] {0,1,8,9};
            casillas[5] = new Casilla(aux, vecinos);
            //6
            aux = new Ficha(6, 1, false, false);
            vecinos = new int[4] {1,2,9,10};
            casillas[6] = new Casilla(aux, vecinos);
            //7
            aux = new Ficha(7, 1, false, false);
            vecinos = new int[4] {2,3,10,11};
            casillas[7] = new Casilla(aux, vecinos);
            //8
            aux = new Ficha(8, 1, false, false);
            vecinos = new int[4] {4,5,12,13};
            casillas[8] = new Casilla(aux, vecinos);
            //9
            aux = new Ficha(9, 1, false, false);
            vecinos = new int[4] {5,6,13,14};
            casillas[9] = new Casilla(aux, vecinos);
            //10
            aux = new Ficha(10, 1, false, false);
            vecinos = new int[4] {6,7,14,15};
            casillas[10] = new Casilla(aux, vecinos);
            //11
            aux = new Ficha(11, 1, false, false);
            vecinos = new int[4] {7,-1,15,-1};
            casillas[11] = new Casilla(aux, vecinos);
            //vacías--------------------------------------
            //12
            aux = null;
            vecinos = new int[4] { -1, 8, -1, 16 };
            casillas[12] = new Casilla(aux, vecinos);
            //13
            vecinos = new int[4] { 8, 9, 16, 17 };
            casillas[13] = new Casilla(aux, vecinos);
            //14
            vecinos = new int[4] { 9, 10, 17, 18 };
            casillas[14] = new Casilla(aux, vecinos);
            //15
            vecinos = new int[4] { 10, 11, -18, 19 };
            casillas[15] = new Casilla(aux, vecinos);
            //16
            vecinos = new int[4] { 12, 13, 20, 21 };
            casillas[16] = new Casilla(aux, vecinos);
            //17
            vecinos = new int[4] { 13, 14, -21, 22 };
            casillas[17] = new Casilla(aux, vecinos);
            //18
            vecinos = new int[4] { 14, 15, 22, 23 };
            casillas[18] = new Casilla(aux, vecinos);
            //19
            vecinos = new int[4] { 15, -1, 23, -1 };
            casillas[19] = new Casilla(aux, vecinos);
            //negras--------------------------------------
            //20
            aux = new Ficha(20, 2, false, false);
            vecinos = new int[4] { -1, 16, -1, 24 };
            casillas[20] = new Casilla(aux, vecinos);
            //21
            aux = new Ficha(21, 2, false, false);
            vecinos = new int[4] { 16, 17, 24, 25 };
            casillas[21] = new Casilla(aux, vecinos);
            //22
            aux = new Ficha(22, 2, false, false);
            vecinos = new int[4] { 17, 18, 25, 26 };
            casillas[22] = new Casilla(aux, vecinos);
            //23
            aux = new Ficha(23, 2, false, false);
            vecinos = new int[4] { 18, 19, 26, 27 };
            casillas[23] = new Casilla(aux, vecinos);
            //24
            aux = new Ficha(24, 2, false, false);
            vecinos = new int[4] { 20, 21, 28, 29 };
            casillas[24] = new Casilla(aux, vecinos);
            //25
            aux = new Ficha(25, 2, false, false);
            vecinos = new int[4] { 21, 22, 29, 30 };
            casillas[25] = new Casilla(aux, vecinos);
            //26
            aux = new Ficha(26, 2, false, false);
            vecinos = new int[4] { 22, 23, 30, 31 };
            casillas[26] = new Casilla(aux, vecinos);
            //27
            aux = new Ficha(27, 2, false, false);
            vecinos = new int[4] { 23, -1, 31, -1 };
            casillas[27] = new Casilla(aux, vecinos);
            //28
            aux = new Ficha(28, 2, false, false);
            vecinos = new int[4] { -1, 24, -1, -1 };
            casillas[28] = new Casilla(aux, vecinos);
            //29
            aux = new Ficha(29, 2, false, false);
            vecinos = new int[4] { 24, 25, -1, -1 };
            casillas[29] = new Casilla(aux, vecinos);
            //30
            aux = new Ficha(30, 2, false, false);
            vecinos = new int[4] { 25, 26, -1, -1 };
            casillas[30] = new Casilla(aux, vecinos);
            //31
            aux = new Ficha(31, 2, false, false);
            vecinos = new int[4] { 26, 27, -1, -1 };
            casillas[31] = new Casilla(aux, vecinos);
        }

        //Cambiar turno
        //
        public void setTurno(int player)
        {
            turno = player;
        }

        public int getTurno()
        {
            return turno;
        }

        

        //Obtener ficha en posición i del tablero
        public Ficha getFicha( int i )
        {
            return casillas[i].getFicha();
        }

     
    }
}
