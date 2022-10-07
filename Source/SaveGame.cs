using System.Runtime.Serialization;

namespace GodotFramework.Core
{
    public partial class SaveGame : Godot.Object, ISerializable
    {
        public string Name = "I";

        public SaveGame() { }

        public SaveGame(SerializationInfo Info, StreamingContext Context)
        {
            //VarName = (VarName type)Info.GetValue("VarName", typeof(VarName type));
            Name = (string)Info.GetValue("Name", typeof(string));
        }

        public void GetObjectData(SerializationInfo Info, StreamingContext Context)
        {
            //Info.AddValue("VarName", this.VarName);
            Info.AddValue("Name", this.Name);
        }
    }
}