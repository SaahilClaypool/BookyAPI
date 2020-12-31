# Booky API

Link: https://bookyapi.azurewebsites.net/login

Goal: simple rest API to create bookmarks with url, content, and tags (maybe personal notes).

- Hosted on Azure App service (Free tier)
- Database hosted on https://api.elephantsql.com/ (Free tier)

## Resources

- partial inspiration: [Memex](https://github.com/WorldBrain/Memex)

## Feature List

- [x] CRUD for simple bookmarks
- [x] Search-on-type
    - [ ] Improve back end with Postgres text search
- [ ] Bookmark should have many notes (which are optional text fragment + comments)
- [ ] Tags
- [ ] Paging ala https://use-the-index-luke.com/no-offset
- [ ] Chrome extension to snip page content using https://github.com/mozilla/readability
    - [ ] UI would need to support longer articles with summaries 
    - [ ] Links should use text fragements to link to content

## Connecting to https://api.elephantsql.com/

(this is the easiest free PSQL tier I could find).

See [Deployment](./Deployment/README.md)


## Connect to azure

See [Deployment](./Deployment/README.md)

Deployed at [azuresites](https://bookyapi.azurewebsites.net/)

## Connecting to AWS

> NOTE: opting to *not* go with AWS. The steps would be something as below.
> I believe the analogue for AWS is [elastic beanstalk](https://docs.aws.amazon.com/elasticbeanstalk/latest/dg/dotnet-core-tutorial.html).
> Or just creating a docker image and deploying via a container.
