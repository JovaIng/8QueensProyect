using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8QueensProyect
{
    public class FitnessAnalizer
    {
        public List<Cromosoma> LstCromosomas { get; set; }
        public int PromedioColisiones { get; set; }
        public List<List<int>> LstTablero { get; set; }

        public FitnessAnalizer(List<Cromosoma> lstIndividuos, List<List<int>> lstTablero)
        {
            LstCromosomas = lstIndividuos;
            LstTablero = lstTablero;
        }

        public bool ValidarFitness()
        {
            bool success = false;
            int colisiones = 0;
            for (int i = 0; i < LstCromosomas.Count; i++)
                colisiones += ObtenerFitnessCromosoma(LstCromosomas[i], LstTablero);

            if (colisiones.Equals(0))
                success = true;
            return success;
        }

        public int ObtenerFitnessCromosoma(Cromosoma cromosoma, List<List<int>> lstTablero)
        {
            // para encontrar el número de colisiones es importante analizar por cada gen del cromosoma.
            // Deberá analizarse las colisiones por fila, columna y diagonal en base al comportamiento de una reina de ajedrez.
            // en un cuadro de N posiciones el nivel es el número de fila.

            // para el caso base, el tablero se compone de 8 filas de 8 de longitud.

            // Nivel 0 => (00, 07)
            // Nivel 1 => (08, 15)
            // Nivel 2 => (16, 23)
            // Nivel 3 => (24, 31)
            // Nivel 4 => (32, 39)
            // Nivel 5 => (40, 47)
            // Nivel 6 => (48, 55)
            // Nivel 7 => (56, 63)

            List<int> lstNiveles = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                //Analiza fila.
                int gen = cromosoma.Genes[i];
                int nivel = ObtieneNivel(gen);
                for(int j = 0; j < lstTablero.Count; j++)
                {
                    if (i != j)
                    {
                        int genAux = cromosoma.Genes[j];
                        if (nivel.Equals(ObtieneNivel(genAux)))
                            cromosoma.Colisiones++;
                    }
                }

                // analizar columna.
                int index = lstTablero[nivel].IndexOf(gen);
                for (int j = 0; j < lstTablero.Count; j++)
                {
                    List<int> genes = lstTablero[j];
                    int genAux = genes[index];
                    if(genAux != gen)
                    {
                        if (cromosoma.Genes.Contains(genAux))
                            cromosoma.Colisiones++;
                    }
                }

                // analizar diagonal.

            }

            cromosoma.Fitness = 28 - cromosoma.Colisiones;
            return cromosoma.Colisiones;
        }

        internal List<Cromosoma> ObtenerLosMejores(int limit)
        {
            LstCromosomas.Sort();
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            for (int i = 0; i < 2; i++)
                lstCromosomas.Add(LstCromosomas[i]);
            return lstCromosomas;
        }

        internal List<Cromosoma> ObtenerLosPeores(int limit)
        {
            LstCromosomas.Sort();
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            for (int i = (LstCromosomas.Count-1)-limit; i < LstCromosomas.Count; i++)
                lstCromosomas.Add(LstCromosomas[i]);
            return lstCromosomas;
        }

        public int ObtieneNivel(int posicion)
        {
            int nivel = 0;
            List<int> lstNivel = new List<int>();
            for (nivel = 0; nivel < LstTablero.Count; nivel++)
            {
                lstNivel = LstTablero[nivel];
                if (lstNivel.Contains(posicion))
                    break;
            }

            return nivel;
        }
    }
}
