using System.Runtime.Serialization;

using Godot;

namespace GameFramework.System
{
    public partial class SaveGame : GodotObject, ISerializable
    {
        public SaveGame() { }

        public SaveGame(SerializationInfo info, StreamingContext context)
        {
            //VarName = (VarName type)Info.GetValue("VarName", typeof(VarName type));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //Info.AddValue("VarName", this.VarName);
        }
    }
}