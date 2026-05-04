# NGO Finance Dashboard

Desktopová WinForms aplikace pro sledování transparentního bankovního účtu Fio banky. Aplikace načítá transakce z veřejně přístupné stránky transparentního účtu, analyzuje je a zobrazuje přehledný dashboard s klíčovými finančními ukazateli.

---

## Funkčnost

Po spuštění uživatel vloží URL transparentního účtu Fio banky do textového pole a stiskne tlačítko pro načtení. Aplikace provede validaci URL, načte stránku přes headless prohlížeč, zparsuje HTML tabulku transakcí a zobrazí:

- **celkový cash flow** (součet všech transakcí),
- **největší výdaj** (transakce s nejnižší zápornou hodnotou),
- **top 3 přispěvatelé** (odesílatelé s největšími příchozími platbami),
- **nejčastější zprávy** (nejopakovanější textové zprávy u transakcí),
- **tabulku všech transakcí** s barevným rozlišením příjmů (zelená) a výdajů (červená).

Příklad validního URL: `https://ib.fio.cz/ib/transparent?a=2200272480`

---

## Architektura

Projekt je rozdělen do čtyř vrstev se striktním oddělením zodpovědností:

```
NGOFinanceDashboard/
├── Forms/
│   └── MainForm.cs          # UI vrstva – obsluha
│   └── MainForm.Designer.cs             - zobrazení 
├── ViewModels/
│   └── ViewModel.cs         # Orchestrace – propojuje UI se službami
├── Models/
│   ├── Transaction.cs       # Datový model jedné transakce
│   └── FinanceReport.cs     # Výsledek analýzy předávaný do UI
├── Services/
│   ├── IDataFetcher.cs      # Rozhraní pro načítání dat
│   ├── IDataAnalyzer.cs     # Rozhraní pro analýzu dat
│   ├── IFioParser.cs        # Rozhraní pro parsování HTML
│   ├── FioDataFetcher.cs    # Implementace: načítání přes Playwright
│   ├── FioHtmlParser.cs     # Implementace: parsování HTML tabulky
│   └── FinanceAnalyzer.cs   # Implementace: výpočty a agregace
└── Utilities/
    ├── CustomExceptions.cs  # Hierarchie vlastních výjimek
    └── ExceptionHandler.cs  # Centralizované logování chyb
```

### SOLID principy

**Single Responsibility Principle** – každá třída má jednu jasnou zodpovědnost. `FioDataFetcher` pouze stahuje HTML, `FioHtmlParser` pouze parsuje, `FinanceAnalyzer` pouze počítá.

**Open/Closed Principle** – novou implementaci fetcheru (např. přes REST API Fio) lze přidat implementací `IDataFetcher` bez zásahu do existujícího kódu.

**Dependency Inversion Principle** – `FioDataFetcher` závisí na rozhraní `IFioParser`, ne na konkrétní třídě `FioHtmlParser`. `MainFormViewModel` závisí na `IDataFetcher` a `IDataAnalyzer`. Závislosti jsou předávány přes konstruktor (constructor injection).

```csharp
// FioDataFetcher závisí na abstrakci, ne na konkrétní implementaci
public FioDataFetcher(HttpClient httpClient, IFioParser parser)
{
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _parser = parser ?? throw new ArgumentNullException(nameof(parser));
}
```

### ViewModel a události (Events)

`MainFormViewModel` slouží jako prostředník mezi UI a business logikou. UI (MainForm) se přihlásí k odběru událostí a reaguje na změny stavu – tím se eliminuje přímá závislost ViewModelu na WinForms třídách:

```csharp
// ViewModel vyvolá událost, MainForm na ni zareaguje
public event EventHandler<(string Message, Color Color)>? StatusChanged;
public event EventHandler? DataAnalyzed;

// MainForm se přihlásí v konstruktoru
_viewModel.StatusChanged += (s, e) => UpdateProgress(e.Message, e.Color);
_viewModel.DataAnalyzed += (s, e) => DisplayAnalysis();
```

Výsledek analýzy je zapouzdřen do objektu `FinanceReport`, který ViewModel vrací metodou `GetAnalysis()` – místo tuple nebo výstupních parametrů.

---

## Asynchronní zpracování

Načítání dat z internetu probíhá asynchronně, aby nedošlo k zamrznutí UI.

Rozhraní `IDataFetcher` deklaruje metodu s podporou `CancellationToken`:

```csharp
Task<IEnumerable<Transaction>> FetchTransactionsAsync(
    string fioAccountUrl, CancellationToken cancellationToken = default);
```

V `MainForm` je použit `_isLoading` guard, který zabraňuje vícenásobnému odeslání požadavku při opakovaném kliknutí na tlačítko:

