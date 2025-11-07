# Étape 5 : Bonjour, est-ce que vous avez 200 ans ?

L’une des fonctionnalités intéressantes de Spring Boot est l’actionneur et son point de terminaison de santé.
Il vous donne un aperçu de l'état de santé de votre application.

The context starts, but what's about the health of the app?

## Configurer Rest Assured

Pour vérifier le point de terminaison de santé de notre application, nous utiliserons la bibliothèque [RestAssured](http://rest-assured.io/).

Cependant, avant de l'utiliser, nous devons d'abord le configurer.
Ajoutez ce qui suit à votre classe de test abstraite puisque nous le partagerons entre tous les tests :

```java
protected RequestSpecification requestSpecification;

@LocalServerPort
protected int localServerPort;

@BeforeEach
void setUpAbstractIntegrationTest() {
    RestAssured.enableLoggingOfRequestAndResponseIfValidationFails();
    requestSpecification = new RequestSpecBuilder()
            .setPort(localServerPort)
            .addHeader(
                    HttpHeaders.CONTENT_TYPE,
                    MediaType.APPLICATION_JSON_VALUE
            )
            .build();
}
```

Ici, nous demandons à Spring Boot d'injecter le port aléatoire qu'il a reçu au démarrage de l'application, afin que nous puissions préconfigurer la requestSpecification de RestAssured.

## Appeler le point de terminaison

Vérifions maintenant si l'application est réellement saine en effectuant ce qui suit dans notre « DemoApplicationTest » :

```java
@Test
void healthy() {
    given(requestSpecification)
            .when()
            .get("/actuator/health")
            .then()
            .statusCode(200)
            .log().ifValidationFails(LogDetail.ALL);
}
```

Si nous exécutons le test, il échouera :

```text
...
HTTP/1.1 503 Service Unavailable
transfer-encoding: chunked
Content-Type: application/vnd.spring-boot.actuator.v2+json;charset=UTF-8

{
    "status": "DOWN",
    "details": {
        "diskSpace": { ... },
        "redis": {
            "status": "DOWN",
            "details": {
                "error": "org.springframework.data.redis.RedisConnectionFailureException: Unable to connect to Redis; nested exception is io.lettuce.core.RedisConnectionException: Unable to connect to localhost:6379"
            }
        },
        "db": {
            "status": "UP",
            "details": {
                "database": "PostgreSQL",
                "hello": 1
            }
        }
    }
}
... 
Expected status code <200> but was <503>.
```

Il semble qu'il n'ait pas pu trouver Redis et qu'il n'y ait pas d'option autoconfigurable pour cela.

### 
[Next](etape-6-ajouter-redis.md)
