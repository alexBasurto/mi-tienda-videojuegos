# Connection String:
Data Source=LOCALHOST;Initial Catalog=MiTiendaVideojuegos;Integrated Security=True;Trust Server Certificate=True

## Scaffold Con NoOnConfiguring:
Scaffold-DbContext "Data Source=localhost;Initial Catalog=MiTiendaVideojuegos;Integrated Security=True;Trust Server Certificate=True" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -Project WebApiTiendaVideojuegos -NoPluralize -NoOnConfiguring

