# Google Cloud

Google cloud is a neat way of hosting your aspnet web applications.

We have created some packages, which are tenant aware for you to use as well as a deployment script and yaml file.

To start... Go here https://cloud.google.com/ , Register, and lets get cracking!...

## Configuration

Configuration is done directly in your tenant file, i.e. settings.txt or tenants.json. Here is an example of what it looks like in tenants.json

```json
{
    "default": {
        "googlecloud": {
            "credentials": {
                # JSON HERE FOR SERVICE ACCOUNT ACCESS
            }
        }
    }
}
```

## Deploy

dotnet restore
dotnet publish -c Release
gcloud auth activate-service-account <SERVICEACCOUNT> --key-file <KEYFILENAME>
gcloud config set project <PROJECTNAME>
gcloud beta app deploy app.yaml -q

## Usages
### Tracing

```c#
if (_diagnostics.IsEnabled("Orchard.GoogleCloud.Diagnostics.TraceStarted"))
{
    _diagnostics.Write(
        "Orchard.GoogleCloud.Diagnostics.TraceStarted",
        new
        {
            name = "A Name Here!"
        });


    // DO SOMETHING

    _diagnostics.Write("Orchard.GoogleCloud.Diagnostics.TraceFinished");
}
```

Orchard.GoogleCloud.Diagnostics.TraceStarted
Orchard.GoogleCloud.Diagnostics.TraceFinished