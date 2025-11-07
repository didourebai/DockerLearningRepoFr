# Étape 1 : Démarrage

## Vérifiez la version de Java

Vous aurez besoin de Java 17 ou d'une version plus récente pour cet atelier.
Les bibliothèques Testcontainers sont compatibles avec Java 8+, mais cet atelier utilise une application Spring Boot 3.x qui nécessite Java 17 ou une version plus récente.

## Installer la version Desktop Testcontainers 
Testcontainers Desktop est un outil gratuit qui peut vous aider lors du développement local et du débogage de vos tests.
Vous pouvez télécharger l'application depuis [https://testcontainers.com/desktop/](https://testcontainers.com/desktop/).

## Vérifier la version de Docker

Pour utiliser Testcontainers, vous devez disposer d'un environnement Docker compatible. 
Si l'application Testcontainers Desktop est installée, elle vous indiquera les environnements d'exécution de conteneurs disponibles sur votre machine.

* Vous pouvez choisir l'un des environnements d'exécution de conteneur installés, comme **Docker Desktop**, **OrbStack**, **Rancher Desktop**, etc.
* Cela peut être [Testcontainers Cloud](https://testcontainers.com/cloud) il est recommandé d'éviter de surcharger le réseau de conférence en extrayant des images Docker lourdes.
* Si vous n'avez pas d'environnements d'exécution de conteneur installés sur votre machine, vous pouvez également utiliser le **l'environnement d'exécution intégré (Embedded Runtime)** fourni par Testcontainers Desktop.
Pour utiliser Embedded Runtime, vous devez ajouter la propriété `embedded.runtime.enabled=true` à votre fichier  **$HOME/.testcontainers.properties**.

Vous pouvez vérifier la disponibilité de Docker en exécutant :
```text
$ docker version

Client:
 Cloud integration: v1.0.22
 Version:           20.10.11
 API version:       1.41
 Go version:        go1.16.10
 Git commit:        dea9396
 Built:             Thu Nov 18 00:42:51 2021
 OS/Arch:           windows/amd64
 Context:           default
Server: Docker Engine - Community
 Engine:
  Version:          20.10.11
  API version:      1.41 (minimum version 1.12)
  Go version:       go1.16.9
  Git commit:       847da18
  Built:            Thu Nov 18 00:35:39 2021
  OS/Arch:          linux/amd64
  Experimental:     false
  ...
```

## Télécharger le projet

Clone the following project from GitHub to your computer:  
[https://github.com/testcontainers/workshop](https://github.com/testcontainers/workshop)

## Créer le projet pour télécharger la dépendance

With Maven:
```text
./mvnw verify
```

## \(en option\) PRécupérez les images requises avant de faire l'atelier

Cela peut être utile si la connexion Internet sur le lieu de l'atelier est quelque peu lente.

```text
docker pull postgres:16-alpine
docker pull redis:7-alpine
docker pull openjdk:8-jre-alpine
docker pull confluentinc/cp-kafka:7.5.0
```

### 
[Suivant](etape-2-decouvrir-l-application.md)


