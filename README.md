# Revenue-Recognition

This is my university project as a final project for course that teach ASP.NET.
It's a RESTful API built using ASP.NET Core with Entity Framework Core using the Code-First approach. It allows users to do revenue recognition.

The repository also consist of:
- Project.pdf file which includes all the requirements
- Swagger is available to discover enpoints
- DatabaseDiagram.png for database structure discovery

## Technologies Used

- **ASP.NET Core** 
- **Entity Framework Core** (Code-First)
- **SQL Server** (or any other database provider, e.g., PostgreSQL, SQLite)
- **Swagger** for API documentation
- **xUnit** 

## Getting Started

### 1. Clone the Repository

First, clone the repository from GitHub:

```bash
git clone https://github.com/your-username/your-project.git
cd your-project
```

### 2. Install Dependencies
Restore the NuGet packages:

```bash
dotnet restore
```

### 3. Set Up Environment Variables
Create an appsettings.json file in the root of the project or use the existing appsettings..json file to configure your database connection string and other environment settings.

Here’s an example of the appsettings.json file for SQL Server:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=your_data_source;Initial Catalog=your_catalog;TrustServerCertificate=True;Integrated Security=True;"
  },
  "AllowedHosts": "*",
  "SecretKey": "your_secret_key"
}
```


###. Migrate the Database
This project uses Entity Framework Core with a Code-First approach. To set up the database, follow these steps:

Run the following command to apply migrations and create the database:

```bash
dotnet ef database update
```

This will automatically create the database schema from the models in the project.

### 6. Run the Application

You can run the application using the following command:

```bash
dotnet run
```

Or run it via Visual Studio by pressing F5.


