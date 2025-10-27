# HRCommerce App - Sistema de Gestión Integral

## Descripción
Sistema web completo para gestión de recursos humanos y módulo comercial desarrollado en ASP.NET Core con arquitectura en capas.

## Características

### Módulo RR.HH.
- CRUD completo de Empleados
- CRUD completo de Departamentos  
- Historial salarial
- Validaciones y auditoría

### Sistema de Autenticación
- Roles: Administrador y Operador
- JWT para API + Cookies para MVC
- Protección por roles

### Arquitectura
- ASP.NET Core 8.0 + Entity Framework Core
- SQL Server LocalDB
- Repository Pattern + Service Layer
- API RESTful + MVC Frontend

## Prerrequisitos

- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server LocalDB (viene con VS)

## Instalación

1. **Clonar repositorio**
   ```bash
   git clone https://github.com/Guille-X/HRCommerceApp.git
   cd HRCommerceApp

2. **Restaurar paquetes**
    dotnet restore

3. **Ejecutar migraciones (Package Manager Console)**
    Update-Database

4. **Configurar startup múltiple88**
    HRCommerceApp.API
    HRCommerceApp.Web

5. **Ejecutar con F5**

Usuarios de Prueba
Administrador:

Email: admin@hrcommerce.com

Password: Admin123!

Operador:

Email: operador@hrcommerce.com

Password: Operador123!

Estructura
HRCommerceApp/
├── HRCommerceApp.API/          # Web API
├── HRCommerceApp.Web/          # Frontend MVC  
├── HRCommerceApp.Core/         # Modelos y DTOs
├── HRCommerceApp.Infrastructure/ # Data Access
├── HRCommerceApp.Services/     # Business Logic
└── HRCommerceApp.Tests/        # Unit Tests

**Soporte**
guillermoajsivinac@gmail.com

