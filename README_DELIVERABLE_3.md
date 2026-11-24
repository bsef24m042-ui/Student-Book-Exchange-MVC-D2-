# Student Book Exchange — Deliverable 3

This project builds on Deliverable 2 and keeps the existing ASP.NET Core MVC, SQL Server, and ASP.NET Core Identity foundation.

## Deliverable 3 features

1. **AJAX with jQuery**
   - Live book search calls `GET /Books/Search` and refreshes the results without reloading the page.
   - Book deletion uses an anti-forgery-protected AJAX `POST /Books/DeleteAjax`.
2. **Dapper**
   - Book listing, search, create, update, delete, and seller-specific queries use Dapper.
   - ASP.NET Core Identity continues to use Entity Framework Core.
3. **Repository Pattern and Dependency Injection**
   - `IBookRepository` / `BookRepository`
   - `IDashboardRepository` / `DashboardRepository`
   - `IUserManagementRepository` / `UserManagementRepository`
   - All three repositories are registered in `Program.cs`.
4. **Customized Identity**
   - Public registration assigns the Student role automatically.
   - Admin and Student roles are seeded at startup.
   - Password complexity, unique email, lockout, login path, and access-denied path are configured.
   - Registration stores the user's display name as an Identity claim.
   - Admin can view users and change their role.
5. **Layout and navigation**
   - Shared responsive Bootstrap layout.
   - Navigation changes according to authentication and role.
6. **SignalR live updates**
   - `/bookHub` broadcasts book listing changes.
   - Open Browse Books pages refresh their results when a listing is added, edited, or deleted.
7. **IIS deployment**
   - See `DEPLOYMENT_IIS.md`.

## Requirements

- Visual Studio 2022
- .NET 8 SDK
- SQL Server LocalDB (or SQL Server / SQL Server Express)
- Internet access for the SignalR JavaScript client CDN on the Browse Books page

## Run locally

1. Open `Deliverable 2_StudentBookExchangeMVC.sln` in Visual Studio.
2. Check `appsettings.json` and confirm the `DefaultConnection` points to your SQL Server instance.
3. Open **Tools → NuGet Package Manager → Package Manager Console**.
4. Run:

   ```powershell
   Update-Database
   ```

   The existing Deliverable 2 migrations create the Identity and Books tables. Deliverable 3 does not add a new database table.
5. Build the solution and press **F5**.

## Demo administrator

- Email: `admin@studentbookexchange.com`
- Password: `Admin@12345`

This is a classroom demonstration account. Change the seeded password before any real deployment.

## Important notes

- Dapper uses the same `DefaultConnection` as Entity Framework Core.
- The Books table and Identity tables must exist before running the application.
- The local database connection string is machine-specific. A different computer may need a different SQL Server instance name.
