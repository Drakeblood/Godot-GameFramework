using System.Runtime.Serialization;

namespace GB.Core
{
    public partial class GBSaveGame : Godot.Object, ISerializable
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