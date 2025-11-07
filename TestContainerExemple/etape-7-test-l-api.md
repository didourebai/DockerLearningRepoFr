# Étape 7 : tester l'API

Créons maintenant un test pour notre API qui vérifiera la logique d'affaires.

```java
package com.example.demo.api;

import com.example.demo.model.Rating;
import com.example.demo.support.AbstractIntegrationTest;
import org.junit.jupiter.api.Test;

import static io.restassured.RestAssured.given;
import static org.awaitility.Awaitility.await;
import static org.hamcrest.Matchers.is;

public class RatingsControllerTest extends AbstractIntegrationTest {

    @Test
    public void testRatings() {
        String talkId = "testcontainers-integration-testing";

        given(requestSpecification)
                .body(new Rating(talkId, 5))
                .when()
                .post("/ratings")
                .then()
                .statusCode(202);

        await().untilAsserted(() -> {
            given(requestSpecification)
                    .queryParam("talkId", talkId)
                    .when()
                    .get("/ratings")
                    .then()
                    .body("5", is(1));
        });

        for (int i = 1; i <= 5; i++) {
            given(requestSpecification)
                    .body(new Rating(talkId, i))
                    .when()
                    .post("/ratings");
        }

        await().untilAsserted(() -> {
            given(requestSpecification)
                    .queryParam("talkId", talkId)
                    .when()
                    .get("/ratings")
                    .then()
                    .body("1", is(1))
                    .body("2", is(1))
                    .body("3", is(1))
                    .body("4", is(1))
                    .body("5", is(2));
        });
    }

    @Test
    public void testUnknownTalk() {
        String talkId = "cdi-the-great-parts";

        given(requestSpecification)
                .body(new Rating(talkId, 5))
                .when()
                .post("/ratings")
                .then()
                .statusCode(404);
    }
}
```

Exécutez-le et il échouera.

Pourquoi?

Il n'y a pas de Kafka !

Exécuter Kafka dans Docker est facile avec Testcontainers.
Il existe un module Testcontainers offrant une intégration avec Kafka et l'abstraction « KafkaContainer » pour votre code.

Ajoutez-le simplement de la même manière que vous avez ajouté Redis et définissez la propriété système `spring.kafka.bootstrap-servers`.

## Indice 1:

Certains conteneurs proposent des méthodes d'assistance. Vérifiez si une méthode d'assistance est disponible sur « KafkaContainer » et pourrait vous être utile.

## Indice 2:

Vous pouvez démarrer plusieurs conteneurs en parallèle en faisant :

```java
Stream.of(redis, kafka).parallel().forEach(GenericContainer::start);
```

### 
[Suivant](etape-7.7-donnes-strategies-init.md)
