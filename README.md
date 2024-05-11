# System Zarządzania Wille

Ten projekt jest prostym systemem zarządzania willami, wykorzystującym C# WinForms do interfejsu użytkownika oraz ASP.NET Web API do komunikacji z bazą danych. Jest użyty API z poprzedniego projektu [WebApplication1](https://github.com/PavelShep/WebApplication1) 

## Opis

System umożliwia użytkownikowi wykonywanie następujących operacji:

- Przeglądanie listy willi
- Dodawanie nowej willi
- Aktualizowanie informacji o willi
- Usuwanie willi
- Wyszukiwanie willi po identyfikatorze

## Wymagania

- .NET Framework 4.7.2 lub nowszy
- Visual Studio (do tworzenia aplikacji WinForms i Web API)
- Sql Server

## Instalacja

1. Sklonuj 2 repozytorium na swój komputer:
```
git clone https://github.com/PavelShep/WebApplication1
git clone https://github.com/PavelShep/WindowsFormsApp2
```

2. Otwórz projekt w Visual Studio:

   - Dla WinForms: otwórz `WindowsFormsApp2.sln`.
   - Dla Web API: otwórz `WebApplication1I.sln`.

3. Skompiluj i uruchom projekty.

4.  W razie czego skonfiguruj string do bazy danych i url do API

## Użycie

1. Uruchom aplikację WinForms `WindowsFormsApp2`.
2. Korzystaj z interfejsu do przeglądania, dodawania, aktualizowania lub usuwania willi.
3. Aby wyszukać willę po identyfikatorze, wprowadź identyfikator w odpowiednie pole i kliknij przycisk "Szukaj".



