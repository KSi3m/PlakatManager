# Election Material Manager

This is a **REST API** designed to manage election materials, offering a range of functionalities.

## Features

- **Material Types:** Manage different types of election materials, including:
  - Billboards
  - Posters
  - LED displays
- **CRUD Operations:** Perform Create, Read, Update, and Delete operations for all materials.
- **JWT Authentication:** Secure user login through JWT tokens.
- **Comments:** Add comments to election items.
- **Location-based Election Items:** When adding a new election item, the city and district will be specified based on the provided geographical coordinates (for now it works for Lublin only)
- **Tags:** Add tags to election items
- **Statuses:** Assign statuses to election items


## Technologies Used

- **C#:** and **.NET Framework 8.0**
### Packages and Libraries:
- AutoMapper
- MediatR
- JwtBearer
- EntityFrameworkCore
- NetTopologySuite.IO.GeoJSON




## Installation
1. Clone the repository:
```bash
git clone https://github.com/KSi3m/PlakatManager.git
```
2. Navigate to the project directory:
```bash
cd Plakat-Manager
```
3. Restore the required packages:
```bash
dotnet restore
```
4. Build the project:
```bash
dotnet build
```
5. Run the application:
```bash
dotnet run
```

