helm upgrade dev-release ./chart/
helm install dev-release ./chart/
kubectl port-forward service/dev-release-service 5000:5000