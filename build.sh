dotnet publish src/kube.sln -c Debug
docker build -t kube-gateway src/KubeGatewayHost/bin/Debug/netcoreapp3.0/
docker build -t kube-client src/KubeClient/bin/Debug/netcoreapp3.0/
