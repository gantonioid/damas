using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNuevo
{
    class Casilla
    {
        private Ficha ficha;
        private int[] vecino { get; set; } //supIzq, supDer, infIzq, infDer

        public Casilla(Ficha ficha, int[] vecinos){
            this.ficha = ficha;
            this.vecino = vecinos;
        }

        public void setFicha(Ficha ficha)
        {
            this.ficha = ficha;
        }

        public Ficha getFicha()
        {
            return ficha;
        }

        public void setVecinos(int[] vecinos)
        {
            this.vecino = vecinos;
        }

        public int[] getVecinos()
        {
            return vecino;
        }

        public int getVecinoSupIzq()
        {
            return vecino[0];
        }

        public int getVecinoSupDer()
        {
            return vecino[1];
        }

        public int getVecinoInfIzq()
        {
            return vecino[2];
        }

        public int getVecinoInfDer()
        {
            return vecino[3];
        }
    }
}
