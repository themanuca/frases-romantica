## Projeto Frases Romântica
 O intuito do projeto é desenvolver uma pequena aplicação que gere frases romanticas programada e envie para minha noiva, via whatsapp.
Aplicação usára LLM para gerar as frases, para nunca se repitrem e para serem únicas.
Terá um painel web com dashboard para gerenciar tudo de forma visual.
- Modo Anônimo: Permitir que outras pessoas usem o sistema de para enviar mensagens secretas;
- Poderá gerar as frases por temas e humor;
- API terá um endpoint publico (*/api/frases/aleatoria*);
- Ranking das frases mais curtidas;
  
##STACKS
- RABBITMQ;
- DOCKER
- ASP.NET CORE
- NEXTJS (REACT)
- SQL Server
- ~~Entity Framework~~ Dapper (A escolha se da pelo fato de ter mais controle sobre a query usada)
- Tailwind
## EF ou Dapper
Vendo a documentação para uso do EF e controle das query usada, decide optar pelo Dapper, o uso dos .FromSql() e .FromSQLRaw() do LINQ é interessante mas dificulta o uso de argumentos e paramentros na query.
(https://learn.microsoft.com/pt-br/ef/core/querying/sql-queries?tabs=postgres).
O repostiório do Dapper mostra que o controle sobre a query é mais facil: https://github.com/DapperLib/Dapper
