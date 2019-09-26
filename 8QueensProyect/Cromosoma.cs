using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8QueensProyect
{
    public class Cromosoma: IComparable<Cromosoma>
    {
        public List<int> Genes{ get; set; }
        public int Colisiones { get; set; }

        public double Fitness { get; set; }

        public int Generacion { get; set; }
        public int Numero { get; set; }

        public Cromosoma()
        {
            Genes = new List<int>();
        }

        public int CompareTo(Cromosoma other)
        {
            return (Colisiones.CompareTo(other.Colisiones));
        }
    }
}
