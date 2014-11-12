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
        private Ficha[] piezas;

        //Número de piezas de cada jugador
        private int whitePieces;
        private int blackPieces;

        //Turno
        private int turno;

        public Tablero()
        {
            piezas = new Ficha[32];
            clearBoard();
        }

        public void clearBoard()
        {
            int i;


            whitePieces = 12;
            blackPieces = 12;

            turno = 1;

            //blancas
            for (i = 0; i < 12; i++)
                piezas[i] = new Ficha(i, 1, false, false);
            //vacías
            for (i = 12; i < 20; i++)
                piezas[i] = null;
            //negras
            for (i = 20; i < 32; i++)
                piezas[i] = new Ficha(i, 2, false, true);
        }

        //Cambiar turno
        //
        public void setTurno(int player)
        {
            turno = player;
        }

        //Obtener tablero de juego
        public Ficha[] getPiezas()
        {
            return piezas;
        }

        //Obtener ficha en posición i del tablero
        public Ficha getFicha( int i)
        {
            return piezas[i];
        }
    }
}
