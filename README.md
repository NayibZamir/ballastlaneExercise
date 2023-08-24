# BallastlaneExercise

The project contains an API to managemnet of books.

## GIT repository
https://github.com/NayibZamir/ballastlaneExercise.git


## Requeriments
- Build an application for managing books, it must allow you to search, create, update and delete books from your catalog.
- Book search can be done anonymously, but catalog modification operations are only allowed for authenticated users.
- The application must be built to allow interoperability using REST API technologies.

## Development

 - The application was built on a multilayer architecture, which includes 3 layers plus the persistence layer that is occupied by a SQL server database.
 - The next layer is data access (Crud.Data), a micro ORM called Dapper was used for this, and the interfaces with the necessary interactions with the database were created.
 - The business layer was also built where the application models were concentrated, as well as the rules and logic dictated by the design.
 - The APIs are concentrated in the last layer, and it is where there are the entry points for the interaction with the application.
 - A transversal authentication component was also created, which uses JwtToken to manage the authentication and security of the application. 
 - Unit tests were created for the components.

## Features

- Rest API
- Swagger
- Docker
- Cross platform
- SQL server

## Deployment

- For easy deployment it was created using docker-compose, allowing to launch images both to host the application as well as the database server.
- Local debugging is simple you should just use the docker profile and launch it from Visual Studio



