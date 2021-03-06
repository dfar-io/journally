![](https://github.com/dfar-io/journally/workflows/Build%20&#x26;%20Deploy/badge.svg)

# Journally

Journally is a minimalist journal web application meant to provide a means for jotting down your thoughts.

## Access

You can [access Journally online](https://journally.io).

## Contributing

After pulling down the source for the project, you can follow the directions
below to build the two major parts of the project.

### Building the API

Requirements:

- SQL Server Express
- .NET Core 3.0 SDK
- EF Core tools (`dotnet tool install --global dotnet-ef`)

1. Connect to your local SQL Server instance and create a database named `journally`.
1. Create a `local.settings.json` file with the following contents:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "connectionString": "Data Source=.\\SQLEXPRESS;Initial Catalog=journally;Integrated Security=SSPI;"
  },
  "Host": {
    "LocalHttpPort": 7071,
    "CORS": "*"
  },
  "ConnectionStrings": {}
}

```

3. Create an environment variable `JOURNALLY_CONN_STR` with the value:
   `Data Source=.\SQLEXPRESS;Initial Catalog=journally;Integrated Security=SSPI;`
4. Create an environment variable `JOURNALLY_JWT_SECRET` with a value generated from https://mkjwk.org/.
5. Use the following commands to build and start the API:

```
dotnet build
func host start
```

### Building the UI

Requirements:

- Node/NPM
- Angular CLI

1. Run `npm i`
1. Run the UI using `ng serve -o`

## License

[GNU](https://www.gnu.org/licenses/gpl-3.0.en.html)
