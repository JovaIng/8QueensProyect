using System;
using System.Collections.Generic;

namespace _8QueensProyect
{
    public class FitnessPopulationAnalizer
    {
        public List<Cromosoma> LstCromosomas { get; set; }
        public int colisionesPoblacion { get; set; }
        public int fitnessPoblacion { get; set; }
        public List<List<int>> LstTablero { get; set; }
        public List<Cromosoma> LstElite { get; set; }

        public FitnessPopulationAnalizer(List<Cromosoma> lstIndividuos, List<List<int>> lstTablero)
        {
            LstCromosomas = lstIndividuos;
            LstTablero = lstTablero;
            LstElite = new List<Cromosoma>();
        }

        /// <summary>
        /// Aplica fitness a todos los individuos de la población
        /// </summary>
        /// <returns></returns>
        public void GenerarFitnessPoblacion()
        {
            LimpiaColisiones();
            foreach(Cromosoma cromosoma in LstCromosomas)
            {
                ObtenerFitnessCromosoma(cromosoma);
                if (cromosoma.Colisiones.Equals(0))
                    LstElite.Add(cromosoma);
            }
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
        /// <param name="cromosoma">cromosoma  a analizar</param>
        /// <param name="lstTablero">tablero actual</param>
        /// <returns></returns>
        public int ObtenerFitnessCromosoma(Cromosoma cromosoma)
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

            this.colisionesPoblacion = 0;
            this.fitnessPoblacion = 0;
            cromosoma.Colisiones = 0;

            List<int> lstNiveles = new List<int>();
            for (int i = 0; i < cromosoma.Genes.Count; i++)
            {
                //Analiza fila.
                int gen = cromosoma.Genes[i];
                int nivel = ObtieneNivel(gen);
                for(int j = 0; j < cromosoma.Genes.Count; j++)
                {
                    if (i != j)
                    {
                        int genAux = cromosoma.Genes[j];
                        if (nivel.Equals(ObtieneNivel(genAux)))
                            cromosoma.Colisiones++;
                    }
                }

                // analizar columna.
                int index = LstTablero[nivel].IndexOf(gen);
                for (int j = 0; j < LstTablero.Count; j++)
                {
                    List<int> genes = LstTablero[j];
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

        internal void Mutar(Cromosoma cromosoma, int valorMax, Random random)
        {
            foreach(int gen in cromosoma.Genes)
            {
                int colisionesGen = ObtenerFitnessGen(cromosoma, gen);
                if (!colisionesGen.Equals(0))
                {
                    int newGen = 0;
                    bool success = false;
                    while (!success)
                    {
                        newGen = random.Next(0, valorMax);
                        if (ObtenerFitnessGen(cromosoma, newGen) < colisionesGen)
                            success = true;
                    }

                    cromosoma.Genes[cromosoma.Genes.IndexOf(gen)] = newGen;
                    break;
                }
            }
        }

        public int ObtenerFitnessGen(Cromosoma cromosoma, int gen)
        {
            //Analiza fila.
            int colisionesGen = 0;
            int nivel = ObtieneNivel(gen);
            for (int j = 0; j < cromosoma.Genes.Count; j++)
            {
                int genAux = cromosoma.Genes[j];
                if (nivel.Equals(ObtieneNivel(genAux)))
                    colisionesGen++;
            }

            // analizar columna.
            int index = LstTablero[nivel].IndexOf(gen);
            for (int j = 0; j < LstTablero.Count; j++)
            {
                List<int> genes = LstTablero[j];
                int genAux = genes[index];
                if (genAux != gen)
                {
                    if (cromosoma.Genes.Contains(genAux))
                        colisionesGen++;
                }
            }

            // analizar diagonal.

            return colisionesGen;
        }

        /// <summary>
        /// Aplica crossover de dos individuos
        /// </summary>
        /// <param name="cromosoma1">primer hijo.</param>
        /// <param name="cromosoma2">segundo hijo.</param>
        /// <returns>retorna una lista de los nuevos hijos generados.</returns>
        public List<Cromosoma> CrossOver(Cromosoma cromosoma1, Cromosoma cromosoma2, Random random)
        {
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            Cromosoma hijo1 = new Cromosoma();
            Cromosoma hijo2 = new Cromosoma();

            int interseccion = (cromosoma1.Genes.Count / 2) - 1;
            for (int i = 0; i < cromosoma1.Genes.Count; i++)
            {
                bool success = false;
                if (i <= interseccion)
                {
                    if (!hijo1.Genes.Contains(cromosoma1.Genes[i]))
                        hijo1.Genes.Add(cromosoma1.Genes[i]);
                    else
                    {
                        while (!success)
                        {
                            int gen = random.Next(0, (LstTablero.Count * LstTablero.Count) - 1);
                            if (!hijo1.Genes.Contains(gen))
                            {
                                hijo1.Genes.Add(gen);
                                success = true;
                            }
                        }
                    }
                }
                else
                {
                    if (!hijo1.Genes.Contains(cromosoma2.Genes[i]))
                        hijo1.Genes.Add(cromosoma2.Genes[i]);
                    else
                    {
                        while (!success)
                        {
                            int gen = random.Next(0, (LstTablero.Count * LstTablero.Count) - 1);
                            if (!hijo1.Genes.Contains(gen))
                            {
                                hijo1.Genes.Add(gen);
                                success = true;
                            }
                        }
                    }
                }
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
        /// <param name="limit">n individuos</param>
        /// <returns>lista con los mejores N individuos</returns>
        public List<Cromosoma> ObtenerLosMejores(int limit)
        {
            LstCromosomas.Sort();
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            for (int i = 0; i < limit; i++)
                lstCromosomas.Add(LstCromosomas[i]);
            return lstCromosomas;
        }

        /// <summary>
        /// Obtiene los mejores N individuos de una lista dada.
        /// </summary>
        /// <param name="limit">Número de individuos límite a obtener</param>
        /// <returns>lista de individuos</returns>
        public List<Cromosoma> ObtenerLosMejores(int limit, List<Cromosoma> lstCromosomas)
        {
            List<Cromosoma> lstResult = new List<Cromosoma>();
            if (lstCromosomas != null && limit <= lstCromosomas.Count)
            {
                lstCromosomas.Sort();
                for (int i = 0; i < limit; i++)
                    lstResult.Add(lstCromosomas[i]);
            }
            
            return lstResult;
        }

        /// <summary>
        /// Obtiene N cantidad de individuos de la población de forma aleatoria 
        /// </summary>
        /// <param name="cantidad">cantidad de individuos que necesite obtener</param>
        /// <param name="random">clase generadora de números aleatorios</param>
        /// <returns></returns>
        public List<Cromosoma> ObtenerIndividuosAleatorios(int cantidad, Random random)
        {
            LstCromosomas.Sort();
            List<int> lstPosiciones = new List<int>();
            List<Cromosoma> lstCromosomas = new List<Cromosoma>();
            for (int i = 0; i < cantidad; i++)
            {
                bool success = false;
                while(!success)
                {
                    int index = random.Next(0, LstCromosomas.Count - 1);
                    if (!lstPosiciones.Contains(index))
                    {
                        success = true;
                        lstPosiciones.Add(index);
                        lstCromosomas.Add(LstCromosomas[index]);
                    }
                }
            }

            return lstCromosomas;
        }

        /// <summary>
        /// Obtiene los mejores N individuos de la población actual.
        /// </summary>
        /// <param name="limit">n individuos</param>
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
        /// Obtiene los mejores N individuos dado una lista determinada.
        /// </summary>
        /// <param name="limit">n individuos</param>
        /// <returns></returns>
        public List<Cromosoma> ObtenerLosPeores(int limit, List<Cromosoma> lstCromosomas)
        {
            List<Cromosoma> lstResult = new List<Cromosoma>();
            if (limit <= lstCromosomas.Count)
            {
                lstCromosomas.Sort();
                for (int i = (lstCromosomas.Count - 1) - limit; i < lstCromosomas.Count; i++)
                    lstResult.Add(lstCromosomas[i]);
            }
           
            return lstResult;
        }

        /// <summary>
        /// Encuentra todos los individuos de la lista 1 que se encuentren en la población actual y los sustituye por la lista 2.
        /// </summary>
        /// <param name="lst1">lista auxiliar para buscar en la población</param>
        /// <param name="lst2">lista que sustituirá los individuos de la lista 1 que se encuentren en la población actual</param>
        public void SustituirPoblacion(List<Cromosoma> lst1, List<Cromosoma> lst2)
        {
            if (lst1 != null && lst2 != null && lst1.Count.Equals(lst2.Count))
            {
                foreach (Cromosoma cromosoma in lst1)
                {
                    int index = LstCromosomas.IndexOf(cromosoma);
                    if (index >= 0)
                    {
                        LstCromosomas[index] = lst2[0];
                        lst2.RemoveAt(0);
                    }
                }
            }
        }

        /// <summary>
        /// Determina el nivel en el tablero en el que se encuentra la posición de la reina indicada con el parámetro posición.
        /// </summary>
        /// <param name="posicion">posición de la reina a analizar.</param>
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
