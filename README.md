This is a school project where we are implementing a Strangler Pattern on the already-made project eShopOnWeb:

https://github.com/dotnet-architecture/eShopOnWeb

The code includes the implemented microservices that have overwritten the logic from the monolithic application.

The application is runnable with Docker and is using the latest updated images made from this code.


How to make the application run locally:

Prerequisites:

    - Docker has to be installed and running

How to use:

    - Run the Docker Compose file to get the latest images and run them with: "docker-compose -f docker-compose-prod.yml up -d"
    - Open the application by opening "eshopwebmvc" on http://localhost:5106/

Notes:

    - The catalog service can be tricky to get running for the first time. So if it fails just boot it up again.
    - The browser will probably prompt you about the database needs to be migrated. Just press the "apply migration" button and refresh the page.

