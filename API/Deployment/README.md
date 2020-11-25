# Deployment 

Postgres database, asp.net core application

## Azure

1. Create postgres database, expose local IP

    Guide: https://docs.microsoft.com/en-us/azure/postgresql/connect-python

    Note: I am choosing the preview autoscaling option for the postgres database.
    This (hopefully) will be cheaper for this API as I am developing.

    To run migrations, either launch a shell from azure, or connect to the remote database locally by setting the variable `ConnectionStrings__BookyDatabase`.
    This will let the local service connect to the remote database.

2. Create dotnet 5 app service

    Guide: https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?pivots=platform-linux

    `az webapp create --resource-group BookyAPI --plan BookyPlan --name BookyAPI --runtime "DOTNET|5.0" --deployment-local-git`

    This is similar to a `heroku push` deployment. 


3. Create deployment user

    `az webapp deployment user set --user-name saahil --password <omitted>`

    TOOD: figure out how keys work (instead of typing my password)

4. Push to azure

    `git push azure master`