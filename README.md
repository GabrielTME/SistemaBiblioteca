# 📚 Sistema de Gestão de Biblioteca - Microsserviços

[cite_start]Projeto desenvolvido como requisito avaliativo prático para a construção do back-end de uma aplicação utilizando a arquitetura de microsserviços[cite: 3, 5]. 

---

## 👥 Equipe de Desenvolvimento
[cite_start]O projeto foi desenvolvido em equipe, conforme as diretrizes da avaliação[cite: 28]. [cite_start]Todos os membros realizaram contribuições (commits) diretas no repositório[cite: 18]:
* **[Bruno Sebastião Tescke Martins]**
* **[Gabriel Tomé]**
* **[Tiago Fritzen Palácio]**

---

## 🎯 1. Documento de Requisitos

[cite_start]Em conformidade com a documentação solicitada[cite: 12, 19], abaixo estão descritos os pilares funcionais do sistema:

### a. Propósito do Sistema
Automatizar e modernizar a gestão de uma biblioteca. O sistema controla de forma distribuída o acervo de livros, o registro de usuários (leitores) e a complexa operação de empréstimos e devoluções. [cite_start]A escolha pela arquitetura de microsserviços garante escalabilidade e isolamento de falhas entre os domínios da aplicação[cite: 14].

### b. Usuários do Sistema
O sistema foi projetado para ser operado por:
* [cite_start]**Bibliotecários e Administradores:** Responsáveis por registrar o acervo, incluir novos leitores, verificar pendências e orquestrar as saídas e entradas de livros no balcão de atendimento[cite: 15].

### c. Requisitos Funcionais
* `[RF-01]` O sistema deve permitir o cadastro e consulta de exemplares de livros.
* `[RF-02]` O sistema deve permitir o cadastro de leitores, registrando dados de contato e pendências financeiras (multas).
* `[RF-03]` O sistema deve permitir o registro de empréstimos, validando obrigatoriamente se o exemplar está com status "Disponível".
* `[RF-04]` O sistema deve permitir o registro de devoluções de exemplares.
* `[RF-05]` O sistema deve perdoar (zerar) automaticamente qualquer multa pendente de um leitor no momento em que ele efetiva uma devolução.
* [cite_start]`[RF-06]` O sistema deve gerar um recibo unificado de empréstimo contendo os detalhes da transação e os dados de contato do leitor cruzados[cite: 16].

---

## 🛠️ 2. Descritivo Técnico e Arquitetura

Para garantir o baixo acoplamento e a alta coesão, a aplicação não possui um banco de dados monolítico. [cite_start]Utilizou-se o padrão *Database-per-service* através do Entity Framework Core, onde cada serviço domina exclusivamente seus dados e os expõe via rotas REST[cite: 11, 17]. 

[cite_start]A arquitetura foi dividida em três microsserviços[cite: 7]:

### 📦 MsAcervo (Porta: 5001)
* **Função:** Domínio de catálogo. Gerencia os exemplares físicos da biblioteca.
* [cite_start]**Tecnologias:** .NET, C#, SQLite, Swagger[cite: 11].
* **Persistência:** `acervo.db` (Tabela: *Livros*).

### 👥 MsLeitores (Porta: 5002)
* **Função:** Domínio de usuários. Armazena as PIIs (Informações Pessoalmente Identificáveis) dos leitores e controla o status de multas por atraso.
* [cite_start]**Tecnologias:** .NET, C#, SQLite, Swagger[cite: 11].
* **Persistência:** `leitores.db` (Tabela: *Leitores*).

### 🔄 MsEmprestimos (Porta: 5003)
* **Função:** Domínio orquestrador de negócios. Não repete os dados de livros e leitores, armazenando apenas os IDs referentes e as datas da transação. Ele atua como o integrador principal da arquitetura.
* [cite_start]**Tecnologias:** .NET, C#, SQLite, Swagger, `HttpClient` (para comunicação inter-serviços)[cite: 11].
* **Persistência:** `emprestimos.db` (Tabela: *Emprestimos*).

