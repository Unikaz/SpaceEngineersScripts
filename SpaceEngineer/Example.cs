using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SpaceEngineer
{
    public class Example : AScript
    {
        /*
         * Ceci est une classe d'exemple pour les scripts pour Space Engineers
         *
         *     Vous  pouvez voir au-dessus que ma classe Example hérite de ma classe AScript. AScript permet de déclarer
         * les objets et fonctions mis à disposition par le jeu. Ces objets vous permettront d'accéder aux informations
         * et fonctions du jeu.
         *
         *     Pour que votre script fonctionne dans le jeu, il faudra le copier/coller dans un ProgrammableBlock, mais
         * il ne faut pas copier tout le contenu de votre fichier, seulement ce qui est entre les lignes marquées ainsi :
         * //====================================================
         */
        
        //=======================================================================
        // Début de votre script

        /*
         *     Cette fonction fait partie des fonctions reconnu par SpaceEngineers. Elle est éxécuté lors du premier
         * lancement de votre script, ainsi qu'au restart du serveur.
         *     Comme vous pouvez le voir, elle n'est pas syntaxiquement valide en C#, ce qui vous affichera une erreur
         * dans votre IDE, mais corriger l'erreur bloque SpaceEngineers...
         *     Elle sert souvent à initiliser votre script et souvent à définir le UpdateFrequency
         */
        public Program()
        {
            
        }

        /*
         *     La fonction Main est votre fonction principale. Elle est éxécutée à chaque fois que vous ferez un "run"
         * sur le Programmable Block, ou à chaque chaque update si vous avez défini un UpdateFrequency.
         */
        public void Main(string argument, UpdateType updateSource)
        {
            
        }

        // Fin de votre script
        //=======================================================================
    }
}