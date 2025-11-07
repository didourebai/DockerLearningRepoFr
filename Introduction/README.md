# Lignes de commandes Docker

Prérequis
----------------------------------------------------------
* Docker Desktop est disponible pour Mac, Linux et Windows.
https://docs.docker.com/desktop
* Consultez notre documentation pour plus d'informations sur l'utilisation de Docker.
https://docs.docker.com

Images Docker
----------------------------------------------------------

Les images Docker sont des packages logiciels légers, autonomes et exécutables qui incluent tout ce qui est nécessaire pour exécuter une application :
code, environnement d'exécution, outils système, bibliothèques système et paramètres.
*  Build an Image from a Dockerfile
```bash
 docker build -t <image_name> .
```
* Liste des images locales
```bash
docker images 
```
* Supprimer une image
```bash
docker rmi <image_name> 
```
* Supprimer toutes les images inutilisées
```bash
 docker image prune 
```

Conteneurs
----------------------------------------------------------
Un conteneur est une instance d'exécution d'une image Docker. Un conteneur s'exécute toujours de la même manière, quelle que soit l'infrastructure.
Les conteneurs isolent le logiciel de son environnement et garantissent son fonctionnement uniforme malgré les différences, par exemple, entre les environnements de développement et de préproduction.
* Créez et exécutez un conteneur à partir d'une image, avec un nom personnalisé :
```bash
 docker run --name <container_name> <image_name>
```
* Lancez un conteneur et publiez le ou les ports de ce conteneur sur l'hôte.
```bash
docker run -p <host_port>:<container_port> <image_name>
```

Créer une machine virtuelle Docker avec Docker Machine
----------------------------------------------------------

*   Vous pouvez utiliser Docker Machine pour :
    
    · Installer et exécuter Docker sur Mac ou Windows
    
    · Provisionner et gérer plusieurs hôtes Docker distants
    
    · Provisionner des clusters Swarm
    
    Exemple simple pour créer une machine virtuelle Docker locale avec VirtualBox :
    
```bash
docker-machine create --driver=virtualbox default
docker-machine ls
eval "$(docker-machine env default)"
```
