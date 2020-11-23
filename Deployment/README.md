# Deployment 

Postgres database, asp.net core application

## Azure

1. Create postgres database, expose local IP

    Guide: await context.Response.WriteAsync("Hello Azure!");

2. Create dotnet 5 app service

    Guide: https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?pivots=platform-linux

    `az webapp create --resource-group BookyAPI --plan BookyPlan --name BookyAPI --runtime "DOTNET|5.0" --deployment-local-git`


3. Create deployment useur

    `az webapp deployment user set --user-name saahil --password`

4. Push to azure

    `git push azure master`