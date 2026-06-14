# ACID — Transações Distribuídas com SQL Server e Windows Forms

**Demonstração prática das propriedades ACID em bases de dados distribuídas com SQL Server e uma aplicação Windows Forms em C#.**

---

## Índice

- [Visão Geral](#visão-geral)
- [Arquitectura](#arquitectura)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Configuração](#instalação-e-configuração)
- [Como Usar](#como-usar)
- [Funcionalidades](#funcionalidades)
- [Testes e Demonstrações](#testes-e-demonstrações)
- [Estrutura do Projecto](#estrutura-do-projecto)
- [Contribuições](#contribuições)

## Visão Geral

Este projecto demonstra os princípios fundamentais de **ACID** (Atomicidade, Consistência, Isolamento, Durabilidade) em sistemas de bases de dados distribuídos.

### Cenário Prático
Um sistema de gestão de stock entre múltiplos pontos de venda, onde:
- **PC1 (Servidor)**: SQL Server 2022 com base de dados VendasDB
- **PC2 e PC3 (Clientes)**: Aplicação Windows Forms que transfere stock entre produtos em tempo real

Cada cliente pode:
- Ver o stock actual em tempo real
- Executar transferências de stock (com garantias ACID)
- Simular falhas para visualizar ROLLBACK automático
- Observar bloqueio de leitura e Dirty Read com diferentes níveis de isolamento
- Registar toda a actividade em log com auditoria completa

---

## Arquitectura

```
┌─────────────────────────────────────────────────────┐
│                 Rede Local (LAN)                    │
├────────────────┬──────────────────────┬─────────────┤
│    PC1         │        PC2           │    PC3      │
│ (Servidor)     │    (Cliente)         │  (Cliente)  │
├────────────────┼──────────────────────┼─────────────┤
│ SQL Server     │ Windows Forms        │ Windows     │
│ 2022 Developer │ .NET Framework 4.8   │ Forms .NET  │
│ Port 1433      │ System.Data.SqlClient│ 4.8         │
│ Mixed Auth     │ ADO.NET              │ ADO.NET     │
└────────────────┴──────────────────────┴─────────────┘
         ↑                ↓                ↓
      TCP/IP Connection (1433)
      SqlConnection + SqlTransaction
```

### Fluxo de Transação

```
Cliente (PC2/PC3)          SQL Server (PC1)       Disco
    │                            │                 │
    ├─ BEGIN TRANSACTION ────────→ Abre transação  │
    │                                              │
    ├─ UPDATE origem ────────────→ Executa UPDATE1 │
    │  (reduz stock)                 (em memória)   │
    │                                              │
    ├─ UPDATE destino ───────────→ Executa UPDATE2 │
    │  (aumenta stock)              (em memória)    │
    │                                              │
    ├─ COMMIT ─────────────────→ Grava em disco ───→ WAL + Dados
    │                                              │
    └─ ✅ Sucesso               Persistido          │
```

---

## Pré-requisitos

### PC1 (Servidor)
- **Windows 10/11 ou Windows Server 2016+**
- **SQL Server 2022 Developer Edition** (gratuito): [Download](https://www.microsoft.com/sql-server/sql-server-downloads)
- **SQL Server Management Studio (SSMS)**
- **SQL Server Configuration Manager** (incluso)
- **.NET Framework 4.8** (para T-SQL, opcional)

### PC2 e PC3 (Clientes)
- **Windows 10/11**
- **Visual Studio 2022 Community** (gratuito): [Download](https://visualstudio.microsoft.com/)
  - Workload: ".NET desktop development"
- **.NET Framework 4.8**
- **NuGet Package**: System.Data.SqlClient

### Rede
- Todos os 3 PCs na **mesma rede local (LAN)**
- Ping entre máquinas deve funcionar
- Firewall configurado para porta TCP 1433

---

## Instalação e Configuração

### Configurar PC1 (Servidor SQL Server)

#### Passo 1: Instalar SQL Server 2022

```bash
# Descarregar de: https://www.microsoft.com/sql-server/sql-server-downloads
# Executar o instalador com modo "Basic"
```

#### Passo 2: Ativar Mixed Mode Authentication

Abre **SQL Server Management Studio**:

```sql
-- No SSMS, fazer clique direito no servidor → Properties → Security
-- Mudar "Server authentication" para "SQL Server and Windows Authentication mode"
```

Ou via PowerShell (Administrador no PC1):

```powershell
Restart-Service -Name MSSQLSERVER -Force
```

#### Passo 3: Activar TCP/IP

**SQL Server Configuration Manager**:
1. SQL Server Network Configuration → Protocols for MSSQLSERVER
2. Clique duplo em **TCP/IP** → Enable
3. Aba IP Addresses → IPAll → TCP Port = 1433
4. Clique OK e reinicie o serviço

#### Passo 4: Configurar Firewall

```powershell
# PowerShell Administrador
New-NetFirewallRule -DisplayName "SQL Server 1433" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
New-NetFirewallRule -DisplayName "SQL Server Browser 1434" -Direction Inbound -Protocol UDP -LocalPort 1434 -Action Allow
```

#### Passo 5: Criar Bases de Dados e Login

Abre **SSMS** e executa este script:

```sql
-- ════════════════════════════════════════════════════════
-- CRIAÇÃO DA BASE DE DADOS VendasDB
-- ════════════════════════════════════════════════════════

-- 1. Criar base de dados
CREATE DATABASE VendasDB;
GO

USE VendasDB;
GO

-- 2. Tabela Produtos
CREATE TABLE Produtos (
    ProdutoID   INT           PRIMARY KEY,
    NomeProduto VARCHAR(100)  NOT NULL,
    Descricao   VARCHAR(255),
    Preco       DECIMAL(10,2) NOT NULL,
    Stock       INT           NOT NULL CHECK (Stock >= 0)
);
GO

-- 3. Tabela de Log de Transações
CREATE TABLE LogTransacoes (
    LogID            INT IDENTITY(1,1) PRIMARY KEY,
    DataHora         DATETIME      DEFAULT GETDATE(),
    ProdutoOrigemID  INT,
    ProdutoDestinoID INT,
    Quantidade       INT,
    Status           VARCHAR(20),
    Mensagem         VARCHAR(500),
    PC               VARCHAR(50)
);
GO

-- 4. Inserir dados de exemplo
INSERT INTO Produtos (ProdutoID, NomeProduto, Descricao, Preco, Stock)
VALUES 
    (101, 'Produto A', 'Origem das transferências', 10.00, 100),
    (102, 'Produto B', 'Destino das transferências', 12.00, 50),
    (103, 'Produto C', 'Stock extra', 8.00, 75);
GO

-- 5. Criar login SQL para acesso remoto
CREATE LOGIN appuser WITH PASSWORD = 'Appuser1234',
    CHECK_POLICY = OFF,
    CHECK_EXPIRATION = OFF;
GO

CREATE USER appuser FOR LOGIN appuser;
EXEC sp_addrolemember 'db_owner', 'appuser';
GO

-- 6. Verificação
SELECT name, type_desc, is_disabled FROM sys.server_principals WHERE name = 'appuser';
GO
```

#### Passo 6: Descobrir IP do PC1

```cmd
ipconfig
```

Anota o **IPv4 Address** (ex: 192.168.1.10)

---

### Configurar PC2 e PC3 (Clientes Windows Forms)

#### Passo 1: Instalar Visual Studio 2022

```bash
# Download: https://visualstudio.microsoft.com/
# Durante instalação, selecciona ".NET desktop development"
```

#### Passo 2: Clonar ou Descarregar o Projecto

```bash
git clone https://github.com/jozzoficial/Transacoes-ACID.git
cd acid-demo
```

#### Passo 3: Abrir em Visual Studio

```
File → Open → Project/Solution → selecciona AcidDemo.sln
```

#### Passo 4: Instalar NuGet Package

```powershell
# Package Manager Console (Tools → NuGet Package Manager → Package Manager Console)
Install-Package System.Data.SqlClient
```

#### Passo 5: Actualizar Connection String

No `Form1.cs`, linha ~20, verifica:

```csharp
// ANTES — mudar para o IP real do PC1
string connectionString = $"Server=192.168.1.10,1433;" +
                           "Database=VendasDB;" +
                           "User Id=appuser;" +
                           "Password=Appuser1234;" +
                           "Connect Timeout=5;";
```

#### Passo 6: Compilar e Correr

```
Build → Build Solution
Debug → Start Debugging (F5)
```

---

## Como Usar

### 1. Conectar ao Servidor

1. Na aplicação, escreve o IP do PC1 no campo "IP do Servidor (PC1)"
2. Clica **"Conectar"**
3. Se bem-sucedido, vês ✅ e a grelha carrega os produtos

### 2. Transferir Stock (Demo ACID)

1. Selecciona:
   - Produto Origem ID: 101
   - Produto Destino ID: 102
   - Quantidade: 20
2. **Sem falha**: Clica **"Executar Transferência"**
   - Stock reduzido em 101, aumentado em 102 ✅
3. **Com falha**: Marca ✓ "Simular Falha", clica transferência
   - Transacção executa UPDATE1, depois falha
   - ROLLBACK automático, stock volta ao original 🔄

### 3. Demonstrar Isolamento (Dirty Read)

**Executar simultaneamente em PC2 e PC3:**

1. **PC2:**
   - Clica **"Iniciar Transação Suja"** (sem COMMIT)
   - Stock reduz (ex: 101 → 80), mas transacção fica aberta
   - Deixa aberto por ~10 segundos

2. **PC3:**
   - Selecciona **READ UNCOMMITTED** no dropdown
   - Clica **"Ler Stock"**
   - Vê 80 (valor sujo) + ⚠️ aviso de DIRTY READ

3. **PC2:**
   - Clica **ROLLBACK**
   - Stock volta a 101

4. **PC3:**
   - Clica novamente **"Ler Stock"**
   - Agora vê 101 (valor confirmado)
   - **Demonstra: 80 foi uma Dirty Read!**

---

## ✨ Funcionalidades

### 🔷 Atomicidade
- ✅ Transferências bem-sucedidas (COMMIT)
- ✅ Falhas simuladas (ROLLBACK automático)
- ✅ Stock nunca fica em estado inconsistente

### 🔷 Consistência
- ✅ Restrição CHECK (Stock >= 0) enforçada
- ✅ Validação programática de stock antes de UPDATE
- ✅ Erros desencadeiam ROLLBACK

### 🔷 Isolamento
- ✅ 4 níveis de isolamento: READ UNCOMMITTED, READ COMMITTED, REPEATABLE READ, SERIALIZABLE
- ✅ Demo de Dirty Read (READ UNCOMMITTED vs READ COMMITTED)
- ✅ Bloqueio visual de leituras em transacções pendentes

### 🔷 Durabilidade
- ✅ Tabela LogTransacoes registra cada operação
- ✅ Dados persistem após reiniciar aplicação/servidor
- ✅ Auditoria completa com DataHora, PC, Status

### 🎨 Experiência do Utilizador
- ✅ Log visual com cores (verde=sucesso, vermelho=erro, amarelo=em progresso)
- ✅ Timestamps em cada evento
- ✅ DataGridView atualiza em tempo real a cada 2 segundos
- ✅ Identificação automática do PC (hostname)

---

## 🧪 Testes e Demonstrações

### Teste 1: Atomicidade — Transferência com Sucesso
```
Resultado esperado: Stock origem -20, stock destino +20, COMMIT bem-sucedido
Status: ✅ PASSOU
```

### Teste 2: Atomicidade — Transferência com Falha
```
Resultado esperado: Stock inalterado (ROLLBACK executado)
Status: ✅ PASSOU
```

### Teste 3: Isolamento — Dirty Read
```
Resultado esperado: PC2 vê valor não confirmado com READ UNCOMMITTED
Status: ✅ PASSOU
```

### Teste 4: Isolamento — Bloqueio em READ COMMITTED
```
Resultado esperado: PC3 fica bloqueado esperando COMMIT/ROLLBACK de PC2
Status: ✅ PASSOU
```

### Teste 5: Durabilidade — Persistência
```
Resultado esperado: LogTransacoes mantém registos após reiniciar
Status: ✅ PASSOU
```

---

## 📁 Estrutura do Projecto

```
acid-demo/
├── AcidDemo/
│   ├── Form1.cs                 # Lógica principal (~450 linhas)
│   ├── Form1.Designer.cs        # UI/Layout (~300 linhas)
│   ├── Program.cs               # Entry point
│   ├── AcidDemo.csproj          # Configuração do projecto
│   └── App.config               # Configurações
├── SQL/
│   └── VendasDB_Setup.sql       # Script de criação da BD
├── Docs/
│   ├── Relatorio_ACID.docx      # Relatório técnico completo
│   └── Relatorio_Testes.docx    # Detalhes de implementação e testes
├── README.md                    # Este ficheiro
└── LICENSE                      # Licença académica
```

---

## 🔧 Troubleshooting

### ❌ Erro: "Login failed for user 'appuser'"

**Causas comuns:**
1. SQL Server em modo "Windows Authentication only" (não misto)
2. Login `appuser` não criado ou password errada
3. Firewall bloqueando porta 1433

**Solução:**
```powershell
# 1. Activar Mixed Mode (em SSMS Properties → Security)
# 2. Recriar login
# 3. Firewall rules
New-NetFirewallRule -DisplayName "SQL Server 1433" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
# 4. Reiniciar
Restart-Service -Name MSSQLSERVER -Force
```

### ❌ Erro: "Network path not found"

**Causa:** PC2/PC3 não conseguem atingir PC1

**Solução:**
```cmd
# Em PC2/PC3:
ping 192.168.1.10
telnet 192.168.1.10 1433
# Se não responder, verificar router/firewall
```

### ❌ Dados não actualizam em tempo real

**Causa:** Timer está parado ou DataGridView não actualiza

**Solução:** Verificar se o Timer está enable no designer:
```csharp
timer1.Enabled = true;
timer1.Interval = 2000;
```

---

## 📚 Documentação Adicional

- [Microsoft SQL Server Documentation](https://learn.microsoft.com/sql/)
- [ADO.NET SqlConnection](https://learn.microsoft.com/dotnet/api/system.data.sqlclient.sqlconnection)
- [Transaction Isolation Levels](https://learn.microsoft.com/sql/t-sql/statements/set-transaction-isolation-level-transact-sql)
- [Windows Forms C# Tutorial](https://learn.microsoft.com/dotnet/desktop/winforms/)

---

## 👨‍💼 Autores

Desenvolvido como projecto educacional para a disciplina de **Sistemas de Bases de Dados (SGBD)** na Universidade Kimpa Vita (UKV), Licenciatura em Engenharia Informática.

---

## 📄 Licença

Este projecto é disponibilizado para fins académicos e educacionais.

**Proibido o uso comercial ou redistributivo sem autorização.**

---

## 🤝 Contribuições

Se encontraste um bug ou tens uma melhoria:

1. Faz um Fork do projecto
2. Cria uma branch (`git checkout -b feature/MinhaFeature`)
3. Commit as mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abre um Pull Request

---

## 📞 Suporte

Para dúvidas ou problemas:
- Consulta a secção [Troubleshooting](#troubleshooting)
- Lê os relatórios em `/Docs/`
- Verifica os comentários no código em `Form1.cs`

---

**Desenvolvido com ❤️ em Uíge, Angola — Junho 2026**

**"ACID: Because Data Integrity Matters"** 🗄️✨
