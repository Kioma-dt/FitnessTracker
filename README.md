# Fitness Tracker API

## Настройка переменных окружения

Перед запуском необходимо задать переменные окружения в файле .env в соотвествии с .env.example

## Запуск приложения

### Запуск через `dotnet run`

Перед запуском приложения необходими применить миграции к базе данных командой:

```bash
dotnet ef database update --project FitnessTracker.DataAccess
```

Приложение можно запустить командой:

```bash
dotnet run --project FitnessTracker.API
```

После запуска API будет доступно по следующим адресам:

- HTTP: `http://localhost:5184`
- HTTPS: `https://localhost:7245`

---

### Запуск через Docker Compose

Необходими выполнить команду:

```bash
docker compose up --build
```

После запуска API будет доступно по адресу:

- HTTP: `http://localhost:5184`

---

## Swagger UI

Документация API через Swagger UI доступна по адресу:

- При локальном запуске:
  - `http://localhost:5184/swagger`
  - `https://localhost:7245/swagger`

- При запуске через Docker:
  - `http://localhost:5184/swagger`
