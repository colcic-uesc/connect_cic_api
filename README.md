# Connect Cic

## Descrição
Connect Cic é uma API projetada para facilitar a conexão entre professores e alunos que estão em busca de oportunidades de projetos, como pesquisa e extensão. 

## Funcionalidades

- Registro de Usuários: Professores e alunos podem se cadastrar na plataforma.
- Publicação de Projetos: Professores podem publicar detalhes sobre projetos disponíveis, incluindo descrição, requisitos e prazos.
- Busca de Projetos: Alunos podem pesquisar projetos publicados pelos professores.
- Solicitação de Participação: Alunos podem solicitar participação em projetos que os interessem.
- Gerenciamento de Projetos: Professores podem revisar as solicitações dos alunos e aceitar ou rejeitar participantes.

## Tecnologias Utilizadas

- C#/.NET: Utilizado para desenvolver o backend da API, aproveitando a robustez e a eficiência do ecossistema .NET.
- ASP.NET Core: Framework utilizado para criar aplicativos web e APIs em C#.
- SQLite: Banco de dados relacional utilizado para armazenar os dados dos usuários e projetos. 
- Entity Framework Core: Framework de mapeamento objeto-relacional (ORM) utilizado para simplificar o acesso e manipulação de dados no banco SQLite.
- JWT (JSON Web Tokens): Utilizado para autenticação e autorização de usuários na API.

## Instalação e Uso
1. Clone o repositório: git clone https://github.com/colcic-uesc/connect_cic_api
2. Restaure as dependências do projeto: dotnet restore
3.  Execute o projeto: dotnet run

## Autores

- Ana Cristina
- Everaldina Guimarães
- Lavínia Fahning
- Maria Gabriella

## Licença
Este projeto é licenciado sob a MIT License.
