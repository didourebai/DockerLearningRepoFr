# Étape 3 : Ajout de quelques tests

L'application ne comporte pas encore de tests. 
Mais avant d'écrire notre premier test, créons une classe de test abstraite pour les points communs entre les tests.

## Classe abstraite

Ajoutez la classe  `com.example.demo.support.AbstractIntegrationTest` u sourceset `src/test/java` . 
Il doit s'agir d'une classe abstraite avec des annotations standard du framework de test Spring Boot :

```java
@SpringBootTest(webEnvironment = WebEnvironment.RANDOM_PORT)
```

## Notre tout premier test

Nous devons maintenant tester que le contexte démarre.
Ajoutez `com.example.demo.DemoApplicationTest`, étendez-le à partir de votre classe de base `AbstractIntegrationTest` et ajoutez un test factice :

```java
@Test
void contextLoads() {
}
```

Exécutez-le et vérifiez que l’application démarre et que le test réussit.
Spring détectera H2 sur le classpath et l'utilisera comme base de données intégrée.

Il s’agit déjà d’un test de fumée utile car il garantit que Spring Boot est capable d’initialiser le contexte de l’application avec succès.

## Remplir la base de données

Le contexte commence.
Cependant, nous devons remplir la base de données avec certaines données avant de pouvoir écrire les tests.

Ajoutons un fichier `src/test/resources/schema.sql` avec le contenu suivant :

```sql
CREATE TABLE IF NOT EXISTS talks(
  id    VARCHAR(64)  NOT NULL,
  title VARCHAR(255) NOT NULL,
  PRIMARY KEY (id)
);

INSERT
  INTO talks (id, title)
  VALUES ('testcontainers-integration-testing', 'Modern Integration Testing with Testcontainers')
  ON CONFLICT do nothing;

INSERT
  INTO talks (id, title)
  VALUES ('flight-of-the-flux', 'A look at Reactor execution model')
  ON CONFLICT do nothing;
```

Relancez le test. Oh non, il échoue !

```text
...
Caused by: org.h2.jdbc.JdbcSQLException: Syntax error in SQL statement "INSERT INTO TALKS (ID, TITLE) VALUES ('testcontainers-integration-testing', 'Modern Integration Testing with Testcontainers') ON[*] CONFLICT DO NOTHING";
...
```

Il semble que H2 ne prenne pas en charge la syntaxe SQL de PostgreSQL, du moins pas par défaut.

### 
[Next](etape-4-votre-premier-testcontainers-integration.md)
