using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNuevo
{
    class Ficha
    {
        private int posicion;   //En el tablero
        private int color;      //Jugador... 1 = blanco, 2 = negro
        private bool atacar;    //Puede comer una ficha
        private bool coronada;      //Está coronada

        public Ficha(int posicion, int color, bool attack, bool king)
        {
            this.posicion = posicion;
            this.color = color;
            this.atacar = attack;
            this.coronada = king;
        }

        public int getPosicion()
        {
            return posicion;
        }
        
        public void setPosicion(int pos)
        {
            this.posicion = pos;
        }

        public int getColor()
        {
            return color;
        }

        public void setColor(int color)
        {
            this.color = color;
        }

        public bool getAtacar()
        {
            return atacar;
        }

        public void setAtacar(bool atacar)
        {
            this.atacar = atacar;
        }

        public bool getCoronada()
        {
            return coronada;
        }

        public void setCoronada(bool coronada)
        {
            this.coronada = coronada;
        }
    }
}