---

## 🔌 3. Mapa de Integrações (Comunicação Inter-Serviços)

[cite_start]O projeto cumpre rigorosamente a exigência de três integrações de rede assíncronas entre os microsserviços[cite: 8]:

1. [cite_start]**Integração de Busca Simples 1 (Empréstimo → Acervo)[cite: 9, 35]:**
   * **Gatilho:** Requisição `POST /api/Emprestimos`.
   * **Ação:** Antes de persistir, o *MsEmprestimos* realiza um `GET` via HttpClient na rota `/api/Acervo/{id}` do *MsAcervo*.
   * **Validação:** Analisa se o atributo `statusDisponibilidade` é igual a "Disponível". Se for "Emprestado" ou "Perdido", a transação é cancelada.

2. [cite_start]**Integração de Busca Simples 2 (Empréstimo → Leitores)[cite: 9, 34]:**
   * **Gatilho:** Requisição `GET /api/Emprestimos/{id}/recibo`.
   * **Ação:** O *MsEmprestimos* faz um `GET` no *MsLeitores* (`/api/Leitores/{id}`) para enriquecer o JSON de resposta com o telefone e e-mail do titular do empréstimo em tempo real.

3. [cite_start]**Integração de Alteração de Dados (Empréstimo → Leitores)[cite: 10, 36]:**
   * **Gatilho:** Requisição `PUT /api/Emprestimos/{id}/devolver`.
   * **Ação:** Ao marcar o livro como devolvido no próprio banco, o *MsEmprestimos* dispara uma requisição `PUT` para `/api/Leitores/{id}/zerar-multa` no *MsLeitores*, forçando a alteração e o perdão da dívida no banco de dados parceiro.

---

## 🚀 4. Guia de Execução Local

1. Clone este repositório (`git clone`).
2. Certifique-se de possuir o **.NET SDK** instalado na máquina.
3. Abra a solução/pasta raiz no seu **Visual Studio** ou IDE de preferência.
4. No Visual Studio, configure a Solução para **"Vários projetos de inicialização"** (Multiple startup projects).
5. Certifique-se de que a Ação dos três projetos (`MsAcervo`, `MsLeitores` e `MsEmprestimos`) esteja marcada como **"Iniciar"**.
6. Compile e rode o projeto (`F5`). Os serviços serão alocados em portas fixas configuradas de forma absoluta (Hardcoded Host):
   * [Swagger - Acervo](http://localhost:5001/swagger)
   * [Swagger - Leitores](http://localhost:5002/swagger)
   * [Swagger - Empréstimos](http://localhost:5003/swagger)

---

## ✅ 5. Checklist de Critérios da Avaliação

Esta seção comprova o atendimento integral aos critérios estabelecidos no documento oficial da disciplina:

- [cite_start][X] **Critério 1:** Possui pelo menos três microsserviços implementados[cite: 7].
- [cite_start][X] **Critério 2.a:** Possui duas integrações de busca simples de dados (Validação de Acervo e Geração de Recibo)[cite: 8, 9].
- [cite_start][X] **Critério 2.b:** Possui uma integração de alteração de dados (Devolução do livro e perdão automático de multa no leitor)[cite: 8, 10].
- [cite_start][X] **Critério 3:** Construído utilizando o template exigido (.NET, SQLite, REST)[cite: 11].
- [cite_start][X] **Critério 4:** Documento de requisitos redigido de forma sucinta[cite: 12, 13].
- [cite_start][X] **Critério 5:** Descritivo técnico com a explicação da função de cada serviço[cite: 17].
- [cite_start][X] **Critério 6:** Entregue via GitHub com histórico de commits dos membros da equipe[cite: 18].
- [cite_start][X] **Critério 7:** Entregáveis extras considerados para aumento de nota, como aplicação sólida dos conceitos de Injeção de Dependência e tratamento assíncrono de HTTP[cite: 20].
