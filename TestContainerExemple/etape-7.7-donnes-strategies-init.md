# Étape 7.7 : Stratégies d'initialisation des données

L'initialisation des données à l'aide de Spring est intéressante, mais vous pourriez parfois avoir besoin de solutions alternatives.

Dans cette étape, nous allons désactiver l'initialisation de la base de données Spring et explorer comment nous pouvons initialiser la base de données à l'aide d'une configuration spécifique au conteneur, puis passer à l'utilisation des migrations[Flyway](https://flywaydb.org/).

## Affirmer que les données sont réellement là

Pour accélérer l'exécution ou l'échec de la tâche, ajoutez un cas de test à « DemoApplicationTest » qui vérifie que les données de « schema.sql » sont correctement chargées dans la base de données.
Pour cela, vous pouvez « @Autowire » le « TalksRepository » dans la classe de test et l'utiliser pour vérifier qu'une conversation avec un ID donné est bien présente dans la base de données.

```java
Assertions.assertTrue(talksRepository.exists("testcontainers-integration-testing"));
```

## Exécution explicite de PostgreSQL

Tout d’abord, nous allons supprimer l’approche « URL JDBC modifiée » de Testcontainers et créer un objet conteneur PostgreSQL explicite pour simplifier la configuration ultérieure.

Dans « AbstractIntegratonTest », veuillez supprimer complètement « properties » de « @SpringBootTest(...) ».
Vous pouvez ensuite instancier un conteneur PostgreSQL en utilisant :

```java
static final PostgreSQLContainer<?> postgres = new PostgreSQLContainer<>("postgres:16-alpine");
```

De plus, nous devons démarrer le conteneur « postgres » de manière similaire aux autres dépendances de service et configurer notre application pour utiliser cette base de données conteneurisée, ce qui peut être fait en définissant les propriétés suivantes dans la méthode annotée « @DynamicPropertySource » :

```text
spring.datasource.url
spring.datasource.username
spring.datasource.password
```

Utilisez les valeurs fournies par l'objet « postgres » pour remplir la configuration requise.

L’exécution du test ajouté au début de cette étape devrait maintenant réussir.

## Initialiser la base de données sans Spring

Il peut arriver que le chargement de `schema.sql` ne suffise pas à initialiser complètement la base de données.
Nous allons simuler ce problème en contournant la convention Spring. Veuillez renommer `schema.sql` en `talks-schema.sql`.
Le test devrait échouer, car le schéma n'est pas initialisé dans le conteneur de base de données et, sans lui, l'application ne peut pas fonctionner correctement.

Faites fonctionner à nouveau l'initialisation de la base de données (et le test réussi) en initialisant la base de données directement dans le conteneur.

### Indice
La plupart des conteneurs de base de données disposent de fonctionnalités permettant d'initialiser la base de données à partir des fichiers de script fournis dans le conteneur.
Le conteneur PostgreSQL exécute tous les fichiers SQL du répertoire `/docker-entrypoint-initdb.d/`, , comme décrit dans le chapitre _Initialization scripts_ de la documentation du [Postgres container](https://hub.docker.com/_/postgres/).

Configurez l'objet « postgres » à l'aide de la méthode « withCopyToContainer » et de « MountableFile.forClasspathResource(String path) » pour configurer le schéma de la base de données. Une fois la base de données correctement initialisée, le test devrait fonctionner à nouveau (même si le fichier « schema.sql » n'est pas présent).

## Migration de la base de données avec Flyway

Ensuite, nous allons supprimer les requêtes d'initialisation des données du fichier « talks-schema.sql » et utiliser [Flyway](https://flywaydb.org/) pour remplir la base de données avec les données réelles.
Liquibase ou d'autres outils de migration de bases de données fonctionneraient de la même manière.

Veuillez ajouter la dépendance Flyway dans `build.gradle` :

```text
implementation 'org.flywaydb:flyway-core'
```

or `pom.xml`:
```text
<dependency>
    <groupId>org.flywaydb</groupId>
    <artifactId>flyway-core</artifactId>
</dependency>
```

Ensuite, déplacez toutes les instructions `INSERT ...` du fichier `talks-schema.sql` vers le fichier `src/main/resources/db/migration/V1_1__talks.sql`.

Notez que le fichier de migrations ne se trouve pas sur le chemin de classe **test**, car Flyway est susceptible d'être également utilisé pour la gestion des schémas de production.

Pour que Flyway ne se plaigne pas de ne pas pouvoir stocker ses données dans la base de données, nous devons le configurer pour créer ses tables et données de gestion de base de données manquantes.

Cela peut être fait dans `application.yml` avec :

```yaml
  flyway:
    baseline-on-migrate: true
```

Notez que `spring.flyway.locations=classpath:db/migration` est l'emplacement par défaut des fichiers de migration utilisés par Flyway, nous n'avons donc pas besoin de le configurer explicitement.
Pour plus de détails sur l'intégration de Spring Boot et Flyway, veuillez vous référer à [Spring manual](https://docs.spring.io/spring-boot/docs/2.6.7/reference/htmlsingle/#howto.data-initialization.migration-tool.flyway).

Le test vérifiant que les données sont correctement initialisées dans la base de données devrait réussir après avoir configuré Flyway pour exécuter correctement les migrations.
### 
[Suivant](etape-8-environnement-local-developpement.md)
