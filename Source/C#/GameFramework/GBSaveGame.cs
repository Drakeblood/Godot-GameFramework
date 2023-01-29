using System.Runtime.Serialization;

using Godot;

namespace GB.GameFramework
{
    public partial class GBSaveGame : GodotObject, ISerializable
    {
        public GBSaveGame() { }

        public GBSaveGame(SerializationInfo Info, StreamingContext Context)
        {
            //VarName = (VarName type)Info.GetValue("VarName", typeof(VarName type));
        }

        public virtual void GetObjectData(SerializationInfo Info, StreamingContext Context)
        {
            //Info.AddValue("VarName", this.VarName);
        }
    }
}