# ProjectManager
First create a local mssql database called ProjectManagerDB via sql server object explorer in catalog with name (localdb)\MSSQLLocalDB. 
Before starting the application, you need to open the ProjectManagerDBcreate.sql file through visualstudio and run the script in it to create tables Employees,Projects and EmployeeProjects in existing database.
After successfully creating the database via the PackageManager Console, issue the Update-Database command to update the database via the migrations file.
After all, you can run the application for testing.
