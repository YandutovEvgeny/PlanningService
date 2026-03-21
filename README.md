# Сервис Планирования

Сервис для построения плоской модели планирования на основе иерархических данных (SKU → SKUSub) с расчётом агрегатов и показателей роста. Реализован на **.NET 8**, **Entity Framework Core (InMemory)**. Основная особенность – расширяемый движок формул, использующий паттерны **Factory** и **Chain of Responsibility** для управления порядком вычислений.

## Архитектура

Проект следует принципам **Чистой архитектуры** (Clean Architecture) и разделён на слои:

- **PlanningService.Domain** – доменные сущности и интерфейсы репозиториев (порты).
- **PlanningService.Infrastructure** – реализация репозиториев, Entity Framework Core, контекст базы данных.
- **PlanningService.Application** – бизнес-логика: DTO, сервисы, движок формул и правила.
- **PlanningService.WebHost** – ASP.NET Core Web API, контроллеры, middleware.
- **PlanningService.Tests** – модульные и интеграционные тесты.

### Основные паттерны

| Паттерн | Применение |
|---------|------------|
| **Clean Architecture / Dependency Inversion** | Зависимости направлены от внешних слоёв к внутренним. Контроллеры зависят от сервисов приложения, сервисы – от абстракций репозиториев и движка формул. |
| **Chain of Responsibility** | Формулы (правила) организованы в цепочки. Каждая формула имеет чёткое место в последовательности для конкретного уровня (Total, SKU, SKUSUB) и колонки (UNITS, PRICE, AMOUNT). |
| **Factory** | `IRuleChainProvider` (фабрика) по запросу (уровень, колонка) возвращает упорядоченный список типов формул. |
| **Strategy** | Каждая формула (`IFormulaRule`) инкапсулирует конкретную вычислительную стратегию. |
| **Repository** | Абстракция доступа к данным через `IPlannerRepository`. |

## Движок формул

Движок реализован в классе `CalculationEngine`. Он получает через DI коллекцию всех правил (`IEnumerable<IFormulaRule>`) и фабрику цепочек (`IRuleChainProvider`). Во время расчёта:

1. Движок строит иерархию узлов (Total, SKU, SKUSUB).
2. Для каждого узла, каждой колонки и типа значения получает цепочку типов правил из `IRuleChainProvider`.
3. Последовательно применяет правила из цепочки, извлекая конкретные экземпляры из словаря `_rules`.

Цепочки настраиваются централизованно при регистрации сервисов:

```csharp
services.AddRuleChains(builder =>
{
    builder.For("Total", Column.Units)
           .AddRule<SkuSubPricePlanningY1Rule>()
           .AddRule<SkuSubAmountPlanningY1Rule>();

    builder.For("SKU", Column.Price)
           .AddRule<SkuSubAmountHistoryY0Rule>()
           .AddRule<SkuSubPriceHistoryY0Rule>();

    builder.For(Level.SkuSub, Column.ContributionGrowth)
           .AddRule<SkuSubAmountGrowthRule>()
           .AddRule<SkuSubPriceGrowthRule>()
           .AddRule<SkuSubUnitsGrowthRule>();
});
```
Такой подход позволяет легко добавлять новые формулы и переопределять порядок выполнения без изменения существующего кода.

## Инструкция по локальному запуску

Для локального запуска необходимо:

1. Клонировать репозиторий.
    ```bash
    git clone https://github.com/YandutovEvgeny/PlanningService.git
    cd PlanningService
    ```
2. Перейти в папку с WebHost проектом.
    ```bash
    cd src/PlanningService.WebHost
    ```
3. Восстановить и собрать пакеты.
    ```bash
    dotnet restore
    dotnet build
    ```
4. Запустить приложение.
    ```bash
    dotnet run
    ```
Сервер запуститься на `http://localhost:5001`.
Swagger будет доступен на `http://localhost:5001/swagger/index.html` 
