# .NET 9 + Vue 3 — Login Demo (PostgreSQL + SQLite)

Проект — страница логина на Vue 3, которая отправляет email и пароль на C# бэкенд. Бэкенд проверяет пользователя в PostgreSQL, а после успешного входа достаёт ФИО и адрес из SQLite и показывает их на фронтенде.

## Используется

- **Бэкенд:** ASP.NET Core 9 на C#
- **Фронтенд:** Vue 3 + TypeScript
- **Базы данных:** PostgreSQL (пользователи) + SQLite (профили)

## Быстрый старт

```bash
./start-dev.sh
```

Запускает PostgreSQL в Docker, бэкенд и фронтенд.

После первого запуска заполнить тестовыми данными:
```bash
docker exec -i test-postgres psql -U postgres -d testdb < backend/sql/postgres/seed.sql
sqlite3 backend/profiles.db < backend/sql/sqlite/seed.sql
```

Открыть: `http://localhost:5173`

**Тестовые данные:** `user@example.com` / `password123`

## Запуск по шагам

1. PostgreSQL

Если в docker-контейнере:
```bash
docker compose -f docker/docker-compose.yml up -d
```

Если нативно:
```bash
initdb -D pgdata --username=postgres --pwfile=<(echo postgres) -c unix_socket_directories=''
pg_ctl -D pgdata -l pgdata/logfile start
createdb -h localhost -U postgres testdb
```

2. Бэкенд
```bash
cd backend && dotnet run
```

3. Фронтенд (в отдельном терминале)
```bash
cd frontend && npm install && npm run dev
```

4. Заполнить тестовыми данными (требует прежде запуска бэкенда - там создаются таблицы)

4.1 PostgreSQL:

Если в docker-контейнере:
```bash
docker exec -i test-postgres psql -U postgres -d testdb < backend/sql/postgres/seed.sql
```

Если нативно:
```bash
psql -h localhost -U postgres -d testdb < backend/sql/postgres/seed.sql
```

4.2 SQLite:
```
sqlite3 backend/profiles.db < backend/sql/sqlite/seed.sql
```

## Остановка и очистка

1. PostgreSQL:
Если в docker-контейнере:
```bash
docker compose -f docker/docker-compose.yml down -v
# Без флага -v - только остановка. С флагом - очистка.
```

Если нативно:
```bash
pg_ctl -D pgdata stop; rm -rf pgdata
```

2. SQLite: достаточно удалить `backend/profiles.db`
