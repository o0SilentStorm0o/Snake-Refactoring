# Snake Refactoring

Tento projekt je refactoring jednoduché konzolové hry Snake v C#. Původní verze byla napsaná jako jeden větší blok kódu v metodě `Main`, kde byla dohromady herní logika, vykreslování do konzole, čtení klávesnice i práce se stavem hry.

Cílem refactoringu bylo zachovat původní chování hry, ale upravit kód tak, aby byl čitelnější, lépe rozdělený a dal se testovat bez spuštění konzolového rozhraní.

## Co se změnilo

- herní logika je oddělená od konzolového vstupu a výstupu,
- místo textových hodnot pro směr se používá `Direction`,
- pozice na herní ploše reprezentuje samostatný typ `Cell`,
- pravidla hry jsou soustředěná ve třídě `SnakeGame`,
- konzolové vykreslování řeší samostatná třída `ConsoleSnakeRenderer`,
- generování jídla je oddělené přes rozhraní `IFoodGenerator`,
- doplněné jsou unit testy pro základní pravidla hry.

## Struktura projektu

Projekt je rozdělený na tři části:

- `Snake.Core` - jádro hry, pravidla, stav hry a datové typy.
- `Snake.ConsoleApp` - konzolová aplikace, která zajišťuje vstup z klávesnice a vykreslení.
- `Snake.Tests` - testy herní logiky.

Díky tomuto rozdělení není hlavní logika závislá na třídě `Console`. To je podle mě nejdůležitější změna, protože pravidla hry jdou ověřovat samostatně a případné jiné rozhraní by nemuselo přepisovat celý projekt.

## Spuštění

Projekt používá .NET. Konzolovou hru lze spustit příkazem:

```bash
dotnet run --project src/Snake.ConsoleApp
```

Testy lze spustit takto:

```bash
dotnet test
```

## Poznámka k refactoringu

Nesnažil jsem se z původně malé hry udělat zbytečně složitou architekturu. Refactoring je zaměřený hlavně na smysluplné názvy, menší metody, oddělení odpovědností a možnost otestovat pravidla hry. Konzolová část zůstává jednoduchá, protože pro tento typ projektu je to podle mě dostačující.
