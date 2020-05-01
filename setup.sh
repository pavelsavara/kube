kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.0.0/aio/deploy/recommended.yaml
kubectl apply -f k8/dashboard-adminuser.yaml
kubectl apply -f k8/dashboard-adminuser-bind.yaml
kubectl create namespace kube
kubectl config set-context --current kube

kubectl -n kubernetes-dashboard describe secret $(kubectl -n kubernetes-dashboard get secret | grep zamboch | awk '{print $1}')
kubectl proxy

kubectl run kube-silo --image=kube-silo:latest --namespace=kube --image-pull-policy=Never
kubectl run kube-gateway --image=kube-gateway:latest --namespace=kube --image-pull-policy=Never
kubectl run kube-client --image=kube-client:latest --namespace=kube --image-pull-policy=Never

kubectl get all