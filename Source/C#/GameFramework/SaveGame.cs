using System.Runtime.Serialization;

using Godot;

namespace GameFramework.System
{
    public partial class SaveGame : GodotObject, ISerializable
    {
        public SaveGame() { }

        public SaveGame(SerializationInfo Info, StreamingContext Context)
        {
            //VarName = (VarName type)Info.GetValue("VarName", typeof(VarName type));
        }

        public virtual void GetObjectData(SerializationInfo Info, StreamingContext Context)
        {
            //Info.AddValue("VarName", this.VarName);
        }
    }
}