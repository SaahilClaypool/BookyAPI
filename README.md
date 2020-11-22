# Booky API

Goal: simple rest API to create bookmarks with url, content, and tags (maybe personal notes).


## Connect to azure

- DB: `https://portal.azure.com/#blade/Microsoft_Azure_MonitoringAz/MetricsV4/ResourceId/%2Fsubscriptions%2F54eee83c-d10a-4585-bd01-8fe47410cf90%2FresourceGroups%2FBookyAPI%2Fproviders%2FMicrosoft.DBforPostgreSQL%2FflexibleServers%2Fbookydb/TimeContext/%7B%22options%22%3A%7B%22grain%22%3A1%7D%2C%22relative%22%3A%7B%22duration%22%3A3600000%7D%7D/Chart/%7B%22metrics%22%3A%5B%7B%22resourceMetadata%22%3A%7B%22id%22%3A%22%2Fsubscriptions%2F54eee83c-d10a-4585-bd01-8fe47410cf90%2FresourceGroups%2FBookyAPI%2Fproviders%2FMicrosoft.DBforPostgreSQL%2FflexibleServers%2Fbookydb%22%7D%2C%22name%22%3A%22cpu_percent%22%2C%22aggregationType%22%3A4%2C%22metricVisualization%22%3A%7B%22displayName%22%3A%22CPU%20percent%22%2C%22resourceDisplayName%22%3A%22bookydb%22%7D%7D%2C%7B%22resourceMetadata%22%3A%7B%22id%22%3A%22%2Fsubscriptions%2F54eee83c-d10a-4585-bd01-8fe47410cf90%2FresourceGroups%2FBookyAPI%2Fproviders%2FMicrosoft.DBforPostgreSQL%2FflexibleServers%2Fbookydb%22%7D%2C%22name%22%3A%22storage_percent%22%2C%22aggregationType%22%3A4%2C%22metricVisualization%22%3A%7B%22displayName%22%3A%22Storage%20percent%22%2C%22resourceDisplayName%22%3A%22bookydb%22%7D%7D%5D%2C%22title%22%3A%22Resource%20utilization%20(bookydb)%22%2C%22titleKind%22%3A2%2C%22timespan%22%3A%7B%22relative%22%3A%7B%22duration%22%3A3600000%7D%2C%22grain%22%3A1%2C%22showUTCTime%22%3Afalse%7D%2C%22visualization%22%3A%7B%22chartType%22%3A2%7D%7D/openInEditMode/`
- Guide: https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?pivots=platform-linux
- https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?pivots=platform-linux

## Connecting to AWS

- Need to add https://console.aws.amazon.com/vpc and create a security group that allows postgres connections

    Can whitelist just "MyIp"

https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?pivots=platform-linux