README - Level 04 : Payload
----------------
---- Situation : 
----------------

Un Payload est apparu, et doit se rendre à son Goal.


---------------
---- Objectif : 
---------------

Trouver un moyen fiable (90~95%) de contrôler le Payload à l'aide de Joints, afin que celui-ci puisse se rendre au PayloadGoal.


--------------
---- Méthode : 
--------------

Vous devez créer un Joint entre votre Spaceship et votre Payload, vous permettant de contrôler la position et la rotation dy Payload.
Vous pouvez utiliser le type de joints de votre choix, mais il vous faut trouver une méthode fiable (au moins à 90~95%) de permettre au Payload d'atteindre le PayloadGoal.


--------------
---- Détails :
--------------

Le PayloadComponent ainsi que le PayloadGoalComponent doivent être détectés à l'aide de Queries depuis votre Spaceship.
Vous pouvez utiliser GetComponent<PayloadGoalComponent> et GetComponent<PayloadComponent> pour filtrer les objets détectés par vos Queries.

Une fois atteint, le PayloadGoal va réaparaitre ailleurs. Vous devrez alors de nouveau le détecter.
Le Payload n'est jamais détruit.

Votre vaisseaux à la rotation en Z bloquée pour ce niveau par simplicité. La rotation initialle est toujours randomisée.


---------------
---- Features :
---------------

Vous pouvez maintenant overrider les méthodes : 
OnGoalSpawned et OnGoalDestroyed pour faire votre logique. Ces events sont disponibles pour vous aider à faire votre logique event based.
Leur utilisation est totalement facultative.


-----------------------------
---- Remarques personnelles : 
-----------------------------

Hinge Joint avec Motors me semble la méthode la plus simple. 
Fixed Joint demande un peu plus de méthode pour fonctionner convenablement.
Configurable Joint est compliqué à mettre en place, mais le motor de Position pourrait vraiment être utile.

Vous pouvez détruire et recréer le Joint autant de fois que vous le voulez.

