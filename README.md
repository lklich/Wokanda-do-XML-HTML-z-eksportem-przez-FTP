# Wokanda-do-XML-HTML-z-eksportem-przez-FTP
System pobierania wokand z systemu SAWA do XML/HTML i przesyłania na hosting przez FTP

Program pobiera dane wokand z systemu SAWA (Wydziały: Cywilny, Karny, Rodzinny, Pracy, Penitencjarny) tworząc plik XML z każdej bazy. Po utworzeniu plików XML, wysyła je na serwer FTP w celu umieszczenia ich na E-wokandzie. 
Program można wywoływać cyklicznie w Harmonogramie zadań.

Struktura pliku wynikowego:
<?xml version="1.0" encoding="WINDOWS-1250"?>
<Rozprawy>
<Rozprawa>
<Sygnatura>Ns 144/2014</Sygnatura>
<Data>2014-06-30</Data>
<Strony> </Strony>
<Sala>8</Sala>
<Wydział>Wydział Cywilny</Wydzial>
<Godzina>08:30</Godzina>
<SygnaturaProkuratury> </SygnaturaProkuratury>
</Rozprawa>
</Rozprawy>

Technologia i wymagania
Program na konsolę, napisany w technologii .NET Framework, w języku C#. Do uruchomienia wymagany jest .NET Framework 4.0. Powinien działać na systemach, na których dostępny jest .NET Framework w wersji 4.0, czyli także w Windows XP.
Konfiguracja

Przed uruchomieniem programu należy dokonać konfiguracji. Konfiguracja zapisana jest w pliku LKWokanda.exe.config. Jest to plik XML, który można konfigurować w notatniku, bądź wizualnie (GUI). Wywołanie konfiguracji wizualnej odbywa się za pomocą przełącznika /k
LKWokanda /k
 
Nazwa wydziału – łańcuch połączenia dla każdej bazy systemu SAWA. Program uwzględnia wydział, oraz jego II instancję (w przypadku SO).

Można stosować typ autoryzacji w domenie: 
Data Source=nazwa_serwera_wraz_z_instancją; Initial Catalog=nazwa_bazy_danych;Integrated Security=True, lub poprzez nazwę uzytkownika i hasło: Data Source=nazwa_serwera_wraz_z_instancją; Initial Catalog=nazwa_bazy_danych;User ID=nazwa_uzytrkownika_bazy;Password=haslo_do_bazy_danych

Import – zaznaczenie opcji przy każdym wydziale oznacza, że program będzie importował dane z zaznaczonego wydziału.

Nazwa pliku XML – nazwa pliku wynikowego XML, do którego program będzie importował dane z bazy danych.

FTP – zaznaczenie opcji oznacza wysyłkę pliku XML na serwer FTP (UWAGA! – przed zaznaczeniem kórejś z tych opcji, należy uprzednio dokonać konfiguracji serwera FTP, gdyż w przeciwnym wypadku program będzie generował błędy, z powodu błędu połaczenia z serwerem).

Twórz loga – zaznaczenie opcji oznacza, że program będzie tworzył plik log.txt, w którym będą zapisywane uruchomienia programu oraz ewentualne błędy.

Parametry FTP – ustawianie parametrów serwera FTP, na który przesyłane będą pliki XML.

Zapisz i zamknij – Przycisk zapisuje ustawienia w pliku konfiguracyjnym LKWokanda.exe.config i kończy działanie programu.
