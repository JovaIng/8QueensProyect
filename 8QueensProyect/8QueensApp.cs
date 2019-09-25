using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _8QueensProyect
{
    public partial class Form1 : Form
    {
        Random random;
        List<Cromosoma> lstIndividuos = null;
        public Form1()
        {
            InitializeComponent();
            random = new Random();
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
            List<List<int>> lstTablero = new List<List<int>>();
            lstTablero = GeneraTablero(tamTablero);
            lstIndividuos = GeneraPoblacion(poblacionInicial, tamTablero);

            FitnessPopulationAnalizer fitnessAnalizer = new FitnessPopulationAnalizer(lstIndividuos, lstTablero);

            this.Cursor = Cursors.WaitCursor;
            while (numGeneraciones >= 0 && fitnessAnalizer.LstElite.Count.Equals(0))
            {
                numGeneraciones--;
                fitnessAnalizer.GenerarFitnessPoblacion();

                // de manera aleatoria elegimos 5 individuos de la población actual y obtenemos los mejores 2 de esa selección.
                List<Cromosoma> lstAux = fitnessAnalizer.ObtenerIndividuosAleatorios(5, random);
                List<Cromosoma> lstMejores = fitnessAnalizer.ObtenerLosMejores(2, lstAux);

                // los 2 mejores obtenidos los cruzamos y obtenemos dos hijos resultantes.
                List<Cromosoma> lstHijos = fitnessAnalizer.CrossOver(lstMejores[0], lstMejores[1], random);

                // mutamos uno de los hijos eligiendolo al azar.
                foreach(Cromosoma cromosoma in lstHijos)
                    fitnessAnalizer.Mutar(cromosoma, (tamTablero * tamTablero) - 1, random);

                // estos 2 hijos sustituirán a los dos peores de la población actual.
                List<Cromosoma> lstPeores = fitnessAnalizer.ObtenerLosPeores(2);
                fitnessAnalizer.SustituirPoblacion(lstPeores, lstHijos);
            }

            ActualizaInformacion(lstIndividuos);
            this.Cursor = Cursors.Default;
        }


        /// <summary>
        /// Actualiza el listado actual de cromosomas en pantalla
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

            //lstCromosomas.Sort();
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
        private List<Cromosoma> GeneraPoblacion(int poblacion, int longitud)
        {
            List<Cromosoma> lstIndividuos = new List<Cromosoma>();
            for (int i = 0; i < poblacion; i++)
            {
                Cromosoma cromosoma = new Cromosoma();
                cromosoma.Numero = i;
                cromosoma.Genes = GeneraGenesCromosoma(longitud);
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
        private List<int> GeneraGenesCromosoma(int longitud)
        {
            int min = 0;
            int max = longitud * longitud;

            List<int> genes = new List<int>();
            for (int i = 0; i < longitud; i++)
            {
                bool success = false;
                while (!success)
                {
                    int gen = random.Next(min, max);
                    if (!genes.Contains(gen))
                    {
                        genes.Add(gen);
                        success = true;
                    }
                }
            }

            return genes;
        }

        /// <summary>
        /// Genera el tablero de NxN según longitud.
        /// </summary>
        /// <param name="longitud">longitud del tablero cuadrado</param>
        /// <returns></returns>
        public List<List<int>> GeneraTablero(int longitud)
        {
            List<List<int>> lstNiveles = new List<List<int>>();
            int posiciones = longitud * longitud;

            int minAnterior = 0;
            for (int i = 0; i < longitud; i++)
            {
                int min = minAnterior;
                int max = min + (longitud - 1);
                lstNiveles.Add(GeneraNiveles(min, max));
                minAnterior = max + 1;
            }

            return lstNiveles;
        }

        /// <summary>
        /// Genera un solo nivel del tablero, los niveles son las filas, el llenado es con consecutivos de un valor mínimo a un valor máximo.
        /// </summary>
        /// <param name="inferior">valor mínimo</param>
        /// <param name="superior">valor máximo</param>
        /// <returns></returns>
        private List<int> GeneraNiveles(int inferior, int superior)
        {
            Random random = new Random();
            List<int> lstPosiciones = new List<int>();
            for (int i = inferior; i <= superior; i++)
                lstPosiciones.Add(i);

            return lstPosiciones;
        }

        public void EnabledControls(bool enabled)
        {
            txbGeneraciones.Enabled = enabled;
            txbTamTablero.Enabled = enabled;
            btnGenerar.Enabled = enabled;
        }
    }
}
