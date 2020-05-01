dotnet publish src/kube.sln -c Debug
docker build -t kube-silo src/KubeSilo/bin/Debug/netcoreapp3.0/
docker build -t kube-api src/KubeApi/bin/Debug/netcoreapp3.0/
