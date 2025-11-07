# Étape 8.8 : Utilisation de Testcontainers sans la prise en charge des frameworks

Redis a ses propres limites.
Y a-t-il des limites à l'incrémentation du hachage ?
Découvrons-le !

## `RatingsRepositoryTest`

Nous allons créer un test isolé pour le référentiel basé sur Redis et vérifier nos cas extrêmes.

```java
package com.example.demo.repository;

import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class RatingsRepositoryTest {

    final String talkId = "testcontainers";

    RatingsRepository repository;

    @Test
    public void testEmptyIfNoKey() {
        assertThat(repository.findAll(talkId)).isEmpty();
    }

    @Test
    public void testLimits() {
        repository.redisTemplate.opsForHash()
                .put(repository.toKey(talkId), "5", Long.MAX_VALUE + "");

        repository.add(talkId, 5);
    }
}
```

Mais comme nous n'utilisons pas Spring Context ici, nous devons créer nous-mêmes une instance de notre référentiel :

```java
    @BeforeEach
    public void setUp() {
        LettuceConnectionFactory connectionFactory = new LettuceConnectionFactory(
                ?,
                ?
        );
        connectionFactory.afterPropertiesSet();
        repository = new RatingsRepository(new StringRedisTemplate(connectionFactory));
    }
```

La seule partie manquante est les arguments de « LettuceConnectionFactory », l'hôte et le port de Redis.

Nous utiliserons l'extension JUnit Jupiter de Testcontainers pour démarrer Redis :

```java
    @Container
    public GenericContainer redis = new GenericContainer("redis:3-alpine")
            .withExposedPorts(6379);
```

And add the `@Testcontainers` annotation to the class: 
```java
@Testcontainers
public class RatingsRepositoryTest {
```
And the code for initializing the connection factory:
```java
LettuceConnectionFactory connectionFactory = new LettuceConnectionFactory(
        redis.getHost(),
        redis.getFirstMappedPort()
);
```
Le test devrait échouer avec une erreur quelque peu cryptique :

```text
Error in execution; nested exception is io.lettuce.core.RedisCommandExecutionException: ERR increment or decrement would overflow
```
Et vous n'avez rien à corriger de votre côté : les tests repoussent les limites de Redis.
Mais le bon côté des choses, c'est que nous avons appris à utiliser Testcontainers en dehors de Spring Framework.
Nous avons également vu comment nous pouvons les utiliser pour comprendre les limites et le comportement des composants supplémentaires.

Supprimez le test avant que quiconque ne le remarque.
Je plaisante, transformons cela en un test utile en affirmant que nous lançons une exception personnalisée « MaxRatingsAddedException », qui indique que notre référentiel a enregistré le nombre maximal d'évaluations.
Nous pourrons toujours décider ultérieurement comment notre logique métier doit gérer ce problème (et déterminer s'il s'agit d'un cas limite à résoudre),
mais avec ce test, nous avons consciemment documenté notre connaissance des limites des systèmes avec lesquels nous intégrons nos services.

```java
@Test
public void testLimits() {
    repository.redisTemplate.opsForHash()
        .put(repository.toKey(talkId), "5", Long.MAX_VALUE + "");

        Assertions.assertThrows(MaxRatingsAddedException.class, () ->  repository.add(talkId, 5));
}
```

L'exercice final consiste maintenant à adapter l'implémentation de `RatingsRepository.add()` en conséquence, pour que le test réussisse.

