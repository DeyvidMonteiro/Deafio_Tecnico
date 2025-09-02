API Gateway com Ocelot em .NET

Este projeto implementa uma API Gateway utilizando o framework Ocelot para centralizar e gerenciar o acesso a múltiplos microserviços.

Arquitetura do Projeto

O projeto segue a arquitetura de microserviços, onde o cliente interage apenas com a API Gateway, que redireciona as requisições para os serviços internos corretos:

API Gateway: http://localhost:5000

---
Tecnologias e Padrões Utilizados
Frameworks: O projeto é construído em .NET e ASP.NET Core. Entity Framework Core 
para mapeamento objeto-relacional. A API Gateway utiliza o framework Ocelot.

Arquitetura: A aplicação segue o padrão de Microserviços e utiliza API Gateway como ponto de entrada.

Os serviços internos (Vendas e Estoque) são baseados em CQRS (Command Query Responsibility Segregation).

Comunicação e Mensageria: As comunicações assíncronas são gerenciadas por filas de mensageria com RabbitMQ.

Autenticação e Autorização: A segurança e o gerenciamento de identidade são implementados com o Identity.

Banco de Dados e Persistência: A camada de dados utiliza SQL Server para o banco de dados. Para a organização do código,
são aplicados os padrões Repository.

---

Como Executar

Pré-requisitos:

.NET SDK

Docker(para o RabbitMQ)

SQL Server

---
Passos de Execução

Iniciar o RabbitMQ
Abra o Docker e inicie o container do RabbitMQ. Ele é necessário para a comunicação entre as APIs de Estoque e Vendas.

Iniciar os Microserviços
Abra um terminal para cada microserviço:
API Gateway, Estoque API, Vendas API, Identity API

Configurar a API Gateway configure a porta 5000 no arquivo launchSettings.json: "applicationUrl": "http://localhost:5000"

Rotas Disponíveis:

As requisições aos microserviços devem ser feitas através da API Gateway:

Estoque API

http://localhost:5000/estoque/categories

http://localhost:5000/estoque/products

Vendas API

http://localhost:5000/vendas/cart

http://localhost:5000/vendas/order

Identity API

http://localhost:5000/identity/login

http://localhost:5000/identity/register

