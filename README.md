<div align="center">
  <img src="https://img.shields.io/badge/ASP.NET%20Core%20MVC-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt="ASP.NET Core" />
  <img src="https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="EF Core" />
  <img src="https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap" />
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server" />
</div>

<h1 align="center">🏥 MediPlus - Sistema de Gestão de Clínica</h1>

<p align="center">
  <strong>Projeto de Desenvolvimento de Software em Ambiente Empresarial Simulado</strong><br />
  Uma aplicação web robusta para gestão de pacientes, médicos e agendamentos clínicos.
</p>

<hr />

### 📝 Descrição do Contexto
[cite_start]O projeto *MediPlus* transforma a sala de aula numa empresa de desenvolvimento de software[cite: 3]. [cite_start]A equipa é responsável por analisar, projetar e implementar uma solução real utilizando o padrão *ASP.NET Core MVC*[cite: 4, 13].

### 🛠️ Requisitos Técnicos Obrigatórios
* [cite_start]*Arquitetura:* Padrão MVC com separação clara entre Model, View e Controller[cite: 45, 46].
* [cite_start]*Base de Dados:* Persistência via *Entity Framework Core, utilizando DbContext e **Migrations*[cite: 49, 50, 51].
* [cite_start]*Segurança:* Implementação de *ASP.NET Core Identity* com controlo de acesso baseado em *Roles* (Admin/Utilizador)[cite: 62, 63].
* [cite_start]*Interface:* Uso de Layouts, pelo menos uma *Partial View* e um *View Component*[cite: 58, 59, 60].

### ⚙️ Funcionalidades e Regras de Negócio
[cite_start]O sistema gere as seguintes entidades e relações[cite: 22, 32]:
- [cite_start]*Gestão (CRUD):* Pacientes, Médicos, Especialidades e Marcações[cite: 28, 29, 30, 31].
- [cite_start]*Relações (1:N):* Especialidade para Médicos, Médico para Marcações e Paciente para Marcações[cite: 33, 34, 35].
- *Regras Críticas:*
  - [cite_start]❌ Proibição de marcações em datas passadas[cite: 37].
  - [cite_start]❌ Impedimento de sobreposição de horários para o mesmo médico[cite: 38].
- [cite_start]*Extras:* Agenda diária de médicos e filtros de marcações[cite: 40, 41].

### 📅 Planeamento (Sprints)
1.  [cite_start]*Sprint 1:* Análise, descrição do problema e diagrama de entidades[cite: 66, 67, 68].
2.  [cite_start]*Sprint 2:* Criação do projeto, Models e DbContext funcional[cite: 69, 70, 71, 72].
3.  [cite_start]*Sprint 3:* Implementação dos CRUDs principais e Views funcionais[cite: 73, 74].
4.  [cite_start]*Sprint 4:* Configuração de relações, Dropdowns e navegação funcional[cite: 75, 76].
5.  [cite_start]*Sprint 5:* Finalização da autenticação, Roles e polimento da interface[cite: 77, 78].

### 👥 Equipa de Desenvolvimento
[cite_start]A equipa é composta por 4 elementos (configuração extra-regulamentar ):
<table>
  <tr>
    <td align="center"><strong>Backend 1</strong><br />Infraestrutura & Segurança</td>
    <td align="center"><strong>Backend 2</strong><br />Lógica & Regras de Negócio</td>
    <td align="center"><strong>Frontend 1</strong><br />Layout & Navegação</td>
    <td align="center"><strong>Frontend 2</strong><br />Componentização & UX</td>
  </tr>
</table>

<hr />

<p align="center">
  [cite_start]<em>"Não basta funcionar, deve estar bem estruturado."</em> [cite: 99]
</p>
