# exercise-example
This repo contains example code for a classroom example for integrating Akka with ASP. 

To start the ClusterBackend, run in terminal:
```
dotnet run ClusterBackend.csproj /port=12000
dotnet run ClusterBackend.csproj /port=12001
```
To start asp app, run in terminal:
```
dotnet run RS_01.csproj
```
TO test once everything is started, go to api:

https://localhost/api/students/1
