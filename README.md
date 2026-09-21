# SoftwareVerification

Система учёта объектов верификации: ПО и программно-аппаратных комплексов.
Состоит из WPF-клиента, ASP.NET Core API и PostgreSQL.

## Структура решения

```
SoftwareVerification/
├── src/
│   ├── SoftwareVerification.Domain/          — сущности и enum'ы (VerificationObject, VerificationType, VerificationResult)
│   ├── SoftwareVerification.Infrastructure/  — EF Core, DbContext, конфигурации, миграции
│   ├── SoftwareVerification.Api/             — ASP.NET Core Web API (контроллеры, DTO)
│   └── SoftwareVerification.WpfClient/       — WPF-клиент (MVVM: View, ViewModel, Model)
├── docker-compose.yml
└── README.md
```

## Стек

- **.NET 9**
- **ASP.NET Core** — Web API
- **EF Core 9 + Npgsql** — доступ к PostgreSQL
- **PostgreSQL 16** — в Docker
- **WPF + MVVM** — десктопный клиент. `INotifyPropertyChanged` и `ICommand` реализованы вручную (`ObservableObject`, `RelayCommand`, `AsyncRelayCommand`) без сторонних MVVM-фреймворков.
## Требования

- **Docker Desktop** (с WSL2 на Windows)
- **.NET 9 SDK**
- **Visual Studio 2022** (или новее) с workload «.NET desktop development» и «ASP.NET and web development»

## Как запустить

### 1. Клонировать репозиторий

### 2. Поднять API и БД

Из **корня решения**:

```bash
docker compose up -d --build api postgres
```

API будет доступен по адресу: **http://localhost:8001**

### 3. Запустить WPF-клиент

Открыть решение `SoftwareVerification.sln` в Visual Studio.
Установить **`SoftwareVerification.WpfClient`** как стартовый проект.
Запустить.

Окно откроется и сразу подтянет список объектов с API.

## Параметры подключения

| Параметр | Значение |
|---|---|
| Хост | `localhost` |
| Порт | `5432` |
| База | `softwareverification` |
| Пользователь | `softwareverification` |
| Пароль | `devpass` |
| URL API | `http://localhost:8001` |

## API эндпоинты

| Метод | URL | Описание |
|---|---|---|
| GET | `/api/verification-objects` | Список с фильтрами (`name`, `type`, `result`) |
| GET | `/api/verification-objects/{id}` | Один объект по Id |
| POST | `/api/verification-objects` | Создать объект |
| PATCH | `/api/verification-objects/{id}` | Изменить `result` и `expertComment` |

Пример POST-запроса:

```json
{
  "name": "Тестовый образец",
  "version": "1.0.0",
  "type": "Software",
  "receivedAt": "2026-09-21T10:00:00Z",
  "expertComment": "Комментарий эксперта"
}
```

Доступные значения:
- `type`: `Software`, `HardwareSoftwareComplex`
- `result`: `InProgress`, `Compliant`, `NonCompliant`

## Возможные проблемы

### Порт 8001 занят

Windows может держать некоторые порты в резерве (Hyper-V, WSL2). Если при `docker compose up` ошибка `ports are not available`, поменять порт в `docker-compose.yml`:

```yaml
ports:
  - "8002:8080"   # вместо 8001:8080
```

И в `App.xaml.cs` WPF-клиента поменять URL API соответственно.

