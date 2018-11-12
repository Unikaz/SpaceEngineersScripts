# SpaceEngineersScripts

Ce repo a pour but de fournir quelques scripts pour les ProgrammableBlock de Space Engineers.
Il essaye aussi de fournir les outils pour en développer soit-même !

## Configurer le projet
 
  Pour développer des scripts sur SE avec un IDE, vous pouvez ajouter certaines DLL du jeu en référence dans votre projet.
La technique pour les ajouter dépend de votre IDE.
  Les DLLs à ajouter sont les suivantes :
  - Sandbox.Common.dll
  - Sandbox.Game.dll
  - VRage.Game.dll
  - VRage.Library.dll
  - VRage.Math.dll
  - VRage.Scripting.dll
  - SpaceEngineers.Game.dll
  
### Sur Rider de JetBrains
  Cliquez droit sur votre projet, faites "Add Reference", sur la fenêtre qui s'ouvre, cliquez en bas sur "Add From..." et
allez dans le dossier de votre jeu (C:\Program Files\Steam\SteamApps\common\SpaceEngineers\Bin64\) et sélectionnez les fichiers.
Enfin, cliquez sur "OK", et c'est bon !

## Développer son propre script

  Pour vous simplifier la vie, il y a une classe Example.cs que vous pouvez copier. Celle-ci fournit les bases pour vos scripts
et fournit aussi quelque explications.  
  Si vous comprenez l'anglais, je vous conseil aussi d'aller lire ce Github https://github.com/malware-dev/MDK-SE. Malware-dev est
le créateur du compileur utilisé par Space Engineers pour ses scripts. Ce Github contient beaucoup d'informations intéressantes.
