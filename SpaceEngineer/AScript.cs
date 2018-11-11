using System;
using Sandbox.ModAPI.Ingame;

namespace SpaceEngineer
{
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