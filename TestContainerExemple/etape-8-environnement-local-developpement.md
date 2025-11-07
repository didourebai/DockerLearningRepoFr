# Étape 8 : Environnement de développement local avec Testcontainers

Testcontainers est essentiel pour créer des environnements éphémères pour tester vos applications.
Cependant, rien dans l'API n'est spécifique à ces conditions, et vous pouvez l'utiliser pour gérer par programmation n'importe quel environnement.

L'un des scénarios les plus courants consiste à créer un environnement pour votre application lors de son exécution locale.
Certains frameworks applicatifs s'intègrent à Testcontainers pour offrir cette fonctionnalité prête à l'emploi :

* Quarkus a [Services de développement](https://quarkus.io/guides/dev-services).
* Micronaut a [Ressources de test](https://micronaut-projects.github.io/micronaut-test-resources/latest/guide/).
* Spring a [peut également être configuré](https://docs.spring.io/spring-boot/docs/3.1.0/reference/html/features.html#features.testing.testcontainers.at-development-time).

Dans ce chapitre, nous allons configurer notre application Spring Boot pour utiliser Testcontainers au moment du développement.

## Extraction de l'environnement vers une configuration

Vérifiez que vos applications ont la dépendance « org.springframework.boot:spring-boot-testcontainers ».

Cette application nécessite la configuration d'instances Postgres, Kafka et Redis pour une exécution locale.
Nos tests contiennent déjà tout le code nécessaire pour instancier ces services, les configurer et l'application qui les utilise.
Cependant, pour faciliter l'exécution locale de l'application, nous devrions refactoriser ce code afin qu'il soit intégré au cycle de vie d'initialisation de l'application.

Nous utiliserons un [TestConfiguration](https://docs.spring.io/spring-boot/docs/current/api/org/springframework/boot/test/context/TestConfiguration.html) pour encapsuler la création de l'environnement.

Créez une classe `ContainerConfig` dans le classpath de test qui implémentera `TestConfiguration` :

```java
@TestConfiguration(proxyBeanMethods = false)
public class ContainerConfig {
    @Bean
    @ServiceConnection(name = "redis")
    public GenericContainer redis() {
        return new GenericContainer<>("redis:7-alpine")
                .withExposedPorts(6379);
    }
}
```

L'exemple ci-dessus contient une définition `@Bean` pour un conteneur Redis.
Créez des définitions `@Bean` similaires pour `PostgresContainer` et `KafkaContainer` ; vous pouvez copier la configuration du conteneur à partir des classes de test.

Spring Boot 3.1 intègre Testcontainers ; les conteneurs exposés comme « Bean » sont donc automatiquement démarrés par le framework.
La méthode « ServiceConnection » permet à Spring Boot de se configurer pour utiliser les services (de manière similaire à la méthode « @DynamicPropertySource » précédemment).

Nous allons maintenant utiliser cette classe de configuration pour créer l'environnement de nos tests et exécuter l'application localement.

## Exécution de tests avec l'approche Context Initializer

Supprimez la configuration du conteneur de la classe `AbstractIntegrationTest` et supprimez la méthode `@DynamicPropertySource`.

Demandez au test d'utiliser la configuration dans laquelle les conteneurs sont initialisés en tant que beans, en ajoutant la propriété `classes` à `SpringBootTest` :

```java
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT,
  properties = {
  }, classes = {ContainerConfig.class})
```

Vous pouvez maintenant vérifier si les tests se déroulent toujours normalement.

## Exécution de votre application avec l'approche Context Initializer

La classe « ContainerConfig » se trouve dans le chemin de classe **test** et n'est pas accessible dans le chemin de classe de l'application. Ceci est intentionnel, car nous ne souhaitons pas rendre Testcontainers et autres bibliothèques de test accessibles dans le chemin de classe de l'application.

So to use the `ContainerConfig`, we need to create a separate entry point for running the application.
Create a `TestDemoApplication` class, that uses the actual `DemoApplication` and also uses the `ContainerConfig` we implemented above: 

```java
public class TestApplication {
    public static void main(String[] args) {
        SpringApplication
                .from(DemoApplication::main)
                .with(ContainerConfig.class)
                .run(args);
    }
}
```

L'exécution de cette classe exécutera « DemoApplication » et les journaux devraient indiquer que l'application a démarré correctement. L'arrêt de l'application supprimera les conteneurs, tout comme Testcontainers nettoie l'environnement après l'exécution des tests.

## Indice 1:

Vous pouvez dissocier le cycle de vie des conteneurs de celui du contexte Spring à l'aide des outils de développement.
Ajoutez la dépendance « spring-boot-devtools ».
Annotez les beans et les méthodes de beans avec `@RestartScope`.

Lors du rechargement des modifications du projet avec devtools, vous pouvez voir que les conteneurs ne sont pas redémarrés.

## Connexion à la base de données
Une fois l'application exécutée, vous souhaiterez peut-être vous connecter à la base de données pour inspecter les données.
Par défaut, Testcontainers démarre les conteneurs et les mappe à un port aléatoire disponible sur l'hôte.
Vous devez donc identifier le port mappé pour vous connecter à la base de données.

Au lieu de cela, nous pouvons utiliser la prise en charge des ports fixes de Testcontainers Desktop pour nous connecter à la base de données.

Ouvrez Testcontainers Desktop et sélectionnez « Services » -> « Ouvrir l'emplacement de configuration ».
Un répertoire contenant les exemples de fichiers de configuration des services fréquemment utilisés s'ouvrira.

Copiez `postgres.toml.example` dans `postgres.toml` et mettez à jour son contenu comme suit :

```toml
ports = [
  {local-port = 5432, container-port = 5432},
]

selector.image-names = ["postgres"]
```

Cette configuration mappera le port 5432 du conteneur Postgres au port 5432 de l'hôte.
Lorsque vous exécutez l'application, vous pouvez désormais vous connecter à la base de données avec les informations de connexion suivantes :

```
host: localhost
port: 5432
username: test
password: test
database: test
```

## Conteneurs réutilisables
Pendant le développement de l'application, vous souhaiterez peut-être l'arrêter et la redémarrer plusieurs fois.
Au lieu de créer les conteneurs de A à Z à chaque fois, vous pouvez utiliser la fonctionnalité de réutilisation de Testcontainers.

* Activez la fonctionnalité de réutilisation dans Testcontainers Desktop en activant **Preferences** -> **Enable reusable containers**.
* Mettez à jour la configuration des conteneurs pour utiliser la fonctionnalité de réutilisation avec `.withReuse(true)` :

```java
@Bean
@ServiceConnection
public PostgreSQLContainer<?> postgres() {
    return new PostgreSQLContainer<>("postgres:16-alpine").withReuse(true);
}
```

Désormais, au redémarrage de l'application, vous constaterez que le conteneur est réutilisé lors de l'exécution précédente. 
En activant la fonctionnalité de réutilisation, Testcontainers ne supprimera pas automatiquement ces conteneurs réutilisables 
lorsque l'application est arrêtée ou que l'exécution du test est terminée.

Si vous n’avez plus besoin du conteneur, vous pouvez le retirer du Testcontainers Desktop -> **Terminate Containers**.

## Geler les conteneurs pour empêcher leur arrêt pour déboguer
Lorsque vous exécutez des tests Testcontainers, les conteneurs sont automatiquement arrêtés et supprimés une fois l'exécution terminée.
Cette fonctionnalité est très utile pour maintenir un environnement propre et prévenir les fuites de ressources.
Cependant, il peut arriver que vous souhaitiez déboguer le test et inspecter les données de la base de données ou les messages de la rubrique Kafka.

Vous pouvez utiliser la fonctionnalité **Freeze containers shutdown** de Testcontainers Desktop 
cela empêchera l'arrêt du conteneur, vous permettant de déboguer le problème.
