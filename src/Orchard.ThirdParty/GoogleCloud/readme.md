# Google Cloud

Google cloud is a neat way of hosting your aspnet web applications.

We have created some packages, which are tenant aware for you to use as well as a deployment script and yaml file.

To start... Go here https://cloud.google.com/ , Register, and lets get cracking!...

## Deploy

gcloud auth activate-service-account <SERVICEACCOUNT> --key-file <KEYFILENAME>
gcloud config set project <PROJECTNAME>
gcloud beta app deploy app.yaml -q
