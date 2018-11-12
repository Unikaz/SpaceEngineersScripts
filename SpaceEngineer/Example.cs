#if DEBUG
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

namespace SpaceEngineer
{
    public sealed class Program : MyGridProgram
    {
#endif
        /*
         * Ceci est une classe d'exemple pour les scripts pour Space Engineers.
         *
         * Le code au dessus de ce commentaire permet de gérer les dépendances dans le cadre du développement sans poser
         * de problème une fois en jeu. Seul le code entre le BEGIN et le END doit être modifié.
         *
         */
        //=======================================================================
        //////////////////////////BEGIN//////////////////////////////////////////
        //=======================================================================

        public Program()
        {
            /* Cette méthode est le constructeur. Elle est exécutée avant tout autre, lors du premier lancement de
             * votre script, ou lors du restart du serveur. Utilisez la pour initialiser votre script et pour définir
             * son UpdateFrequency.
             */
        }

        public void Main(string args)
        {
            /* La méthode principale de votre script. Elle est exécutée à chaque fois que vous lancerez un run,
             * ou à chaque update configurée par le UpdateFrequency
             */
        }

        public void Save()
        {
            /* Cette méthode est appelée quand le programme souhaite sauvegarder son état. Utilisez cette méthode pour
             * sauvegarder des données dans le champs Storage.
             */
        }

        //=======================================================================
        //////////////////////////END////////////////////////////////////////////
        //=======================================================================
#if DEBUG
    }
}
#endif