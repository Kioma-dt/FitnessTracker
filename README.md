# Fitness Tracker API

## Перед запуском необходимо задать следующие переменные окружения:

```env
DB_HOST=postgres
DB_PORT=5432
DB_NAME=fitness
DB_USER=postgres
DB_PASSWORD=Kioma220
JWT_KEY=qP9Jj9WnA0K3m8Xx6y4YlG8R2nT5sU7aB1cD3eF6hI=
IMAGEKIT_PRIVATE_KEY=private_dOY7SDziKUQEtTHn+AQzk0oklrQ=
```

## Запуск приложения

### Запуск через `dotnet run`

Для запуска приложения без использования Docker необходимо создать файл `environment.env` в директории `FitnessTracker.API` и поместить в него все необходимые переменные окружения.

После этого приложение можно запустить командой:

```bash
dotnet run --project FitnessTracker.API
```

После запуска API будет доступно по следующим адресам:

* HTTP: `http://localhost:5184`
* HTTPS: `https://localhost:7245`

---

### Запуск через Docker Compose

Для запуска приложения с использованием Docker необходимо создать файл `.env` в корневой директории проекта и поместить в него все необходимые переменные окружения.

После этого выполните команду:

```bash
docker compose up
```

После запуска API будет доступно по адресу:

* HTTP: `http://localhost:5184`

---

## Swagger UI

Документация API через Swagger UI доступна по адресу:

* При локальном запуске:

  * `http://localhost:5184/swagger`
  * `https://localhost:7245/swagger`

* При запуске через Docker:

  * `http://localhost:5184/swagger`
