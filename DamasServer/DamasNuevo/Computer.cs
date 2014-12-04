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
        public int color; //color de fichas que le tocó, según el turno
        int fichaComida = -1;

        public Computer()
        {
        }
        
        public Computer(int jugador)
        {
            color = jugador;
        }

        public Tablero play(Tablero tableroActualizado)
        {
            listaMovimientos.Clear();
            this.tablero = tableroActualizado;
            Casilla[] casillas = tablero.getCasillas();
            for (int i = 0; i < casillas.Length; i++)
            {   
                if(casillas[i].getFicha() != null && casillas[i].getFicha().getColor()==color)
                    ChecarCasilla(casillas[i]);
            }
            //move();
            

            //Después de la jugada, devolver el tablero para dibujarlo de nuevo
            return this.tablero;
        }

        public Tablero play(Tablero tableroActualizado, Movimiento accion) {
            //listaMovimientos.Clear();
            this.tablero = tableroActualizado;
            Casilla[] casillas = tablero.getCasillas();
            for (int i = 0; i < casillas.Length; i++) {
                if (casillas[i].getFicha() != null && casillas[i].getFicha().getColor() == color)
                    ChecarCasilla(casillas[i]);
            }
            move(accion);


            //Después de la jugada, devolver el tablero para dibujarlo de nuevo
            return this.tablero;
        }

        public List<Movimiento> getListaMovimientos() {
            return listaMovimientos;
        }

        public void ChecarCasilla(Casilla casilla)
        {
            int[] vecinos = casilla.getVecinos(); //checo los vecinos de esta casilla 
            Casilla[] casillas = tablero.getCasillas(); //para revisar todas las casillas del tablero

            if (casilla.getFicha().getCoronada() == true) //es reina y tengo que revisar todos sus limites 
            {
                for (int i = 0; i < 4; i++) //reviso todos los vecinos
                {
                    if(vecinos[i] > -1){
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
                                if(vecinos[i] > -1){
                                    Ficha fichaVecino = tablero.getFicha(vecinos[i]);
                                    if (fichaVecino != null) //hay ficha
                                    {
                                        if (color != fichaVecino.getColor())
                                        {
                                            checkVecino(i, casillas[fichaVecino.getPosicion()],casilla.getFicha().getPosicion()); 
                                        }
                                    }else
                                    {
                                        //no hay ficha, es movimiento válido
                                        Movimiento movimiento = new Movimiento(casilla.getFicha().getPosicion(), vecinos[i]);
                                        listaMovimientos.Add(movimiento);
                                    }
                                }
                            }

                        } break;
                    case 2: //es negro
                        {
                            for (int i = 0; i < 2; i++) //reviso los superiores
                            {
                                if (vecinos[i] > -1)
                                {
                                    Ficha fichaVecino = tablero.getFicha(vecinos[i]);
                                    if (fichaVecino != null) //hay ficha
                                    {
                                        if (color != fichaVecino.getColor())
                                        {
                                            checkVecino(i, casillas[fichaVecino.getPosicion()], casilla.getFicha().getPosicion());
                                        }
                                    }else
                                    {
                                        //no hay ficha, es movimiento válido
                                        Movimiento movimiento = new Movimiento(casilla.getFicha().getPosicion(), vecinos[i]);
                                        listaMovimientos.Add(movimiento);
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
            if (vecinos[direccion] > -1)
            {
                Ficha fichaVecino = tablero.getFicha(vecinos[direccion]); //agarro la ficha del vecino de la direccion a la que se quiere comer
                if (fichaVecino == null) //no hay ficha, entonces es valido y puedo comer
                {
                    Movimiento movimiento = new Movimiento(posini, vecinos[direccion]);
                    listaMovimientos.Insert(0, movimiento); //tengo posibilidad de comer, lo agrego al principio.
                    fichaComida = casilla.getFicha().getPosicion();
                }
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
               ficha.setPosicion(posFin);
               casillas[posFin].setFicha(ficha); //pongo la ficha en la casilla nueva
               if(fichaComida > -1)    casillas[fichaComida].setFicha(null);
               if (posFin >= 0 && posFin <= 3 && color == 2) ficha.setCoronada(true);
               if (posFin >= 28 && posFin <= 31 && color == 1) ficha.setCoronada(true);
               tablero.setCasillas(casillas);
               fichaComida = -1;
            }
        }

        public void move(Movimiento accion) {
            if (listaMovimientos == null) //no hay movimientos validos 
            {
                //perder o empate
            }
            else //hay algun movimiento? 
            {
                int posIni = accion.getPosIni();
                int posFin = accion.getPosFin();

                //tableor destino = tablero origen 
                Casilla[] casillas = tablero.getCasillas(); //tomo las casillas actuales
                Ficha ficha = casillas[posIni].getFicha(); //agarro la ficha que quiero tomar 
                casillas[posIni].setFicha(null); //pongo la casilla en null
                ficha.setPosicion(posFin);
                casillas[posFin].setFicha(ficha); //pongo la ficha en la casilla nueva
                if (fichaComida > -1) casillas[fichaComida].setFicha(null);
                if (posFin >= 0 && posFin <= 3 && color == 2) ficha.setCoronada(true);
                if (posFin >= 28 && posFin <= 31 && color == 1) ficha.setCoronada(true);
                tablero.setCasillas(casillas);
                fichaComida = -1;
            }
        }

    }
}
