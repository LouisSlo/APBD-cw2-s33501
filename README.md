# Wypożyczalnia Sprzętu (Equipment Rental System)

Projekt zrealizowany w ramach przedmiotu APBD - Ćwiczenia 2.
Jest to aplikacja konsolowa modelująca uczelnianą wypożyczalnię sprzętu, napisana z naciskiem na strukturę kodu, dobre praktyki programowania obiektowego (SOLID) oraz czytelny podział odpowiedzialności.

## Instrukcja uruchomienia
1. Sklonuj repozytorium na swój dysk.
2. Otwórz rozwiązanie (plik `.sln`) w środowisku Rider lub Visual Studio.
3. Uruchom projekt (F5 / Ctrl+F5). 
4. W konsoli automatycznie wykona się predefiniowany scenariusz demonstracyjny prezentujący wszystkie główne funkcjonalności.

## Architektura i Decyzje Projektowe

Projekt został podzielony na trzy główne warstwy (katalogi), co pozwoliło oddzielić model danych od logiki wykonywania operacji:

### 1. Domain (Domena)
Zawiera czyste modele danych, które przechowują stan obiektów. 
- Wykorzystałem dziedziczenie (np. klasy `Laptop` i `Camera` dziedziczą po abstrakcyjnej klasie `Equipment`), ponieważ wynikało to bezpośrednio z modelu domeny – każdy typ urządzenia ma część wspólną (Id, Name, IsAvailable) oraz własne cechy specyficzne.

### 2. Application (Logika biznesowa)
- **Kohezja i SRP (Single Responsibility Principle):** Centralnym punktem aplikacji jest klasa `RentalService`. Jej jedyną odpowiedzialnością jest orkiestracja procesu wypożyczania i zwrotów. Została ona oddzielona od interfejsu konsolowego (`Program.cs`) oraz od mechanizmów przechowywania danych.
- **Niski Coupling (Luźne powiązanie):** Reguły biznesowe (limity wypożyczeń dla konkretnych typów użytkowników oraz sposób naliczania kar) nie zostały "zaszyte" w serwisie. Zamiast tego stworzyłem interfejsy `IUserLimitPolicy` oraz `IPenaltyCalculator`. `RentalService` korzysta z nich poprzez wstrzykiwanie zależności w konstruktorze. Dzięki temu modyfikacja reguł naliczania kary wymaga jedynie dopisania nowej klasy, bez zmieniania samego serwisu (zgodnie z zasadą OCP - Open/Closed Principle).

### 3. Infrastructure (Infrastruktura)
Zawiera klasy repozytoriów (np. `EquipmentRepository`, `RentalRepository`), które symulują bazę danych, przechowując obiekty w pamięci operacyjnej (w listach). Dzięki temu klasa realizująca logikę biznesową nie musi martwić się o to, jak i gdzie fizycznie zapisywane są dane.
