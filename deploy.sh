kubectl run kube-gateway --image=kube-gateway:latest --namespace=kube --image-pull-policy=Never
kubectl run kube-client --image=kube-client:latest --namespace=kube --image-pull-policy=Never
