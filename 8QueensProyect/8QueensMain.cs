using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace _8QueensProyect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            EnabledControls(false);
            if (ValidaControles(txbGeneraciones.Text, txbTamTablero.Text, tbxPoblacionInicial.Text))
                GeneraSolucion(Convert.ToInt32(txbGeneraciones.Text), Convert.ToInt32(txbTamTablero.Text), Convert.ToInt32(tbxPoblacionInicial.Text));
            EnabledControls(true);
        }

        private bool ValidaControles(string generaciones, string tableroSize, string poblacion)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(txbGeneraciones.Text) && !string.IsNullOrEmpty(txbTamTablero.Text) && !string.IsNullOrEmpty(tbxPoblacionInicial.Text))
            {
                int maxLength = int.MaxValue.ToString().Length;
                if (txbGeneraciones.Text.Length <= maxLength && txbTamTablero.Text.Length <= maxLength && tbxPoblacionInicial.Text.Length <= maxLength)
                {
                    if (Convert.ToInt32(generaciones) >= 0 && Convert.ToInt32(tableroSize) >= 4 && Convert.ToInt32(poblacion) >= 5)
                    {
                        if (Convert.ToInt32(tableroSize) % 2 == 0)
                            success = true;
                    }
                }
            }

            return success;
        }

        private void GeneraSolucion(int numGeneraciones, int tamTablero, int poblacionInicial)
        {
            bool success = false;
            Random random = new Random();
            List<Cromosoma> lstIndividuos = GeneraPoblacion(poblacionInicial, tamTablero, random);
            FitnessPopulationAnalizer fitnessAnalizer = new FitnessPopulationAnalizer(lstIndividuos);

            this.Cursor = Cursors.WaitCursor;
            int generaciones = 0;
            List<int> lstGeneraciones = new List<int>();
            List<float> lstFitness = new List<float>();
            while (generaciones < numGeneraciones && fitnessAnalizer.LstElite.Count.Equals(0))
            {
                generaciones++;
                fitnessAnalizer.GenerarFitnessPoblacion();
                lstFitness.Add(fitnessAnalizer.fitnessPoblacion);
                lstGeneraciones.Add(generaciones);
                // de manera aleatoria elegimos 5 individuos de la población actual y obtenemos los mejores 2 de esa selección.
                List<Cromosoma> lstAux = fitnessAnalizer.ObtenerIndividuosAleatorios(5, random);
                List<Cromosoma> lstMejores = fitnessAnalizer.ObtenerLosMejores(2, lstAux);

                // los 2 mejores obtenidos los cruzamos y obtenemos dos hijos resultantes.
                List<Cromosoma> lstHijos = fitnessAnalizer.CrossOver(lstMejores[0], lstMejores[1], random);

                // mutamos a los hijos si no son la solución.
                foreach (Cromosoma cromosoma in lstHijos)
                {
                    if (fitnessAnalizer.ObtenerFitnessCromosoma(cromosoma) != 0)
                        fitnessAnalizer.Mutar(cromosoma, cromosoma.Genes.Count - 1, random);
                }

                // estos 2 hijos sustituirán a los dos peores de la población actual.
                List<Cromosoma> lstPeores = fitnessAnalizer.ObtenerLosPeores(2);
                fitnessAnalizer.SustituirPoblacion(lstPeores, lstHijos);
            }

            ActualizaInformacion(lstIndividuos);
            GenerarArchivo(fitnessAnalizer);
            this.Cursor = Cursors.Default;
        }

        private void GenerarArchivo(FitnessPopulationAnalizer fitnessPopulationAnalizer)
        {
            string pathGeneraciones = AppDomain.CurrentDomain.BaseDirectory + "generaciones.txt";
            string pathFitness = AppDomain.CurrentDomain.BaseDirectory + "fitness.txt";

            if (File.Exists(pathGeneraciones))
                File.Delete(pathGeneraciones);

            if (File.Exists(pathFitness))
                File.Delete(pathFitness);

            try
            {
                StreamWriter sw = new StreamWriter(pathGeneraciones);
                sw.WriteLine("Generación");
                for (int i = 0; i < fitnessPopulationAnalizer.LstCromosomas.Count; i++)
                    sw.WriteLine(i + 1);
                sw.Close();

                sw = new StreamWriter(pathFitness);
                sw.WriteLine("Fitness");
                for (int i = fitnessPopulationAnalizer.LstCromosomas.Count - 1; i >= 0; i--)
                    sw.WriteLine(fitnessPopulationAnalizer.LstCromosomas[i].Fitness);
                sw.Close();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Actualiza el listado actual de cromosomas y la gráfica de fitness por generación.
        /// </summary>
        /// <param name="lstCromosomas">lista completa de la población actual</param>
        private void ActualizaInformacion(List<Cromosoma> lstCromosomas)
        {
            dgvIndividuos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            if (dgvIndividuos.Rows.Count > 0)
                dgvIndividuos.Rows.Clear();

            if (dgvIndividuos.Columns.Count == 0)
            {
                dgvIndividuos.Columns.Add("cromosomas", "Cromosomas");
                dgvIndividuos.Columns.Add("colisiones", "Colisiones");
                dgvIndividuos.Columns.Add("fitness", "Fitness");
            }

            for (int i = 0; i < lstCromosomas.Count; i++)
            {
                string genes = string.Empty;
                foreach (int gen in lstCromosomas[i].Genes)
                    genes += string.Format(" {0}", gen.ToString("00"));

                dgvIndividuos.Rows.Add(genes.Trim(), lstCromosomas[i].Colisiones, lstCromosomas[i].Fitness);
            }


        }

        /// <summary>
        /// Genera población inicial
        /// </summary>
        /// <param name="poblacion">número de individuos a generar</param>
        /// <returns></returns>
        private List<Cromosoma> GeneraPoblacion(int poblacion, int longitud, Random random)
        {
            List<Cromosoma> lstIndividuos = new List<Cromosoma>();
            for (int i = 0; i < poblacion; i++)
            {
                Cromosoma cromosoma = new Cromosoma();
                cromosoma.Numero = i;
                cromosoma.Genes = GeneraGenesCromosoma(longitud, random);
                lstIndividuos.Add(cromosoma);
            }

            return lstIndividuos;
        }

        /// <summary>
        /// Genera listado de genes de un cromosoma.
        /// </summary>
        /// <param name="random">clase generadora de números random</param>
        /// <param name="min">valor mínimo</param>
        /// <param name="max">valor máximo</param>
        /// <returns></returns>
        private List<int> GeneraGenesCromosoma(int longitud, Random random)
        {
            List<int> genes = new List<int>();
            for (int i = 0; i < longitud; i++)
            {
                bool success = false;
                while (!success)
                {
                    int gen = random.Next(0, longitud - 1);
                    if (success = !genes.Contains(gen))
                        genes.Add(gen);
                    else
                    {
                        int aux = gen + 1;
                        if (success = (gen <= longitud && !genes.Contains(aux)))
                            genes.Add(aux);
                        else
                        {
                            aux = aux - 2;
                            if (aux >= 0)
                            {
                                if (success = !genes.Contains(aux))
                                    genes.Add(aux);
                            }
                        }
                    }
                }
            }

            return genes;
        }

        public void EnabledControls(bool enabled)
        {
            txbGeneraciones.Enabled = enabled;
            txbTamTablero.Enabled = enabled;
            btnGenerar.Enabled = enabled;
        }
    }
}

