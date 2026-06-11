using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcidDemo
{
    public partial class Form1 : Form
    {

        //CONFIGURAÇÕES
        private string connectionString = "";
        private SqlConnection dirtyConnection = null;  // para demo dirty read
        private SqlTransaction dirtyTransaction = null;
        private string pcName = System.Environment.MachineName;


        //CONSTRUTOR
        public Form1()
        {
            InitializeComponent();
            lblPC.Text = "PC: " + pcName;
            cmbIsolamento.SelectedIndex = 1; // READ COMMITTED por padrão
            timer1.Tick += Timer1_Tick;
        }

        //CONEXÃO
        private void btnConectar_Click(object sender, EventArgs e)
        {
            connectionString = $"Server={txtServer.Text},1433;" +
                               "Database=VendasDB;" +
                               "User Id=appuser;Password=App@1234;" +
                               "Connect Timeout=5;";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Log("Conectado ao servidor: " + txtServer.Text, Color.LimeGreen);
                    CarregarProdutos();
                    timer1.Start();
                    btnConectar.Enabled = false;
                    btnTransferir.Enabled = true;
                    btnLerStock.Enabled = true;
                    btnIniciarTransacao.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Log("Erro ao conectar: " + ex.Message, Color.Red);
                MessageBox.Show("Falha na ligação:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //CARREGAR PRODUTOS
        private void CarregarProdutos()
        {
            if (string.IsNullOrEmpty(connectionString)) return;
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var adapter = new SqlDataAdapter(
                        "SELECT ProdutoID, NomeProduto, Descricao, Preco, Stock FROM Produtos ORDER BY ProdutoID",
                        conn);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dgvProdutos.DataSource = dt;
                    dgvProdutos.Columns["ProdutoID"].HeaderText = "ID";
                    dgvProdutos.Columns["NomeProduto"].HeaderText = "Nome";
                    dgvProdutos.Columns["Descricao"].HeaderText = "Descrição";
                    dgvProdutos.Columns["Preco"].HeaderText = "Preço (€)";
                    dgvProdutos.Columns["Stock"].HeaderText = "Stock";
                    dgvProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                Log("Erro ao carregar produtos: " + ex.Message, Color.Orange);
            }
        }

        //ATUALIZAR TIMER A CADA 2S
        private void Timer1_Tick(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        //TRANSFERÊNCIA DE STOCK (ACID Demo)
        private void btnTransferir_Click_1(object sender, EventArgs e)
        {
            int origem = (int)nudOrigem.Value;
            int destino = (int)nudDestino.Value;
            int quantidade = (int)nudQuantidade.Value;
            bool simularFalha = chkSimularFalha.Checked;

            if (origem == destino)
            {
                MessageBox.Show("Produto de origem e destino não podem ser iguais!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Log($"Iniciando transferência: Produto {origem} → Produto {destino} | Qtd: {quantidade}", Color.Yellow);

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    //ATOMICIDADE: as duas operações são uma só unidade

                    // 1. Verificar stock disponível
                    var cmdCheck = new SqlCommand(
                        "SELECT Stock FROM Produtos WHERE ProdutoID = @id",
                        conn, trans);
                    cmdCheck.Parameters.AddWithValue("@id", origem);
                    int stockAtual = (int)cmdCheck.ExecuteScalar();

                    Log($"   Stock atual do Produto {origem}: {stockAtual}", Color.Cyan);

                    if (stockAtual < quantidade)
                        throw new Exception($"Stock insuficiente! Disponível: {stockAtual}, Necessário: {quantidade}");

                    // 2. Reduzir stock de origem
                    var cmdReducir = new SqlCommand(
                        "UPDATE Produtos SET Stock = Stock - @qtd WHERE ProdutoID = @id",
                        conn, trans);
                    cmdReducir.Parameters.AddWithValue("@qtd", quantidade);
                    cmdReducir.Parameters.AddWithValue("@id", origem);
                    cmdReducir.ExecuteNonQuery();
                    Log($"   Stock do Produto {origem} reduzido em {quantidade}", Color.LightYellow);

                    // 3. SIMULAR FALHA (antes do segundo UPDATE)
                    if (simularFalha)
                        throw new Exception("Falha simulada! O segundo UPDATE não foi executado.");

                    // 4. Aumentar stock de destino
                    var cmdAumentar = new SqlCommand(
                        "UPDATE Produtos SET Stock = Stock + @qtd WHERE ProdutoID = @id",
                        conn, trans);
                    cmdAumentar.Parameters.AddWithValue("@qtd", quantidade);
                    cmdAumentar.Parameters.AddWithValue("@id", destino);
                    cmdAumentar.ExecuteNonQuery();
                    Log($"    Stock do Produto {destino} aumentado em {quantidade}", Color.LightYellow);

                    // 5. COMMIT
                    trans.Commit();
                    Log($"COMMIT — Transferência concluída com sucesso! (PC: {pcName})", Color.LimeGreen);

                    // Registar no log
                    RegistarLog(conn, null, origem, destino, quantidade, "SUCESSO", "Transferência OK", pcName);
                }
                catch (Exception ex)
                {
                    // ROLLBACK — atomicidade garantida
                    trans.Rollback();
                    Log($"ROLLBACK executado! Motivo: {ex.Message}", Color.Red);
                    Log("   ↩️  Base de dados voltou ao estado anterior (ATOMICIDADE garantida)", Color.Orange);

                    using (var conn2 = new SqlConnection(connectionString))
                    {
                        conn2.Open();
                        RegistarLog(conn2, null, origem, destino, quantidade, "ROLLBACK", ex.Message, pcName);
                    }
                    MessageBox.Show("ROLLBACK!\n\n" + ex.Message, "Transação Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            CarregarProdutos();

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void nudOrigem_ValueChanged(object sender, EventArgs e)
        {

        }

        //Registar no Log da BD
        private void RegistarLog(SqlConnection conn, SqlTransaction trans,
            int origem, int destino, int qtd, string status, string msg, string pc)
        {
            try
            {
                var cmd = new SqlCommand(
                    "INSERT INTO LogTransacoes (ProdutoOrigemID, ProdutoDestinoID, Quantidade, Status, Mensagem, PC) " +
                    "VALUES (@o, @d, @q, @s, @m, @pc)", conn, trans);
                cmd.Parameters.AddWithValue("@o", origem);
                cmd.Parameters.AddWithValue("@d", destino);
                cmd.Parameters.AddWithValue("@q", qtd);
                cmd.Parameters.AddWithValue("@s", status);
                cmd.Parameters.AddWithValue("@m", msg);
                cmd.Parameters.AddWithValue("@pc", pc);
                cmd.ExecuteNonQuery();
            }
            catch { /* log é secundário, não queremos falhar por isso */ }
        }


        //Log visual na RichTextBox 
        private void Log(string mensagem, Color cor)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => Log(mensagem, cor)));
                return;
            }
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = Color.Gray;
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] ");
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionColor = cor;
            rtbLog.AppendText(mensagem + "\n");
            rtbLog.ScrollToCaret();
        }


        // DEMO DIRTY READ

        // Botão: Iniciar transação suja (sem COMMIT)
        private void btnIniciarTransacao_Click(object sender, EventArgs e)
        {
            if (dirtyTransaction != null)
            {
                Log("Já existe uma transação aberta. Faz COMMIT ou ROLLBACK primeiro.", Color.Orange);
                return;
            }

            dirtyConnection = new SqlConnection(connectionString);
            dirtyConnection.Open();
            dirtyTransaction = dirtyConnection.BeginTransaction(IsolationLevel.ReadCommitted);

            int produtoId = (int)nudOrigem.Value;
            int qtd = (int)nudQuantidade.Value;

            var cmd = new SqlCommand(
                "UPDATE Produtos SET Stock = Stock - @qtd WHERE ProdutoID = @id",
                dirtyConnection, dirtyTransaction);
            cmd.Parameters.AddWithValue("@qtd", qtd);
            cmd.Parameters.AddWithValue("@id", produtoId);
            cmd.ExecuteNonQuery();

            Log($"Transação aberta (SEM COMMIT): Produto {produtoId} stock reduzido em {qtd}", Color.Yellow);
            Log("   Agora no outro PC, clicar em 'Ler Stock'. Com READ UNCOMMITTEDveremos o valor não confirmado!", Color.Cyan);
            btnCommit.Enabled = true;
            btnRollback.Enabled = true;
            btnIniciarTransacao.Enabled = false;
        }


        // Botão Ler stock com o nível de isolamento selecionado
        private void btnLerStock_Click(object sender, EventArgs e)
        {
            int produtoId = (int)nudOrigem.Value;
            string isolamento = cmbIsolamento.SelectedItem.ToString();

            IsolationLevel nivel;
            switch (isolamento)
            {
                case "READ UNCOMMITTED": nivel = IsolationLevel.ReadUncommitted; break;
                case "REPEATABLE READ": nivel = IsolationLevel.RepeatableRead; break;
                case "SERIALIZABLE": nivel = IsolationLevel.Serializable; break;
                default: nivel = IsolationLevel.ReadCommitted; break;
            }

            Log($" Lendo Produto {produtoId} com isolamento: {isolamento}", Color.Cyan);

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var trans = conn.BeginTransaction(nivel);
                    var cmd = new SqlCommand(
                        "SELECT Stock FROM Produtos WHERE ProdutoID = @id",
                        conn, trans);
                    cmd.Parameters.AddWithValue("@id", produtoId);

                    // Timeout de 5 segundos para não bloquear forever
                    cmd.CommandTimeout = 5;

                    try
                    {
                        int stock = (int)cmd.ExecuteScalar();
                        trans.Commit();
                        Log($"   Stock lido: {stock}  [{isolamento}]", Color.LimeGreen);
                        if (isolamento == "READ UNCOMMITTED")
                            Log("   ATENÇÃO: Pode ser uma DIRTY READ (valor não confirmado)!", Color.OrangeRed);
                    }
                    catch (SqlException)
                    {
                        trans.Rollback();
                        Log("   Leitura BLOQUEADA! Outro PC tem uma transação aberta (READ COMMITTED a funcionar).", Color.Orange);
                    }
                }
            }
            catch (Exception ex)
            {
                Log("   Erro: " + ex.Message, Color.Red);
            }
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            if (dirtyTransaction == null) return;
            dirtyTransaction.Commit();
            Log("COMMIT feito na transação aberta.", Color.LimeGreen);
            FecharTransacaoSuja();
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {
            if (dirtyTransaction == null) return;
            dirtyTransaction.Rollback();
            Log("ROLLBACK feito na transação aberta.", Color.Red);
            FecharTransacaoSuja();
        }

        private void FecharTransacaoSuja()
        {
            dirtyTransaction?.Dispose();
            dirtyConnection?.Close();
            dirtyConnection?.Dispose();
            dirtyTransaction = null;
            dirtyConnection = null;
            btnCommit.Enabled = false;
            btnRollback.Enabled = false;
            btnIniciarTransacao.Enabled = true;
            CarregarProdutos();
        }


        //Fechar limpo
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer1.Stop();
            FecharTransacaoSuja();
            base.OnFormClosing(e);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
