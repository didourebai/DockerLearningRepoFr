# Étape 11 : Utilisation de Testcontainers pour l'ingénierie du chaos

Jusqu'à présent, nous avons testé notre système dans des conditions très prévisibles.
Mais en réalité, nous savons que des problèmes peuvent survenir.
Le réseau peut être lent, la base de données indisponible, etc.

Dans cette étape, nous utiliserons Testcontainers pour simuler de telles conditions et voir comment notre système se comporte.

Pour cela, nous utiliserons le module [Toxiproxy](https://www.testcontainers.org/modules/toxiproxy/).

Consultez la documentation et écrivez un test qui vérifie le scénario suivant :
1. Initialement, PostgreSQL est disponible et nous pouvons enregistrer une évaluation.
2. Nous simulons ensuite une panne réseau entre notre application et PostgreSQL à l'aide de Toxiproxy, et nous nous attendons à ce que le point de terminaison renvoie une erreur.
3. Nous simulons ensuite une récupération réseau et nous nous attendons à ce que le point de terminaison renvoie une réussite.

## Indice

Vous devez ajouter le module Toxiproxy aux dépendances de votre projet.
