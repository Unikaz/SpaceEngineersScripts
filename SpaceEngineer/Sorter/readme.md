# Configurations du Sorter

Ce script s'occupe de trier les items en fonction des données mises dans la partie CustomData des coffres.

Chaque ligne de ces CustomData définie un ou des types de contenus à rappatrier/conserver. Le système utilise une configuration simple. Par exemple, si vous marquez "iron" il placera/autorisera dans ce coffre tout ce qui contient "iron" dans son nom (comme les "iron ingots" et les "iron ore"). Si vous mettez "ingot" il placera/autorisera donc tous les ingots.

Tous les coffres non-configurés seront aspirés vers les coffres configurés (dans la mesure où il y a de la place pour l'objet en question). Si vous voulez ignorez un coffre pour qu'il ne soit pas traité par le système, placez la commande "disableSorter" dans le CustomData du bloc.

Le système va aussi s'occuper de vider les outputs des raffineries et des assembleurs.