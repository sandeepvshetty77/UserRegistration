# UserRegistration
This is a UserRegistration application module

The full code is in one application in order for it to be easily tested. It is currently in a ASP.NET Core MVC application as it provides the UI as well as the models for writing code logic. In the interest of time, to avoid lots of moving parts and to keep it simple, ASP.NET VC was chosen. 
Ideally the UI can be separated out maybe using Angular and there can be one User/Login web app for validating and generating JWT. This can act as the Authentication web app (if login is also included in it). The code can be reused to support that architecture. The html from the view can be inserted into the template in the Angular  Component. The MVC code can be moved into ASP.NET WebAPI.

Currently this just has the the user registration functionality. When a user registers,  it is checked if the user already exists. If so an error message is returned if not the user is registered and a JWT is generated and returned. The JWT has the username inside it. To verify that the JWT can be put through the JWT debugger in https://jwt.io/ and checked. The key can be found in the appSettings.json file.
After the user registers the JWT is returned and displayed on the page. This is done only for a quick verification. It is also stored in an httpOnlyCookie. It can be verified by using Developer tools (pressing F12 on the browser) -> Going to 'Applications'->Cookies. The name of the Cookie is 'JwtToken'.

In interest of time Unit tests have not been written, but the code is Unit Testable as it in .NET core which makes it easy to unit test. All the dependencies are specified as interfaces. Mock objects can be created to test the class methods. The JWT itself contains basic information and for the sake of verification has the claim for username which can be verified as stated above. More information can be added as needed.

The Source in GIT is complete and can be downloaded, built and run. I have run it using IISExpress to host it.
For database, The Migrations are uploaded instead of the full SQL database. The database can be recreated with the help of these migration files. 
Steps:
1. Open the project in Visual Studio.
2. Open Package Manager: Open Tools->Nuget Package Manager->Package Manager Console.
3. Type Update-Database. 
A 'UsersDB' should be created in your local SQL database inside (localDB)\MSSQLLocalDB. The connection string for this database is specified in the appSettings.json file. It will use Windows Authentication.
The database has just one table User which has Id, Username and Password.

