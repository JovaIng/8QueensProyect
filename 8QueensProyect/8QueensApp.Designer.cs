namespace _8QueensProyect
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.dgvIndividuos = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txbGeneraciones = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbTamTablero = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chFitness = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxPoblacionInicial = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndividuos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFitness)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(12, 634);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(270, 49);
            this.btnGenerar.TabIndex = 0;
            this.btnGenerar.Text = "Generar solución";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.BtnGenerar_Click);
            // 
            // dgvIndividuos
            // 
            this.dgvIndividuos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIndividuos.Location = new System.Drawing.Point(12, 36);
            this.dgvIndividuos.Name = "dgvIndividuos";
            this.dgvIndividuos.Size = new System.Drawing.Size(270, 439);
            this.dgvIndividuos.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = " Listado de individuos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 540);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Máximo de generaciones: ";
            // 
            // txbGeneraciones
            // 
            this.txbGeneraciones.Location = new System.Drawing.Point(149, 537);
            this.txbGeneraciones.Name = "txbGeneraciones";
            this.txbGeneraciones.Size = new System.Drawing.Size(133, 20);
            this.txbGeneraciones.TabIndex = 4;
            this.txbGeneraciones.Text = "1000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 582);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tamaño del tablero: ";
            // 
            // txbTamTablero
            // 
            this.txbTamTablero.Location = new System.Drawing.Point(149, 579);
            this.txbTamTablero.Name = "txbTamTablero";
            this.txbTamTablero.Size = new System.Drawing.Size(133, 20);
            this.txbTamTablero.TabIndex = 6;
            this.txbTamTablero.Text = "8";
            // 
            // chFitness
            // 
            chartArea1.Name = "ChartArea1";
            this.chFitness.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chFitness.Legends.Add(legend1);
            this.chFitness.Location = new System.Drawing.Point(308, 36);
            this.chFitness.Name = "chFitness";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chFitness.Series.Add(series1);
            this.chFitness.Size = new System.Drawing.Size(1030, 647);
            this.chFitness.TabIndex = 7;
            this.chFitness.Text = "Fitness";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 501);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Población inicial:";
            // 
            // tbxPoblacionInicial
            // 
            this.tbxPoblacionInicial.Location = new System.Drawing.Point(149, 498);
            this.tbxPoblacionInicial.Name = "tbxPoblacionInicial";
            this.tbxPoblacionInicial.Size = new System.Drawing.Size(133, 20);
            this.tbxPoblacionInicial.TabIndex = 9;
            this.tbxPoblacionInicial.Text = "100";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.tbxPoblacionInicial);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chFitness);
            this.Controls.Add(this.txbTamTablero);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbGeneraciones);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvIndividuos);
            this.Controls.Add(this.btnGenerar);
            this.Name = "Form1";
            this.Text = "8 Queens";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndividuos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFitness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.DataGridView dgvIndividuos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbGeneraciones;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbTamTablero;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chFitness;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxPoblacionInicial;
    }
}

