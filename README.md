# BackOfficeSolutionTemplate
Solution template de projet DotnetCore 2.1 (Architecture en oignon). 
Un grand merci à @Raynald Messie pour son savoir et sa patience :).

La solution comprend les projets suivants :

- Core : Définition des interfaces du projet.
- Data : Couche d'accès aux données. Configuré avec la librairie Linq2DB.SqlServer (WIP).
- Data : Projet contenant l'ensemble des Dtos.
- Services : Couche de communication entre les couches utiilisateurs et la couche d'accès aux données.
- Api : Couche de définiton des Api Controller. Contient la configuration nécessaire pour Swagger.

En parallèle, les projets complémentaires :

- DatabaseScripts : Projet de dépôt des scripts SQL de génération de la base de démo (WIP).
- TestsDemo : Projet librairie contenant le nécessaire pour exécuter des tests NUnit sur les projets DotnetCore. 
Contient également quelques classes d'exemples.
