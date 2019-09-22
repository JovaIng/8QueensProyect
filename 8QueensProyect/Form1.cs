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
        int count;
        public Form1()
        {
            count = 0;
            InitializeComponent();
        }


        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            int longitud = 8;
            List<List<int>> lstTablero = new List<List<int>>();
            lstTablero = GeneraTablero(longitud);
            int generacion = 1;
            List<Cromosoma> lstIndividuos = GeneraPoblacion(100);
            ActualizaInformacion(lstIndividuos);

            for (int i = 0; i < 100; i++)
            {
                FitnessAnalizer fitnessAnalizer = new FitnessAnalizer(lstIndividuos, lstTablero);
                if (fitnessAnalizer.ValidarFitness())
                    break;
                else
                {
                    CrossOver(fitnessAnalizer, longitud);
                }

                ActualizaInformacion(fitnessAnalizer.LstCromosomas);
            }
        }

        private void CrossOver(FitnessAnalizer fitnessAnalizer, int longitud)
        {
            List<Cromosoma> lstPadres = fitnessAnalizer.ObtenerLosMejores(2);
            Cromosoma padre1 = lstPadres[0];
            Cromosoma padre2 = lstPadres[1];

            Cromosoma hijo1 = new Cromosoma();
            Cromosoma hijo2 = new Cromosoma();
            int puntoCorte = longitud / 2;

            int i;
            for (i = 0; i < puntoCorte; i++)
                hijo1.Genes.Add(padre1.Genes[i]);
            for (i = puntoCorte; i < longitud; i++)
                hijo1.Genes.Add(padre2.Genes[i]);

            for (i = 0; i < puntoCorte; i++)
                hijo2.Genes.Add(padre2.Genes[i]);
            for (i = puntoCorte; i < longitud; i++)
                hijo2.Genes.Add(padre1.Genes[i]);

            fitnessAnalizer.LstCromosomas[fitnessAnalizer.LstCromosomas.Count - 2] = hijo1;
            fitnessAnalizer.LstCromosomas[fitnessAnalizer.LstCromosomas.Count - 1] = hijo2;
        }

        private void ActualizaInformacion(List<Cromosoma> lstCromosomas)
        {
            dgvIndividuos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            if (dgvIndividuos.Rows.Count > 0)
                dgvIndividuos.Rows.Clear();

            if (dgvIndividuos.Columns.Count == 0)
            {
                dgvIndividuos.Columns.Add("cromosomas", "Cromosomas");
                dgvIndividuos.Columns.Add("colisiones", "Colisiones");
            }

            lstCromosomas.Sort();
            for (int i = 0; i < lstCromosomas.Count; i++)
            {
                string genes = string.Empty;
                foreach (int gen in lstCromosomas[i].Genes)
                    genes += string.Format(" {0}", gen.ToString("00"));

                dgvIndividuos.Rows.Add(genes.Trim(), lstCromosomas[i].Colisiones);
            }
        }

        private List<Cromosoma> GeneraPoblacion(int poblacion)
        {
            List<Cromosoma> lstIndividuos = new List<Cromosoma>();
            Random random = new Random();
            for (int i = 0; i < poblacion; i++)
            {
                Cromosoma cromosoma = new Cromosoma();
                cromosoma.Numero = i;
                cromosoma.Genes = GeneraGenesCromosoma(random);
                lstIndividuos.Add(cromosoma);
            }

            return lstIndividuos;
        }

        private List<int> GeneraGenesCromosoma(Random random)
        {
            List<int> cromosoma = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                bool success = false;
                while (!success)
                {
                    int gen = random.Next(0, 63);
                    if (!cromosoma.Contains(gen))
                    {
                        cromosoma.Add(gen);
                        success = true;
                    }
                }
            }

            return cromosoma;
        }

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

        private List<int> GeneraNiveles(int inferior, int superior)
        {
            Random random = new Random();
            List<int> lstPosiciones = new List<int>();
            for (int i = inferior; i <= superior; i++)
                lstPosiciones.Add(i);

            return lstPosiciones;
        }

    }
}
