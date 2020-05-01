dotnet publish src/kube.sln -c Debug -o ./bin/PublishOutput
docker build -t kubesilo src/KubeClient/bin/Debug/netcoreapp3.0/