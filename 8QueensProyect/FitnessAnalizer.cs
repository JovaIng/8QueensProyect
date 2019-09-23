using System.Collections.Generic;

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

        /// <summary>
        /// Aplica fitness a todos los individuos de la población.
        /// </summary>
        /// <returns></returns>
        public bool GenerarFitnessCromosomas()
        {
            bool success = false;
            int colisiones = 0;
            for (int i = 0; i < LstCromosomas.Count; i++)
                colisiones += ObtenerFitnessCromosoma(LstCromosomas[i], LstTablero);

            if (colisiones.Equals(0))
                success = true;
            return success;
        }

        /// <summary>
        /// Elimina las colisiones de la generación actual.
        /// </summary>
        public void LimpiaColisiones()
        {
            foreach (Cromosoma cromosoma in LstCromosomas)
                cromosoma.Colisiones = 0;
        }

        /// <summary>
        /// Obtiene el fitness de un sólo individuo
        /// </summary>
        /// <param name="cromosoma"></param>
        /// <param name="lstTablero"></param>
        /// <returns></returns>
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

            return cromosoma.Colisiones;
        }


        /// <summary>
        /// Aplica crossover de dos individuos
        /// </summary>
        /// <param name="cromosoma1">Primer hijo.</param>
        /// <param name="cromosoma2">Segundo hijo.</param>
        /// <returns>Retorna una lista de los nuevos hijos generados.</returns>
        public List<Cromosoma> CrossOver(Cromosoma cromosoma1, Cromosoma cromosoma2)
        {
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            Cromosoma hijo1 = new Cromosoma();
            Cromosoma hijo2 = new Cromosoma();

            int interseccion = (cromosoma1.Genes.Count / 2) - 1;
            for (int i = 0; i < cromosoma1.Genes.Count; i++)
            {
                if (i <= interseccion)
                    hijo1.Genes.Add(cromosoma1.Genes[i]);
                else
                    hijo1.Genes.Add(cromosoma2.Genes[0]);
            }

            for (int i = hijo1.Genes.Count; i > 0; i--)
                hijo2.Genes.Add(hijo1.Genes[i - 1]);

            lstCromosomas.Add(hijo1);
            lstCromosomas.Add(hijo2);

            return lstCromosomas;
        }

        /// <summary>
        /// Obtiene los peores N individuos de la población actual.
        /// </summary>
        /// <param name="limit">N individuos</param>
        /// <returns>Lista con los mejores N individuos</returns>
        public List<Cromosoma> ObtenerLosMejores(int limit)
        {
            LstCromosomas.Sort();
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            for (int i = 0; i < limit; i++)
                lstCromosomas.Add(LstCromosomas[i]);
            return lstCromosomas;
        }

        /// <summary>
        /// Obtiene los mejores N individuos de la población actual.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<Cromosoma> ObtenerLosPeores(int limit)
        {
            LstCromosomas.Sort();
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            for (int i = (LstCromosomas.Count-1)-limit; i < LstCromosomas.Count; i++)
                lstCromosomas.Add(LstCromosomas[i]);
            return lstCromosomas;
        }

        /// <summary>
        /// Determina el nivel en el tablero en el que se encuentra la posición de la reina indicada con el parámetro posición.
        /// </summary>
        /// <param name="posicion">Posición de la reina a analizar.</param>
        /// <returns></returns>
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
