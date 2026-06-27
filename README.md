# Joyería Oro

## Proyecto Integrador – ISW-306

### Descripción

Joyería Oro es una aplicación web desarrollada en **ASP.NET Core MVC** para la gestión de una joyería. El sistema permite administrar productos, usuarios y ventas, utilizando **SQL Server** como base de datos para la persistencia de la información.

## Tecnologías utilizadas

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- Bootstrap
- iText7 (Generación de reportes PDF)
- Stripe (Integración de pagos)

## Requisitos

- Visual Studio 2022 o superior
- .NET 8 SDK
- SQL Server Express o SQL Server
- SQL Server Management Studio (SSMS)

## Instalación

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/anderson0698/joyeriaoro.git
   ```

2. Abrir la solución en Visual Studio.

3. Restaurar la base de datos ejecutando el archivo **JoyeriaDB.sql** en SQL Server.

4. Verificar la cadena de conexión en el archivo `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=DESKTOP-JV6CJD2\\SQLEXPRESS;Database=JoyeriaDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

5. Ejecutar el proyecto.

## Funcionalidades

- Inicio de sesión de usuarios.
- Registro de usuarios.
- Gestión de productos.
- Inventario de productos.
- Registro de ventas.
- Generación de facturas en PDF.
- Reporte de inventario en PDF.

## Base de Datos

La base de datos utilizada es **JoyeriaDB** en SQL Server.

El script de creación se encuentra en el archivo:

- `JoyeriaDB.sql`

## Autor

**Anderson De La Rosa Lebrón**