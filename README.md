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

#### Setting up the Database

1. Connect to your local SQL Server instance and create a database named `bluJournal`.
1. Create a `local.settings.json` file with the following contents:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "connectionString": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=bluJournal;Integrated Security=SSPI;"
  },
  "Host": {
    "LocalHttpPort": 7071,
    "CORS": "*"
  },
  "ConnectionStrings": {}
}

```

3. In `api/`, Run `dotnet ef database update` to apply database migrations:

### Building the UI

## License

[GNU](https://www.gnu.org/licenses/gpl-3.0.en.html)
