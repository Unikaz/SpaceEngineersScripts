# Configuration des LCDs

Pour configurer les LCDs, ce script utilise les champs CustomData des écrans.

Différentes commandes sont à disposition et prennent cette forme :
**$commandName[params]**
Tout ce qui n'a pas cette forme sera interprété comme du texte normal.

## Liste des commandes et leur options
Pour information, les paramètres entourés de chevrons (\<param>) sont obligatoires, ceux entre crochets ([param]) sont optionnels.
Dans le cas d'un choix entre des paramètres, ceci sont représentés par un pipe (|) de cette manière : <param1 | param2>.
Les paramètres doivent être nommé. Par exemple **$count \<name="Uranium Ore">** (sauf $page qui prend directement le nom)

- **$page \<name>** : permet de créer des pages qu'il sera possible de d'appeler via des boutons. 
- **$count \<item> [ignoreIfEmpty]** : affiche la quantité d'un item/ressource, ignoreIfEmpty n'affiche pas la ligne si le résultat est 0. Le nom de l'item est très permissif. Par exemple, vous pouvez mettre "ingot" pour lister tout les lingots quelque soit la matière. 
- **$progress \<group | name> [length, text | bar | full]** : affiche une progress bar du remplissage du groupe ou du bloc spécifié. Length permet de changer la taille de la bar, text n'affichera que le pourcentage, bar que la barre (par défaut) et full affichera les deux.
- **$GlobalJumpDrive** : affiche le remplissage global des Jump Drives
- **$ores** [container]: liste tous les minerais du vaisseau, ou d'un conteneur si spécifié
  
