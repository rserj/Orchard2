Deploy....

gcloud auth activate-service-account <SERVICEACCOUNT> --key-file <KEYFILENAME>
gcloud config set project <PROJECTNAME>
gcloud beta app deploy app.yaml -q
