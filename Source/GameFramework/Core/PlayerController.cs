
namespace GameFramework.Core
{
    public partial class PlayerController : Controller
    {
        private Player player;

        public Pawn Pawn;

        public void SetPlayer(Player player)
        {
            this.player = player;
            player.PlayerController = this;
        }
    }
}