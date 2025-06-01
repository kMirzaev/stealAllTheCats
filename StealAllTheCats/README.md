# StealAllTheCats

A simple ASP.NET Core Web API that fetches cat data from [TheCatAPI](https://thecatapi.com/) and stores it in a SQL Server database. Supports tagging, paging, and duplicate prevention.

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express](https://aka.ms/sqlexpress) or any accessible SQL Server instance
- (Optional) [SQL Server Management Studio (SSMS)](https://aka.ms/ssms) for database management

---

## Setup Instructions

### 1. Clone the Repository

```sh
git clone <your-repo-url>
cd StealAllTheCats
```

### 2. Configure the Database Connection

Edit `appsettings.json` and set your SQL Server connection string.  
For SQL Server Express on your local machine, use:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=StealAllTheCatsDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

If using a different SQL Server, update the `Server` value accordingly.

---

### 3. Restore Dependencies

```sh
dotnet restore
```

---

### 4. Apply Database Migrations

This will create the database and tables:

```sh
dotnet ef database update
```

---

### 5. Build and Run the Application

```sh
dotnet build
dotnet run
```

---

### 6. Access the API Documentation

Open your browser and go to:

```
http://localhost:<port>/swagger
```

Replace `<port>` with the port shown in your terminal (e.g., `5000` or `5153`).

---

## Usage

### Fetch and Store Cats

- **POST `/api/cats/fetch`**  
  Fetches 25 cat images from TheCatAPI and stores them in the database.  
  No duplicate cats (by `catId`) will be stored.

### Retrieve a Cat by Cat API ID

- **GET `/api/cats/{catId}`**  
  Retrieves a cat by its external Cat API ID (e.g., `/api/cats/SpioNJPsd`).

### List Cats with Paging and Tag Filtering

- **GET `/api/cats?page=1&pageSize=10`**  
  Retrieves cats with paging support.

- **GET `/api/cats?tag=playful&page=1&pageSize=10`**  
  Retrieves cats with a specific tag and paging support.

### List All Tags

- **GET `/api/tags`**  
  Lists all tags in the database.

---

## Validation Rules

- `page` must be greater than 0.
- `pageSize` must be between 1 and 100.
- `catId` must be a non-empty string.

---

## Troubleshooting

- **File lock errors during build:**  
  Stop any running instance of the app before rebuilding.
- **SQL Server connection errors:**  
  Ensure SQL Server is running and your connection string is correct.
- **Pending model changes:**  
  Run `dotnet ef migrations add <MigrationName>` then `dotnet ef database update` if you change the models.

---

## Testing

To run the tests:

```sh
dotnet test
```

---

## Notes

- The API uses TheCatAPI. You may need an API key for higher rate limits.  
  Set it in your configuration if required.
- All endpoints are available and documented in Swagger UI.

---
