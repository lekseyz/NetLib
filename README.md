# Библиотека на ASP MVC Framework

## Запуск
Если выбрасывается исключение `System.DllNotFoundException: Не удается загрузить DLL "SQLite.Interop.dll": Не найден указанный модуль (0x8007007E)`, то необходимо переустановить `System.Data.SQLite.Core` модуль.
В NuGet терминале:
```
Update-Package System.Data.SQLite.Core -Reinstall
```
