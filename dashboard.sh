kubectl -n kubernetes-dashboard describe secret $(kubectl -n kubernetes-dashboard get secret | grep zamboch | awk '{print $1}')
kubectl proxy
