# Étape 2: Découvrir l'application

L'application est un microservice simple basé sur Spring Boot pour l'évaluation des conférences. Elle fournit une API permettant de suivre les évaluations des conférences en temps réel.

## Stockage

### Base de données SQL avec les discussions

Lorsqu'une évaluation est soumise, nous devons vérifier que le discours pour l'ID donné est présent dans notre base de données.

Notre base de données de choix est PostgreSQL, accessible avec Spring JDBC.

Vérifier `com.example.demo.repository.TalksRepository`.

### Redis

Nous stockons les notes dans la base de données Redis avec Spring Data Redis.

Vérifier `com.example.demo.repository.RatingsRepository`.

### Kafka

Nous utilisons ES/CQRS pour matérialiser les événements dans l'état. Kafka agit comme un courtier et nous utilisons Spring Kafka.

Vérifier `com.example.demo.streams.RatingsListener`.

## API

L'API est un contrôleur Spring Web REST \(`com.example.demo.api.RatingsController`\) et expose deux points de terminaison :

* `POST /ratings { "talkId": ?, "value": 1-5 }` ajouter une note à une discussion
* `GET /ratings?talkId=?` pour obtenir l'histogramme des notes de la conférence donnée
### 
[Suivant](etape-3-ajouter-quelques-tests.md)
