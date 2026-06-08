# Sistema de Gestão de Biblioteca (Arquitetura em Microsserviços)

## Propósito do Sistema
O sistema tem como propósito automatizar e gerenciar o fluxo operacional de uma biblioteca, controlando o catálogo do acervo de livros e registrando o histórico de empréstimos e devoluções.

## Usuários do Sistema
* **Bibliotecários/Administradores:** Responsáveis por alimentar o acervo, gerenciar os cadastros e registrar as movimentações de empréstimo.

## Requisitos Funcionais
* Cadastrar, consultar e atualizar livros no acervo.
* Cadastrar e consultar leitores (nome, e-mail, telefone e status de multas).
* Registrar novos empréstimos (validando se o livro está com status "disponível").
* Registrar devolução de livros.
* Isentar/zerar multas de leitores automaticamente após a devolução.
* Consultar recibo detalhado de empréstimo.

## Descritivo Técnico (Microsserviços)
A aplicação foi desenvolvida utilizando .NET, SQLite e comunicação REST, dividida nos seguintes microsserviços:

1. **MsAcervo:** Responsável exclusivamente por gerenciar o catálogo de livros e a disponibilidade física de cada exemplar.
2. **MsLeitores:** Responsável por armazenar os dados de contato dos usuários da biblioteca e controlar pendências financeiras (multas por atraso).
3. **MsEmprestimos:** Responsável por orquestrar a regra de negócio central. Ele realiza buscas no **MsAcervo** para validar a disponibilidade antes de um empréstimo, busca dados no **MsLeitores** para gerar recibos e dispara ações de alteração de dados (zerar multas) no **MsLeitores** quando uma devolução é efetivada.
