API em .NET

Este projeto implementa uma API Gateway utilizando o framework Ocelot para centralizar e gerenciar o acesso a múltiplos microserviços, bem como o RabbitMQ para mensageria.
Os serviços são: Identity Api, Estoque Api, Vendas Api, Gateway Api.

A Identity API gerencia registro, login e roles, gerando tokens JWT que permitem acessar endpoints protegidos.
A Vendas API controla carrinho e pedidos, usando o token da Identity para autenticação e consultando a Estoque API.
A Estoque API mantém os produtos , servindo dados para a API de Vendas.

---
Tecnologias e Padrões Utilizados
Frameworks: O projeto é construído em .NET e ASP.NET Core. Entity Framework Core 
para mapeamento objeto-relacional. A API Gateway utiliza o framework Ocelot.

Arquitetura: A aplicação segue o padrão de Microserviços e utiliza API Gateway como ponto de entrada.

Os serviços internos (Vendas, Estoque ) são baseados em CQRS (Command Query Responsibility Segregation).

Comunicação e Mensageria: As comunicações assíncronas são gerenciadas por filas de mensageria com RabbitMQ.

Autenticação e Autorização: A segurança e o gerenciamento de identidade são implementados atraves da API Identity.

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

As requisições aos microserviços podem ser feitas através da API Gateway:

---

Vendas API (todas protegidas)

GET http://localhost:5000/vendas/cart/getcart/{userId}

POST http://localhost:5000/vendas/cart/addcart/{userId}

DELETE http://localhost:5000/vendas/cart/deletecartitem/{id}

DELETE http://localhost:5000/vendas/cart/clearcart?userId={userId}

GET http://localhost:5000/vendas/order/{id}

POST http://localhost:5000/vendas/order/finalize/{userId}

---

Identity API

Público:

POST http://localhost:5000/identity/login

POST http://localhost:5000/identity/register

Protegido :

POST http://localhost:5000/identity/createRole?roleName={roleName}

POST http://localhost:5000/identity/AddUserToRole?email={email}&roleName={roleName}

POST http://localhost:5000/identity/revoke/{username}

