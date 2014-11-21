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
        int numChecks = 0; //para no comer mas de una ficha por turno

        public Computer()
        {

        }

        public Tablero play(Tablero tableroActualizado)
        {
            this.tablero = tableroActualizado;
            ChecarTablero();
            move();

            //Después de la jugada, devolver el tablero para dibujarlo de nuevo
            return this.tablero;
        }

        public void ChecarTablero()
        {
            //Reset de listaMOvimientos
            listaMovimientos.Clear();

            for (int i = 0; i < 32; i++)
            {
                Ficha ficha = tablero.getFicha(i); //La ficha de esa posicion si no hay, regresa null 
                if (ficha != null) //Hay una ficha en esta posicion
                {
                    int color = ficha.getColor();
                    if (i % 8 == 0 || i % 8 == 7) //si es orilla
                    {
                        if (color == 1) //1: es Blanco
                        {
                            check(i, i + 4, color);
                        }
                        else //2: es negro
                        {
                            check(i, i - 4, color);
                        }
                    }
                    else //si no es orilla
                    {
                        if (color == 1) //1: es Blanco
                        {
                            check(i, i + 4, color);
                            check(i, i + 3, color);
                        }
                        else //2: es negro
                        {
                            check(i, i - 4, color);
                            check(i, i - 3, color);
                        }
                    }

                    //if reina
                }
            }
        }

        public void move()
        {
            if (listaMovimientos == null) //no hay movimientos validos 
            {
                //que hacer?
            }
            else //hay algun movimiento? 
            {
                //aplicar movimiento en el tablero, no eliminar de la lista
                //reset atacar = false
            }
        }

        public void check(int posini, int posfin, int color)
        {
            if (numChecks < 2)
            {
                numChecks++;
                Ficha ficha = tablero.getFicha(posfin);
                int colorFicha = ficha.getColor();
                if (ficha == null) //no hay una ficha ahi
                {
                    Movimiento mov = new Movimiento(posini, posfin);
                    if (numChecks == 1)
                    {
                        ficha.setAtacar(true);
                        listaMovimientos.Insert(0, mov);
                    }
                    else
                    {
                        listaMovimientos.Add(mov);
                    }
                }
                else //hay alguna ficha
                {
                    if (colorFicha == color) //es un color diferente al mio
                    {
                        if (color == 1) //soy ficha blanca
                            check(posfin, posfin + 4, color);
                        else //soy ficha negra
                            check(posfin, posfin - 4, color);
                    }
                }
            }
            else
            {
                numChecks = 0;
                //no hay movimiento
            }
        }
    }
}
