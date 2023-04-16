start:
	docker-compose -f docker-compose.yml up -d
stop:
	docker-compose -f docker-compose.yml down
bash:
	docker exec -it anur-backend-1  "bash"
install:
	docker exec -it anur-backend-1 "bash" -c "dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL"
	docker exec -it anur-backend-1 "bash" -c "dotnet add package Microsoft.EntityFrameworkCore.Design"
	docker exec -it anur-backend-1 "bash" -c "dotnet add package Microsoft.EntityFrameworkCore.Tools"
	docker exec -it anur-backend-1 "bash" -c "dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer"
	docker exec -it anur-backend-1 "bash" -c "dotnet add package Microsoft.EntityFrameworkCore.Analyzers"
	docker exec -it anur-backend-1 "bash" -c "dotnet add package Faker.NETCore --version 1.0.2"
	docker exec -it anur-backend-1 "bash" -c "dotnet tool install --global dotnet-ef"
migrate:
	docker exec -it anur-backend-1 "bash" -c 'export PATH="$$PATH:/root/.dotnet/tools" && dotnet-ef database update --no-build'
