using System;
using Sandbox.ModAPI.Ingame;

namespace SpaceEngineer
{
    /**
     * This class is here to get the standard objects that the game will offer.
     * Your classes should extends this one
     */
    public abstract class AScript
    {
        public IMyGridTerminalSystem GridTerminalSystem;
        public IMyGridProgramRuntimeInfo Runtime;
        public IMyProgrammableBlock Me;
        [Obsolete("Use Runtime.TimeSinceLastRun instead")] public TimeSpan ElapsedTime;
        public string Storage;
        public Action<string> Echo;

        public virtual void Program()
        {
        }

        public virtual void Main(string argument, UpdateType updateSource){}
    }
    
    
}