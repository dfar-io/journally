# bluJournal

bluJournal is a minimalist journal web application meant to provide a means for jotting down your thoughts.

## Access

You can [access bluJournal online](https://blujournal.com).

## Contributing

After pulling down the source for the project, you can follow the directions
below to build the two major parts of the project.

### Building the API

Requirements:

- SQL Server Express
- .NET Core 3.0 SDK
- EF Core tools (`dotnet tool install --global dotnet-ef`)

1. Connect to your local SQL Server instance and create a database named `bluJournal`.
1. Create a `local.settings.json` file with the following contents:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "connectionString": "Data Source=.\\SQLEXPRESS;Initial Catalog=bluJournal;Integrated Security=SSPI;"
  },
  "Host": {
    "LocalHttpPort": 7071,
    "CORS": "*"
  },
  "ConnectionStrings": {}
}

```

3. Create an environment variable `BLUJOURNAL_CONN_STR` with the value:
   `Data Source=.\SQLEXPRESS;Initial Catalog=bluJournal;Integrated Security=SSPI;`
4. Create an environment variable `BLUJOURNAL_JWT_SECRET` with a value generated from https://mkjwk.org/.
5. In `api/`, Run `dotnet ef database update` to apply database migrations.
6. Use the following commands to build and start the API:

```
dotnet build
func host start
```

### Building the UI

## License

[GNU](https://www.gnu.org/licenses/gpl-3.0.en.html)
