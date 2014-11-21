using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNuevo
{
    class Movimiento
    {
        int posIni;
        int posFin;

        public Movimiento(int posIni, int posFin)
        {
            this.posIni = posIni;
            this.posFin = posFin;
        }

        public int getPosIni()
        {
            return posIni;
        }
        public int getPosFin()
        {
            return posFin;
        }
    }
}
