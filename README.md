# SpaceEngineersScripts

Ce repo a pour but de fournir quelques scripts pour les ProgrammableBlock de Space Engineers.
Il essaye aussi de fournir les outils pour en développer soit-même !

## Configurer le projet
 
  Pour développer des scripts sur SE avec un IDE, vous pouvez ajouter certaines DLL du jeu en référence dans votre projet.
La technique pour les ajouter dépend de votre IDE.
### Sur Rider de JetBrains
  Cliquez droit sur votre projet, faites "Add Reference", sur la fenêtre qui s'ouvre, cliquez en bas sur "Add From..." et
allez dans le dossier de votre jeu (C:\Program Files\Steam\SteamApps\common\SpaceEngineers\Bin64\) et sélectionnez les fichiers
Sandbox.Common.dll, Sandbox.Game.dll, VRage.Game.dll, VRage.Library.dll, VRage.Math.dll et VRage.Scripting.dll.
Enfin, cliquez sur "OK", et c'est bon !

## Développer son propre script

  Pour vous simplifier la vie, il y a une classe Example.cs que vous pouvez copier. Celle-ci étend de AScript qui fournit les
objects accessibles dans le jeu. Il y a aussi les méthodes principales des scripts, ainsi que quelques explications.
