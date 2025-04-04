# Api RESTful C# .NET 8

Api RESTful desenvolvida em C# .NET 8

# Tecnologias usadas:
- [x] **C#: Linguagem base**
- [x] **.NET 8: Framework**
- [x] **EntityFramework: ORM**
- [x] **JWT: Autenticação**

# Como rodar o projeto:
  - Clone o repositório;
  - Preencha a ConnectionString em appsetting.json;
  - Crie e execute uma migration ou execute a migration '20250404165252_UpdateReservationsAndRoomsTable.cs';
  - Rode a solution;
  - Acesse o link que aparecerá no console, o mesmo abrirá o swagger, garantindo que a API está rodando perfeitamente;
  - Se preferir, utilize a collection do postman localizada na pasta raíz do repositório.

# Estimativa de horas gastas em cada atividade feita durante todo o processo:
- [x] **Estudar APIs em C# .NET 8: 3h**
- [x] **Testes aleatórios pos estudos: 3h**
- [x] **Estudar EntityFramework: 2h**
- [x] **Testes aleatórios pos estudos: 1h**
- [x] **Desenvolvimento total do projeto: 20h**
- **Estimativa de horas totais gastas: 29h**

# Regras de negócio:
- Admins:
  Qualquer pessoa pode se cadastrar como Admin, pois o endpoint de cadastro de Admin é livre, porém ao cadastrar-se, o campo 'IsAdmin' no DB, por padrão, é false. Essa regra foi criada visando um sistema externo, separado desta API, que faria a ativação da permissão de Admin dos usuários cadastrados como tal.
  
  - Foi criado uma categoria Admin, com tabelas e endpoints separados dos Usuários;
  - Somente Admins podem editar usuários (mudar nome e status);
  - Somente Admins podem inativar usuários;
  - Somente Admins podem ver a lista de usuários (tanto ativos como inativos).

- Usuários:
  Usuários possuem endpoints específicos, que só eles têm acesso, assim como os Admins.
  - Usuários podem cadastrar-se no sistema;
  - Usuários podem logar-se no sistema;
  - Um Admin não pode utilizar os mesmos dados de sua conta para se cadastrar como usuário;
  - Apenas usuários podem editar suas senhas, caso um Admin tente fazer isso, mesmo que saiba todos os dados do usuário, não será possível.

- Salas
    - Apenas Admins podem criar salas;
    - Apenas Admins podem editar salas;
    - Apenas Admins podem inativar salas;
    - Apenas Admins podem ver todas as salas (ativas e inativas);
    - Todos podem ver as salas ativas.
 
- Reservas
    - Apenas usuários podem criar reservas;
    - Admins e usuários podem editar reservas;
    - Admins e usuários podem inativar reservas;
    - Apenas admins podem ver todas as reservas (Ativas e inativas);
    - Todos podem ver as reservas ativas;
    - Todos podem buscar reservas pelo endpoint 'listar_reservas_filtradas'.

# Considerações Finais:

Esse foi um projeto de API feito em um curto intervalo de tempo, porém utilizei todo o meu conhecimento atual (até a data de hoje 04/04/2025) para desenvolve-lo. Fiquei deveras satisfeito com o reusltado, e mesmo sabendo que dá sim para melhorar alguns pontos, ficom um projeto bem estruturado e com uma boa escalabilidade.
