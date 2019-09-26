using System;
using System.Collections.Generic;

namespace _8QueensProyect
{
    public class FitnessPopulationAnalizer
    {
        public List<Cromosoma> LstCromosomas { get; set; }
        public int fitnessPoblacion { get; set; }
        public List<Cromosoma> LstElite { get; set; }

        public FitnessPopulationAnalizer(List<Cromosoma> lstIndividuos)
        {
            LstCromosomas = lstIndividuos;
            LstElite = new List<Cromosoma>();
        }

        /// <summary>
        /// Aplica fitness a todos los individuos de la población
        /// </summary>
        /// <returns></returns>
        public void GenerarFitnessPoblacion()
        {
            LimpiaColisiones();
            fitnessPoblacion = 0;
            foreach (Cromosoma cromosoma in LstCromosomas)
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
            this.fitnessPoblacion = 0;
            cromosoma.Colisiones = 0;

            for (int i = 0; i < cromosoma.Genes.Count; i++)
            {
                for (int j = 1; j < cromosoma.Genes.Count - i; j++)
                {
                    int aux = i + j;
                    if (aux < cromosoma.Genes.Count)
                    {
                        if (cromosoma.Genes[i].Equals(cromosoma.Genes[i + j] + j))
                            cromosoma.Colisiones++;
                        if (cromosoma.Genes[i].Equals(cromosoma.Genes[i + j] - j))
                            cromosoma.Colisiones++;
                    }

                    aux = i - j;
                    if (aux > 0)
                    {
                        if (cromosoma.Genes[i].Equals(cromosoma.Genes[i - j] - j))
                            cromosoma.Colisiones++;
                        if (cromosoma.Genes[i].Equals(cromosoma.Genes[i - j] + j))
                            cromosoma.Colisiones++;
                    }
                }
            }

            double peorCaso = cromosoma.Genes.Count * (cromosoma.Genes.Count - 1);
            double penalizacion = cromosoma.Colisiones / peorCaso;

            cromosoma.Fitness = 1 - penalizacion;
            return cromosoma.Colisiones;
        }

        internal void Mutar(Cromosoma cromosoma, int valorMax, Random random)
        {
            bool success = false;
            for (int i = 0; i < cromosoma.Genes.Count; i++)
            {
                if (GetFitnessGen(cromosoma, cromosoma.Genes[i]) > 0)
                {
                    while (!success)
                    {
                        int gen = random.Next(0, valorMax);
                        if (!cromosoma.Genes.Contains(gen))
                        {
                            if (GetFitnessGen(cromosoma, gen).Equals(0))
                            {
                                cromosoma.Genes[i] = gen;
                                success = true;
                            }
                        }
                    }
                }

                if (success)
                    break;
            }
        }

        public int GetFitnessGen(Cromosoma cromosoma, int gen)
        {
            int colisiones = 0;
            for (int j = 1; j < cromosoma.Genes.Count - gen; j++)
            {
                int aux = gen + j;
                if (aux < cromosoma.Genes.Count)
                {
                    if (cromosoma.Genes[gen].Equals(cromosoma.Genes[gen + j] + j))
                        cromosoma.Colisiones++;
                    if (cromosoma.Genes[gen].Equals(cromosoma.Genes[gen + j] - j))
                        cromosoma.Colisiones++;
                }

                aux = gen - j;
                if (aux > 0)
                {
                    if (cromosoma.Genes[gen].Equals(cromosoma.Genes[gen - j] - j))
                        cromosoma.Colisiones++;
                    if (cromosoma.Genes[gen].Equals(cromosoma.Genes[gen - j] + j))
                        cromosoma.Colisiones++;
                }
            }
            return colisiones;
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

            int interseccion = (cromosoma1.Genes.Count - 1) / 2;
            for (int i = 0; i < cromosoma1.Genes.Count; i++)
            {
                bool success = false;
                if (i <= interseccion)
                    hijo1.Genes.Add(cromosoma1.Genes[i]);
                else
                {
                    if (!hijo1.Genes.Contains(cromosoma2.Genes[i]))
                        hijo1.Genes.Add(cromosoma2.Genes[i]);
                    else
                    {
                        while (!success)
                        {
                            int gen = random.Next(0, cromosoma1.Genes.Count - 1);
                            if (success = !hijo1.Genes.Contains(gen))
                                hijo1.Genes.Add(gen);
                            else
                            {
                                gen = gen + 1;
                                if (success = (gen <= cromosoma1.Genes.Count - 1 && !hijo1.Genes.Contains(gen)))
                                    hijo1.Genes.Add(gen);
                                else
                                {
                                    gen = gen - 2;
                                    if (success = (gen >= 0 && !hijo1.Genes.Contains(gen)))
                                        hijo1.Genes.Add(gen);
                                }
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
    }
}
