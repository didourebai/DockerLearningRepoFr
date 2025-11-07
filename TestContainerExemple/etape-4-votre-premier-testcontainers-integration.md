# Étape 4 : Votre première intégration de Testcontainers

Depuis le site Web Testcontainers, nous apprenons qu'il existe un moyen simple d'exécuter différentes bases de données JDBC prises en charge avec Docker :[https://www.testcontainers.org/usage/database\_containers.html](https://www.testcontainers.org/usage/database_containers.html)

Une partie particulièrement intéressante sont les conteneurs basés sur JDBC-URL : 
[https://www.testcontainers.org/usage/database\_containers.html\#jdbc-url](https://www.testcontainers.org/usage/database_containers.html#jdbc-url)

Cela signifie que commencer à utiliser Testcontainers dans notre projet (une fois que nous avons ajouté une dépendance) est aussi simple que de modifier quelques propriétés dans Spring Boot :

```java
@SpringBootTest(webEnvironment = WebEnvironment.RANDOM_PORT, properties = {
 "spring.datasource.url=jdbc:tc:postgresql:16-alpine://testcontainers/workshop"
})
```

Si nous divisons l'URL magique JDBC, nous voyons :

* `jdbc:tc:` - cette partie indique que nous devons utiliser Testcontainers comme fournisseur JDBC
* `postgresql:16-alpine://` - nous utilisons une base de données PostgreSQL et nous sélectionnons la bonne image PostgreSQL à partir du Docker Hub comme image
* `testcontainers/workshop` - Le nom d'hôte (peut être quelconque) est « testcontainers » et le nom de la base de données est « workshop ». À vous de choisir !
  
Après avoir ajouté les propriétés et relancé le test, le problème est résolu ? Parfait !

Vérifiez les journaux.

```text
    13:30:59.352  INFO   --- [    Test worker] o.t.d.DockerClientProviderStrategy       : Environnement Docker trouvé avec socket Npipe local (npipe:////./pipe/docker_engine)
    13:30:59.369  INFO   --- [    Test worker] org.testcontainers.DockerClientFactory   : L'adresse IP de l'hôte Docker est localhost
    13:30:59.431  INFO   --- [    Test worker] org.testcontainers.DockerClientFactory   : Connecté à Docker: 
      Server Version: 20.10.11
      API Version: 1.41
      Operating System: Docker Desktop
      Total Memory: 3929 MB
    13:31:03.294  INFO   --- [    Test worker] org.testcontainers.DockerClientFactory   : Ryuk démarré - surveillera et terminera les conteneurs Testcontainers à la sortie de la JVM
    13:31:03.295  INFO   --- [    Test worker] org.testcontainers.DockerClientFactory   : Vérification du système...
    13:31:03.296  INFO   --- [    Test worker] org.testcontainers.DockerClientFactory   : ✔ La version du serveur Docker doit être au moins 1.6.0
    13:31:03.588  INFO   --- [    Test worker] org.testcontainers.DockerClientFactory   : ✔ L'environnement Docker doit disposer de plus de 2 Go d'espace disque libre
```

Comme vous pouvez le voir, Testcontainers a rapidement découvert votre environnement et s'est connecté à Docker.
Il a également effectué quelques vérifications avant le vol pour s'assurer que vous disposez d'un environnement valide.

## Indice 1:

Ajoutez la ligne suivante à votre fichier `~/.testcontainers.properties` pour désactiver ces vérifications et accélérer les tests :

```text
checks.disable=true
```

## Indice 2:

Changer la version de PostgreSQL est aussi simple que de remplacer « 16-alpine » par, par exemple, « 15-alpine ».
Essayez-le, mais n'oubliez pas qu'il téléchargera la nouvelle image depuis Internet, si elle n'est pas déjà présente sur votre ordinateur.

### 
[Suivant](etape-5-hello-200-ok.md)
