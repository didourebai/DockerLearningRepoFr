# Étape 6 : Ajout de Redis

Le moyen le plus simple de fournir une instance Redis pour vos tests est d'utiliser `GenericContainer` avec une image Docker Redis :[https://www.testcontainers.org/usage/generic\_containers.html](https://www.testcontainers.org/usage/generic_containers.html)
L'intégration entre le code des tests et les Testcontainers est simple.  

## Des règles ? Non merci !

Testcontainers est fourni avec un support de première classe pour JUnit, mais dans notre application, nous souhaitons avoir une seule instance Redis partagée entre **tous (all)** les tests.
Heureusement, il existe les méthodes `.start()`/`.stop()` de `GenericContainer` pour le démarrer ou l'arrêter manuellement.

Ajoutez simplement le code suivant à votre `AbstractIntegrationTest` avec le code suivant :
```java
static final GenericContainer redis = new GenericContainer("redis:7-alpine")
                                            .withExposedPorts(6379);

@DynamicPropertySource
public static void configureRedis(DynamicPropertyRegistry registry) {
  redis.start();
  registry.add("spring.data.redis.host", redis::getHost);
  registry.add("spring.data.redis.port", redis::getFirstMappedPort);
}
```

Simple et beau, hein ?

Exécutez les tests, ils devraient maintenant tous réussir.

### 
[Suivant](etape-7-test-l-api.md)
