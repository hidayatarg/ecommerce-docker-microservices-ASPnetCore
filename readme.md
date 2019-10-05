Docker Images:
```docker
docker pull microsoft/mssql-server-linux
// Run the docker container with name catalogDB in port 1433 expose to outside
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=ProductApi(!)' -e 'MSSQL_PID=Express' -p 1433:1433 --name=catalogdb microsoft/mssql-server-linux
```