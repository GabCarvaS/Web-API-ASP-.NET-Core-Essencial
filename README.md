# Aplicação ASP .NET Core com Clean Architecture

Este repositório contém a implementação de uma aplicação ASP .NET Core MVC seguindo os princípios da **Clean Architecture**. O objetivo é criar uma aplicação robusta, escalável e fácil de manter, aplicando boas práticas de desenvolvimento de software.

## Sobre o Projeto

A Clean Architecture refere-se à organização do projeto de forma que seja fácil de entender e modificar conforme o projeto cresce. Durante este projeto, partimos de uma solução monolítica para uma estrutura com cinco projetos distintos, cada um com responsabilidades específicas.

### Estrutura do Projeto

A solução final é composta pelos seguintes projetos:

- **Domain**: Contém as entidades e interfaces de domínio.
- **Application**: Implementa a lógica de negócios e casos de uso.
- **Infrastructure**: Gerencia a persistência de dados e outras infraestruturas externas.
- **IoC**: Configura a injeção de dependências.
- **Presentation**: A aplicação ASP .NET Core MVC que interage com os usuários.

### Principais Conceitos Aplicados

- **Clean Architecture**: Organização do código em camadas separadas para facilitar a manutenção e evolução.
- **Domain-Driven Design (DDD)**: Foco no domínio do problema e na lógica de negócios.
- **Padrões Repository e CQRS**: Implementação para gerenciar o acesso a dados e a separação de comandos e consultas.
- **Princípios DRY, YAGNI e KISS**: Escrita de código claro, conciso e reutilizável.
- **Injeção de Dependência**: Desacoplamento de componentes para facilitar testes e manutenção.

## Tecnologias Utilizadas

- ASP .NET Core
- C#
- Entity Framework Core
- Visual Studio 2022 Community
- .NET 8.0
