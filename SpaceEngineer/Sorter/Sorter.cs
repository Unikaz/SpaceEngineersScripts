#if DEBUG
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Activation;
using System.Security;
using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Compiler;
using VRage.Game.ObjectBuilders.Definitions;

namespace SpaceEngineer.Sorter
{
    public sealed class Program : MyGridProgram
    {
#endif
        /*
         * Ce script permet de trier des coffres automatiquement
         */
        //=======================================================================
        //////////////////////////BEGIN//////////////////////////////////////////
        //=======================================================================
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Main(string args)
        {
            var infos = new Dictionary<IMyTerminalBlock, List<List<string>>>();
            var containers = new List<IMyCargoContainer>();
            GridTerminalSystem.GetBlocksOfType(containers);
            // lister tous les CustomData
            for (var i = 0; i < containers.Count; i++)
            {
                if (string.IsNullOrEmpty(containers[i].CustomData)) continue;
                var list = new List<List<string>>();
                infos.Add(containers[i], list);
                var lines = containers[i].CustomData.Split('\n');
                list.AddRange(from l in lines where !string.IsNullOrWhiteSpace(l) select CleanList(l));
            }
            PrintConfig(infos);
            // transfert
            for (var i = 0; i < containers.Count; i++)
            {
                var source = containers[i];
                List<List<string>> config = null;
                if (!infos.TryGetValue(source, out config))
                    config = new List<List<string>>();

                for (int k = source.GetInventory(0).GetItems().Count - 1; k >= 0; k--)
                {
                    var item = source.GetInventory(0).GetItems()[k];
                    if (config.Any(line =>
                        line.All(namePart => item.GetDefinitionId().ToString().ToLower().Contains(namePart))))
                    {
                        // l'item a le droit de rester
                    }
                    else
                    {
                        // il faut mettre l'item ailleurs
                        // donc je liste les emplacements dédiés
                        var dests = infos
                            .Where(info => info.Value
                                .Any(line => line
                                    .All(namePart =>
                                        item.GetDefinitionId().ToString().ToLower().Contains(namePart))))
                            .Select(info => info.Key)
                            .Where(c => !c.GetInventory(0).IsFull)
                            .ToList();
                        // je transfert autant que possible
                        for (var j = 0; j < dests.Count && item.Amount.RawValue > 0; j++)
                        {
                            source.GetInventory(0).TransferItemTo(dests[j].GetInventory(0), k, null, true, null);
                        }
                    }
                }
            }
        }

        private void PrintConfig(Dictionary<IMyTerminalBlock, List<List<string>>> infos)
        {
            foreach (var keyValuePair in infos)
            {
                Echo(keyValuePair.Key.CustomName + ": ");
                foreach (var list in keyValuePair.Value)
                {
                    Echo("- " + String.Join(" ", list.ToArray()));
                }
            }
        }


        public List<string> CleanList(string value)
        {
            return new List<string>(value.Split('_'))
                .SelectMany(p => p.Split(' '))
                .Select(namePart => namePart.ToLower())
                .Select(namePart => namePart.Replace("5.56", "5p56"))
                .Where(namePart => namePart.Length > 1)
                .ToList();
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