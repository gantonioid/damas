using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNuevo
{
    class Computer
    {
        //Variables globales
        Tablero tablero;
        List<Movimiento> listaMovimientos = new List<Movimiento>();  //este debe ser un arreglo de posicion inicial y final

        public Computer()
        {

        }

        public Tablero play(Tablero tableroActualizado)
        {
            this.tablero = tableroActualizado;
            Casilla[] casillas = tablero.getCasillas();
            for (int i = 0; i < casillas.Length; i++)
            {   
                ChecarCasilla(casillas[i]);
            }
            move();

            //Después de la jugada, devolver el tablero para dibujarlo de nuevo
            return this.tablero;
        }

        public void ChecarCasilla(Casilla casilla)
        {
            int[] vecinos = casilla.getVecinos(); //checo los vecinos de esta casilla 
            Casilla[] casillas = tablero.getCasillas(); //para revisar todas las casillas del tablero

            if (casilla.getFicha().getCoronada() == true) //es reina y tengo que revisar todos sus limites 
            {
                for (int i = 0; i < 4; i++) //reviso todos los vecinos
                {
                    Ficha fichaVecino = tablero.getFicha(vecinos[i]);
                    if (fichaVecino != null) //hay ficha en la posicion del vecino i
                    {
                        if (fichaVecino.getColor() != casilla.getFicha().getColor()) //es diferente a mi propio color
                        {
                            checkVecino(i, casillas[fichaVecino.getPosicion()], casilla.getFicha().getPosicion()); 
                        }
                    }
                    else //si no hay ficha en esta posicion
                    {
                        Movimiento movimiento=new Movimiento(casilla.getFicha().getPosicion(), vecinos[i]);
                        listaMovimientos.Add(movimiento); //no voy a comer, lo agrego al final
                    }

                }
            }
            else //no es reina
            {
                int color=casilla.getFicha().getColor();
                switch (color)
                {
                    case 1: //es blanco
                        {
                            for (int i = 2; i < 4; i++) //reviso los inferiores
                            {
                                Ficha fichaVecino = tablero.getFicha(vecinos[i]);
                                if (fichaVecino != null) //hay ficha
                                {
                                    if (color != fichaVecino.getColor())
                                    {
                                        checkVecino(i, casillas[fichaVecino.getPosicion()],casilla.getFicha().getPosicion()); 
                                    }
                                }
                            }

                        } break;
                    case 2: //es negro
                        {
                            for (int i = 0; i < 2; i++) //reviso los superiores
                            {
                                Ficha fichaVecino = tablero.getFicha(vecinos[i]);
                                if (fichaVecino != null) //hay ficha
                                {
                                    if (color != fichaVecino.getColor())
                                    {
                                        checkVecino(i, casillas[fichaVecino.getPosicion()], casilla.getFicha().getPosicion());
                                    }
                                }
                            }

                        } break;
                }
            }
        }

        public void checkVecino(int direccion, Casilla casilla, int posini)
        {
            int[] vecinos=casilla.getVecinos();
            Casilla[] casillas = tablero.getCasillas();
            Ficha fichaVecino = tablero.getFicha(vecinos[direccion]); //agarro la ficha del vecino de la direccion a la que se quiere comer
            if(fichaVecino!=null) //no hay ficha, entonces es valido y puedo comer
            {
                Movimiento movimiento=new Movimiento(posini, vecinos[direccion]);
                listaMovimientos.Insert(0,movimiento); //tengo posibilidad de comer, lo agrego al principio. 
            }
        }

        public void move()
        {
            if (listaMovimientos == null) //no hay movimientos validos 
            {
                //perder o empate
            }
            else //hay algun movimiento? 
            {
               Movimiento accion=listaMovimientos[0]; //tomo el primer movimiento valido
               int posIni=accion.getPosIni();
               int posFin=accion.getPosFin();
               //tableor destino = tablero origen 
               Casilla[] casillas = tablero.getCasillas(); //tomo las casillas actuales
               Ficha ficha = casillas[posIni].getFicha(); //agarro la ficha que quiero tomar 
               casillas[posIni].setFicha(null); //pongo la casilla en null
               casillas[posFin].setFicha(ficha); //pongo la ficha en la casilla nueva
               tablero.setCasillas(casillas);

            }
        }

    }
}
