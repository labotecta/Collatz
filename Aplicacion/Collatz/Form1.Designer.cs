namespace Collatz
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.numero = new System.Windows.Forms.TextBox();
            this.paso = new System.Windows.Forms.Button();
            this.lista = new System.Windows.Forms.ListBox();
            this.borra = new System.Windows.Forms.Button();
            this.listab = new System.Windows.Forms.ListBox();
            this.salva = new System.Windows.Forms.Button();
            this.original = new System.Windows.Forms.RadioButton();
            this.nuevo = new System.Windows.Forms.RadioButton();
            this.todo = new System.Windows.Forms.Button();
            this.listabc = new System.Windows.Forms.ListBox();
            this.listac = new System.Windows.Forms.ListBox();
            this.busca = new System.Windows.Forms.Button();
            this.b_cancelar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.borraycalcula = new System.Windows.Forms.Button();
            this.recopila_impares = new System.Windows.Forms.Button();
            this.gen_11 = new System.Windows.Forms.Button();
            this.terminaciones = new System.Windows.Forms.Button();
            this.n_hilos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.prec = new System.Windows.Forms.Button();
            this.anticipos = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // numero
            // 
            this.numero.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numero.Location = new System.Drawing.Point(12, 12);
            this.numero.Name = "numero";
            this.numero.Size = new System.Drawing.Size(127, 28);
            this.numero.TabIndex = 1;
            this.numero.TextChanged += new System.EventHandler(this.Numero_TextChanged);
            // 
            // paso
            // 
            this.paso.Location = new System.Drawing.Point(12, 120);
            this.paso.Name = "paso";
            this.paso.Size = new System.Drawing.Size(127, 32);
            this.paso.TabIndex = 2;
            this.paso.Text = "Paso";
            this.paso.UseVisualStyleBackColor = true;
            this.paso.Click += new System.EventHandler(this.Paso_Click);
            // 
            // lista
            // 
            this.lista.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lista.FormattingEnabled = true;
            this.lista.ItemHeight = 20;
            this.lista.Location = new System.Drawing.Point(12, 159);
            this.lista.Name = "lista";
            this.lista.Size = new System.Drawing.Size(146, 204);
            this.lista.TabIndex = 3;
            this.lista.SelectedIndexChanged += new System.EventHandler(this.Lista_SelectedIndexChanged);
            this.lista.DoubleClick += new System.EventHandler(this.Lista_DoubleClick);
            // 
            // borra
            // 
            this.borra.ForeColor = System.Drawing.Color.Red;
            this.borra.Location = new System.Drawing.Point(12, 712);
            this.borra.Name = "borra";
            this.borra.Size = new System.Drawing.Size(127, 32);
            this.borra.TabIndex = 4;
            this.borra.Text = "Borra";
            this.borra.UseVisualStyleBackColor = true;
            this.borra.Click += new System.EventHandler(this.Borra_Click);
            // 
            // listab
            // 
            this.listab.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listab.FormattingEnabled = true;
            this.listab.ItemHeight = 20;
            this.listab.Location = new System.Drawing.Point(164, 159);
            this.listab.Name = "listab";
            this.listab.Size = new System.Drawing.Size(1730, 204);
            this.listab.TabIndex = 5;
            this.listab.SelectedIndexChanged += new System.EventHandler(this.Listab_SelectedIndexChanged);
            // 
            // salva
            // 
            this.salva.ForeColor = System.Drawing.Color.Green;
            this.salva.Location = new System.Drawing.Point(1767, 712);
            this.salva.Name = "salva";
            this.salva.Size = new System.Drawing.Size(127, 32);
            this.salva.TabIndex = 6;
            this.salva.Text = "Salva";
            this.salva.UseVisualStyleBackColor = true;
            this.salva.Click += new System.EventHandler(this.Salva_Click);
            // 
            // original
            // 
            this.original.AutoSize = true;
            this.original.Checked = true;
            this.original.Location = new System.Drawing.Point(12, 50);
            this.original.Name = "original";
            this.original.Size = new System.Drawing.Size(82, 21);
            this.original.TabIndex = 7;
            this.original.TabStop = true;
            this.original.Text = "3 * n + 1";
            this.original.UseVisualStyleBackColor = true;
            // 
            // nuevo
            // 
            this.nuevo.AutoSize = true;
            this.nuevo.Location = new System.Drawing.Point(12, 82);
            this.nuevo.Name = "nuevo";
            this.nuevo.Size = new System.Drawing.Size(61, 21);
            this.nuevo.TabIndex = 8;
            this.nuevo.Text = "n + 1";
            this.nuevo.UseVisualStyleBackColor = true;
            // 
            // todo
            // 
            this.todo.Location = new System.Drawing.Point(861, 712);
            this.todo.Name = "todo";
            this.todo.Size = new System.Drawing.Size(259, 32);
            this.todo.TabIndex = 9;
            this.todo.Text = "Calcula Todo";
            this.todo.UseVisualStyleBackColor = true;
            this.todo.Click += new System.EventHandler(this.Todo_Click);
            // 
            // listabc
            // 
            this.listabc.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listabc.FormattingEnabled = true;
            this.listabc.ItemHeight = 20;
            this.listabc.Location = new System.Drawing.Point(164, 399);
            this.listabc.Name = "listabc";
            this.listabc.Size = new System.Drawing.Size(1730, 304);
            this.listabc.TabIndex = 11;
            this.listabc.SelectedIndexChanged += new System.EventHandler(this.Listabc_SelectedIndexChanged);
            // 
            // listac
            // 
            this.listac.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listac.FormattingEnabled = true;
            this.listac.ItemHeight = 20;
            this.listac.Location = new System.Drawing.Point(12, 399);
            this.listac.Name = "listac";
            this.listac.Size = new System.Drawing.Size(146, 304);
            this.listac.TabIndex = 10;
            this.listac.SelectedIndexChanged += new System.EventHandler(this.Listac_SelectedIndexChanged);
            this.listac.DoubleClick += new System.EventHandler(this.Listac_DoubleClick);
            // 
            // busca
            // 
            this.busca.Location = new System.Drawing.Point(364, 71);
            this.busca.Name = "busca";
            this.busca.Size = new System.Drawing.Size(72, 32);
            this.busca.TabIndex = 12;
            this.busca.Text = "Busca";
            this.busca.UseVisualStyleBackColor = true;
            this.busca.Click += new System.EventHandler(this.Busca_Click);
            // 
            // b_cancelar
            // 
            this.b_cancelar.Location = new System.Drawing.Point(299, 70);
            this.b_cancelar.Name = "b_cancelar";
            this.b_cancelar.Size = new System.Drawing.Size(59, 32);
            this.b_cancelar.TabIndex = 13;
            this.b_cancelar.Text = "Esc";
            this.b_cancelar.UseVisualStyleBackColor = true;
            this.b_cancelar.Click += new System.EventHandler(this.Cancela_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 371);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 18);
            this.label1.TabIndex = 14;
            this.label1.Text = "Impares";
            // 
            // borraycalcula
            // 
            this.borraycalcula.Location = new System.Drawing.Point(164, 71);
            this.borraycalcula.Name = "borraycalcula";
            this.borraycalcula.Size = new System.Drawing.Size(129, 32);
            this.borraycalcula.TabIndex = 15;
            this.borraycalcula.Text = "Borra y Calcula";
            this.borraycalcula.UseVisualStyleBackColor = true;
            this.borraycalcula.Click += new System.EventHandler(this.Borraycalcula_Click);
            // 
            // recopila_impares
            // 
            this.recopila_impares.Location = new System.Drawing.Point(442, 71);
            this.recopila_impares.Name = "recopila_impares";
            this.recopila_impares.Size = new System.Drawing.Size(114, 32);
            this.recopila_impares.TabIndex = 16;
            this.recopila_impares.Text = "Tabla Impares";
            this.recopila_impares.UseVisualStyleBackColor = true;
            this.recopila_impares.Click += new System.EventHandler(this.Tabla_impares_Click);
            // 
            // gen_11
            // 
            this.gen_11.Location = new System.Drawing.Point(801, 67);
            this.gen_11.Name = "gen_11";
            this.gen_11.Size = new System.Drawing.Size(136, 32);
            this.gen_11.TabIndex = 17;
            this.gen_11.Text = "Secuencias ...11";
            this.gen_11.UseVisualStyleBackColor = true;
            this.gen_11.Click += new System.EventHandler(this.Gen_11_Click);
            // 
            // terminaciones
            // 
            this.terminaciones.Location = new System.Drawing.Point(679, 67);
            this.terminaciones.Name = "terminaciones";
            this.terminaciones.Size = new System.Drawing.Size(116, 32);
            this.terminaciones.TabIndex = 18;
            this.terminaciones.Text = "Terminaciones";
            this.terminaciones.UseVisualStyleBackColor = true;
            this.terminaciones.Click += new System.EventHandler(this.Terminaciones_Click);
            // 
            // n_hilos
            // 
            this.n_hilos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.n_hilos.Location = new System.Drawing.Point(632, 71);
            this.n_hilos.Name = "n_hilos";
            this.n_hilos.Size = new System.Drawing.Size(41, 28);
            this.n_hilos.TabIndex = 19;
            this.n_hilos.Text = "36";
            this.n_hilos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(562, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 24);
            this.label2.TabIndex = 20;
            this.label2.Text = "Hilos";
            // 
            // prec
            // 
            this.prec.Location = new System.Drawing.Point(943, 67);
            this.prec.Name = "prec";
            this.prec.Size = new System.Drawing.Size(67, 32);
            this.prec.TabIndex = 21;
            this.prec.Text = "Pre C";
            this.prec.UseVisualStyleBackColor = true;
            this.prec.Click += new System.EventHandler(this.Prec_Click);
            // 
            // anticipos
            // 
            this.anticipos.Location = new System.Drawing.Point(1016, 67);
            this.anticipos.Name = "anticipos";
            this.anticipos.Size = new System.Drawing.Size(87, 32);
            this.anticipos.TabIndex = 22;
            this.anticipos.Text = "Anticipos";
            this.anticipos.UseVisualStyleBackColor = true;
            this.anticipos.Click += new System.EventHandler(this.Anticipos_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 753);
            this.Controls.Add(this.anticipos);
            this.Controls.Add(this.prec);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.n_hilos);
            this.Controls.Add(this.terminaciones);
            this.Controls.Add(this.gen_11);
            this.Controls.Add(this.recopila_impares);
            this.Controls.Add(this.borraycalcula);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_cancelar);
            this.Controls.Add(this.busca);
            this.Controls.Add(this.listabc);
            this.Controls.Add(this.listac);
            this.Controls.Add(this.todo);
            this.Controls.Add(this.nuevo);
            this.Controls.Add(this.original);
            this.Controls.Add(this.salva);
            this.Controls.Add(this.listab);
            this.Controls.Add(this.borra);
            this.Controls.Add(this.lista);
            this.Controls.Add(this.paso);
            this.Controls.Add(this.numero);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collatz 3n+1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox numero;
        private System.Windows.Forms.Button paso;
        private System.Windows.Forms.ListBox lista;
        private System.Windows.Forms.Button borra;
        private System.Windows.Forms.ListBox listab;
        private System.Windows.Forms.Button salva;
        private System.Windows.Forms.RadioButton original;
        private System.Windows.Forms.RadioButton nuevo;
        private System.Windows.Forms.Button todo;
        private System.Windows.Forms.ListBox listabc;
        private System.Windows.Forms.ListBox listac;
        private System.Windows.Forms.Button busca;
        private System.Windows.Forms.Button b_cancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button borraycalcula;
        private System.Windows.Forms.Button recopila_impares;
        private System.Windows.Forms.Button gen_11;
        private System.Windows.Forms.Button terminaciones;
        private System.Windows.Forms.TextBox n_hilos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button prec;
        private System.Windows.Forms.Button anticipos;
    }
}

