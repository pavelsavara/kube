kubectl run kube-silo --image=kube-silo:latest --namespace=kube --image-pull-policy=Never
kubectl run kube-api --image=kube-api:latest --namespace=kube --image-pull-policy=Never