```csharp
private async void FetchButton_Click(object? sender, EventArgs e)
{
    if (_isLoading) return;
    _isLoading = true;
    try {
        await _viewModel.FetchAndAnalyzeAsync(this.urlTextBox.Text);
    }
    finally {
        _isLoading = false; // vždy se odblokuje, i při výjimce
    }
}
```

`HttpClient` je sdílen přes `Lazy<HttpClient>`, aby nedocházelo k opakovanému vytváření instancí (socket exhaustion). Logování chyb využívá `File.AppendAllTextAsync`, takže zápis do logu neblokuje UI vlákno.

### Playwright – proč headless prohlížeč?

Stránka transparentního účtu Fio banky vyžaduje JavaScript pro vykreslení tabulky transakcí. Pro zachování čistého asynchronního přístupu je použit Playwright, který spouští Chromium v headless režimu, počká na síťový klid (`NetworkIdle`) i na přítomnost tabulky v DOM, a teprve poté předá HTML parseru:

```csharp
await page.GotoAsync(fioAccountUrl, new PageGotoOptions {
    WaitUntil = WaitUntilState.NetworkIdle
});
await page.WaitForSelectorAsync("//table[contains(@class,'table')]/tbody/tr");
var pageSource = await page.ContentAsync();
return _parser.ParseRawHtml(pageSource);
```

---

## Výjimky

### Hierarchie vlastních výjimek

Všechny výjimky aplikace dědí z abstraktní třídy `DashboardException`, která rozšiřuje standardní `Exception` o dvě vlastnosti:

- `UserFriendlyMessage` – zpráva zobrazená uživateli v UI (česky, srozumitelně),
- `LogDetails` – technický detail určený pro log soubor.

```
DashboardException (abstraktní)
├── ValidationException       – prázdný nebo chybně formátovaný vstup
├── InvalidURLException       – URL nepatří transparentnímu účtu Fio
├── DataFetchException        – selhání při stahování stránky (Playwright)
├── HtmlParsingException      – změna struktury HTML tabulky Fio
└── DataAnalysisException     – chyba při výpočtech nad transakcemi
```

### Propagace výjimek

Výjimky vznikají v nejnižší vrstvě (parser, fetcher) a jsou propagovány nahoru až do ViewModelu, kde se jednotlivě zachytí a přeloží na stavovou zprávu pro UI. UI se tím vůbec nestará o to, jaký typ výjimky nastal – dostane jen finální zprávu:

```csharp
// ViewModel – zachycení podle typu, každý typ má jinou reakci
catch (ValidationException ex) {
    UpdateStatus($"⚠️ {ex.UserFriendlyMessage}", Color.Orange);
}
catch (DataFetchException ex) {
    UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
}
catch (DashboardException ex) {
    UpdateStatus($"❌ {ex.UserFriendlyMessage}", Color.Red);
}
catch (Exception ex) {
    UpdateStatus("❌ Unknown error", Color.Red);
}
```

### Centralizované logování

`ExceptionHandler` je statická třída, která centralizuje zápis chyb do souboru `logs/error.log` umístěného vedle spustitelného souboru. Každý záznam obsahuje časové razítko, kontext (název metody), typ výjimky, zprávu, stack trace a inner exception:

```
[2025-05-04 14:23:01] [FioDataFetcher.FetchTransactionsAsync] [PlaywrightException] net::ERR_NAME_NOT_RESOLVED
StackTrace: ...
--------------------------------------------------------------------------------
```

Třída nabízí tři úrovně: `HandleException` (chyba), `LogInfo` a `LogWarning`.

---

## Instalační požadavky

Aplikace cílí na **.NET 10 (Windows)** a vyžaduje Windows kvůli WinForms.

Po prvním buildu je potřeba nainstalovat Playwright prohlížeč:

```powershell
# Ve složce projektu po buildu
pwsh bin/Debug/net10.0-windows/playwright.ps1 install chromium
```

### Závislosti (NuGet)

| Balíček | Verze | Účel |
|---|---|---|
| Microsoft.Playwright | 1.59.0 | Headless browser pro JS rendering |
| HtmlAgilityPack | 1.11.56 | Parsování HTML tabulky transakcí |

---

## Validace URL

Před každým požadavkem se URL validuje ve dvou krocích:

1. Prázdný řetězec → `ValidationException`
2. Strukturální kontrola přes `Uri.TryCreate` – musí jít o absolutní URL s hostitelem končícím na `fio.cz`, cestou obsahující `/ib/transparent` a query parametrem `a=` → jinak `InvalidURLException`

```csharp
if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
    !uri.Host.EndsWith("fio.cz") ||
    !uri.AbsolutePath.Contains("/ib/transparent") ||
    !uri.Query.Contains("a="))
    throw new InvalidURLException(...);
```
