README - Level 03 : Contournement d'obstacle

Situation : 

Un obstacle apparait maintenant entre vous et le Goal.

Objectif : 

Trouver un moyen de contourner l'obstacle, et se rendre au Goal.
Vous pouvez utiliser tous les types de Queries que vous voulez.


 
NOTE IMPORTANTE : Le Système de Sensor est maintenant Optionnel par soucis de simplicité. 
Si vous voulez malgré tout l'utiliser, vous pouvez créer vos propres SensorsComponents qui héritent de ISensor (cf : RaySensor_Example.cs).
Mais vous pouvez aussi simplement ajouter le code de détection (Physics.Raycast et autres...) dans votre SpaceshipController.



Détails :
 
Le Goal n'est plus détecté par défaut. Il vous faut faire une query (Raycast, SphereCast, Overlap etc...) pour le trouver.
Le Goal est le seul objet ayant un GoalComponent. Utilisez cette information pour le filtrer des autres objets.
 
Il vous faut ensuite trouver un chemin vous même pour arriver à atteindre le Goal.
Là aussi vous pouvez utiliser des Queries pour trouver les potentiels obstacle et établir une trajectoire.
 
Lorsque votre Spaceship atteint le Goal, la fonction SpaceshipBase.OnGoalReached est appelée. Un nouveau Goal est alors présent dans le niveau.
Vous devriez donc relancer une détection à ce moment.


 
Utilisez le moins de Queries possibles. 
D'ici la fin du projet, le nombre de queries à chaque Frame sera limité.
Les queries peuvent être une opération couteuse pour les temps de calculs. Essayez de garder ça en tête.

