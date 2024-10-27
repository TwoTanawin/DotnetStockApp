dotnet ef dbcontext scaffold "Host=localhost;Port=5434;Database=storedb;Username=admin;Password=password" \
Npgsql.EntityFrameworkCore.PostgreSQL \
--output-dir "Models" \
--context ApplicationDbContext \
--use-database-names \
--no-onconfiguring \
--force