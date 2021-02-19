# GlobalIMC

It contains three projects : \
1 - Restful API .Net Core . \
2 - Web Application .Net Core . \
3 - Shared .Net Core Library . \

# Details

### Restful API .Net Core :

It has the Database connections and operations using Entity Framework Core - Code First, uses Dependency Injection and services to handle API calls.
Uses the DTO (Data Transfer Object) that placed in the Shared Liberary, DTO casting was done by AutoMapper which Maps the DTO with the moldel without any hard work.
In addintion to handle the database operations using EF core, it stores Images that it recieves from requsets and map them.

### Web Application .Net Core :

It's a SAP(Single Page Application) that connects to the previous API to get/manage data using Net.Http and Newtonsoft packages, and uses the AutoMapper to handle DTO and ViewModle Casting.
Manages the CRUD operations through the API and applies Search and indexing items .
some Javascript, JQuery, HTML, CSS and Razor Helped with FrontEnd operations .

### Shared .Net Core Library :

Contains the DTO and the response structure to be recognized by both API and Application.



### Tools :
- Database : SQLServer 2019
- IDE : Visual Studio 2019
- .Net Version : .Net Core 3.1


### Publish :
Sadly I couldn't publish on Azure because I don't have an account but I published on another hosts and here the links : \
API : http://yamenagha766-001-site1.itempurl.com/ \
APP : http://yamenagha767-001-site1.itempurl.com/
