namespace AcidDemo
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblPC = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.dgvProdutos = new System.Windows.Forms.DataGridView();
            this.grpTransferencia = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudOrigem = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudQuantidade = new System.Windows.Forms.NumericUpDown();
            this.chkSimularFalha = new System.Windows.Forms.CheckBox();
            this.btnTransferir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDestino = new System.Windows.Forms.NumericUpDown();
            this.grpIsolamento = new System.Windows.Forms.GroupBox();
            this.cmbIsolamento = new System.Windows.Forms.ComboBox();
            this.btnLerStock = new System.Windows.Forms.Button();
            this.btnIniciarTransacao = new System.Windows.Forms.Button();
            this.btnRollback = new System.Windows.Forms.Button();
            this.btnCommit = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProdutos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrigem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestino)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(338, -1);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(108, 24);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "VendasDB";
            this.lblTitulo.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblPC
            // 
            this.lblPC.AutoSize = true;
            this.lblPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPC.Location = new System.Drawing.Point(12, 21);
            this.lblPC.Name = "lblPC";
            this.lblPC.Size = new System.Drawing.Size(43, 16);
            this.lblPC.TabIndex = 1;
            this.lblPC.Text = "PC: ?";
            this.lblPC.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(92, 37);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(100, 20);
            this.txtServer.TabIndex = 2;
            this.txtServer.Text = "192.168.56.1";
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(24, 164);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 3;
            this.btnConectar.Text = "CONECTAR";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // dgvProdutos
            // 
            this.dgvProdutos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProdutos.Location = new System.Drawing.Point(24, 270);
            this.dgvProdutos.Name = "dgvProdutos";
            this.dgvProdutos.ReadOnly = true;
            this.dgvProdutos.Size = new System.Drawing.Size(738, 239);
            this.dgvProdutos.TabIndex = 4;
            // 
            // grpTransferencia
            // 
            this.grpTransferencia.Location = new System.Drawing.Point(555, 59);
            this.grpTransferencia.Name = "grpTransferencia";
            this.grpTransferencia.Size = new System.Drawing.Size(183, 100);
            this.grpTransferencia.TabIndex = 5;
            this.grpTransferencia.TabStop = false;
            this.grpTransferencia.Text = "Transferência de Stock";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "De Produto ID:";
            // 
            // nudOrigem
            // 
            this.nudOrigem.Location = new System.Drawing.Point(96, 59);
            this.nudOrigem.Name = "nudOrigem";
            this.nudOrigem.Size = new System.Drawing.Size(47, 20);
            this.nudOrigem.TabIndex = 7;
            this.nudOrigem.ValueChanged += new System.EventHandler(this.nudOrigem_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Quantidade:";
            // 
            // nudQuantidade
            // 
            this.nudQuantidade.Location = new System.Drawing.Point(96, 108);
            this.nudQuantidade.Name = "nudQuantidade";
            this.nudQuantidade.Size = new System.Drawing.Size(47, 20);
            this.nudQuantidade.TabIndex = 9;
            this.nudQuantidade.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // chkSimularFalha
            // 
            this.chkSimularFalha.AutoSize = true;
            this.chkSimularFalha.Location = new System.Drawing.Point(24, 141);
            this.chkSimularFalha.Name = "chkSimularFalha";
            this.chkSimularFalha.Size = new System.Drawing.Size(154, 17);
            this.chkSimularFalha.TabIndex = 10;
            this.chkSimularFalha.Text = "Simular Falha (ROLLBACK)";
            this.chkSimularFalha.UseVisualStyleBackColor = true;
            // 
            // btnTransferir
            // 
            this.btnTransferir.Location = new System.Drawing.Point(103, 164);
            this.btnTransferir.Name = "btnTransferir";
            this.btnTransferir.Size = new System.Drawing.Size(75, 23);
            this.btnTransferir.TabIndex = 11;
            this.btnTransferir.Text = "Executar Transferência";
            this.btnTransferir.UseVisualStyleBackColor = true;
            this.btnTransferir.Click += new System.EventHandler(this.btnTransferir_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Para Produto ID:";
            // 
            // nudDestino
            // 
            this.nudDestino.Location = new System.Drawing.Point(97, 83);
            this.nudDestino.Name = "nudDestino";
            this.nudDestino.Size = new System.Drawing.Size(46, 20);
            this.nudDestino.TabIndex = 13;
            // 
            // grpIsolamento
            // 
            this.grpIsolamento.Location = new System.Drawing.Point(342, 61);
            this.grpIsolamento.Name = "grpIsolamento";
            this.grpIsolamento.Size = new System.Drawing.Size(207, 100);
            this.grpIsolamento.TabIndex = 14;
            this.grpIsolamento.TabStop = false;
            this.grpIsolamento.Text = "Nível de Isolamento (Dirty Read Demo)";
            // 
            // cmbIsolamento
            // 
            this.cmbIsolamento.FormattingEnabled = true;
            this.cmbIsolamento.Items.AddRange(new object[] {
            "READ UNCOMMITTED",
            "READ COMMITTED",
            "REPEATABLE READ",
            "SERIALIZABLE"});
            this.cmbIsolamento.Location = new System.Drawing.Point(342, 164);
            this.cmbIsolamento.Name = "cmbIsolamento";
            this.cmbIsolamento.Size = new System.Drawing.Size(121, 21);
            this.cmbIsolamento.TabIndex = 15;
            // 
            // btnLerStock
            // 
            this.btnLerStock.Location = new System.Drawing.Point(24, 193);
            this.btnLerStock.Name = "btnLerStock";
            this.btnLerStock.Size = new System.Drawing.Size(75, 23);
            this.btnLerStock.TabIndex = 16;
            this.btnLerStock.Text = "Ler Stock (com isolamento)";
            this.btnLerStock.UseVisualStyleBackColor = true;
            this.btnLerStock.Click += new System.EventHandler(this.btnLerStock_Click);
            // 
            // btnIniciarTransacao
            // 
            this.btnIniciarTransacao.Location = new System.Drawing.Point(106, 193);
            this.btnIniciarTransacao.Name = "btnIniciarTransacao";
            this.btnIniciarTransacao.Size = new System.Drawing.Size(75, 22);
            this.btnIniciarTransacao.TabIndex = 17;
            this.btnIniciarTransacao.Text = "Iniciar Transação Suja";
            this.btnIniciarTransacao.UseVisualStyleBackColor = true;
            this.btnIniciarTransacao.Click += new System.EventHandler(this.btnIniciarTransacao_Click);
            // 
            // btnRollback
            // 
            this.btnRollback.Location = new System.Drawing.Point(24, 223);
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new System.Drawing.Size(75, 23);
            this.btnRollback.TabIndex = 18;
            this.btnRollback.Text = "ROLLBACK";
            this.btnRollback.UseVisualStyleBackColor = true;
            this.btnRollback.Click += new System.EventHandler(this.btnRollback_Click);
            // 
            // btnCommit
            // 
            this.btnCommit.Location = new System.Drawing.Point(106, 222);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(75, 23);
            this.btnCommit.TabIndex = 19;
            this.btnCommit.Text = "COMMIT";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.Black;
            this.rtbLog.ForeColor = System.Drawing.Color.Lime;
            this.rtbLog.Location = new System.Drawing.Point(469, 163);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(269, 96);
            this.rtbLog.TabIndex = 20;
            this.rtbLog.Text = "";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Servidor:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 542);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.btnRollback);
            this.Controls.Add(this.btnIniciarTransacao);
            this.Controls.Add(this.btnLerStock);
            this.Controls.Add(this.cmbIsolamento);
            this.Controls.Add(this.grpIsolamento);
            this.Controls.Add(this.nudDestino);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTransferir);
            this.Controls.Add(this.chkSimularFalha);
            this.Controls.Add(this.nudQuantidade);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudOrigem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpTransferencia);
            this.Controls.Add(this.dgvProdutos);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblPC);
            this.Controls.Add(this.lblTitulo);
            this.Name = "Form1";
            this.Text = "DBVENDAS";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProdutos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrigem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestino)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblPC;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.DataGridView dgvProdutos;
        private System.Windows.Forms.GroupBox grpTransferencia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudOrigem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudQuantidade;
        private System.Windows.Forms.CheckBox chkSimularFalha;
        private System.Windows.Forms.Button btnTransferir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDestino;
        private System.Windows.Forms.GroupBox grpIsolamento;
        private System.Windows.Forms.ComboBox cmbIsolamento;
        private System.Windows.Forms.Button btnLerStock;
        private System.Windows.Forms.Button btnIniciarTransacao;
        private System.Windows.Forms.Button btnRollback;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label4;
    }
}

