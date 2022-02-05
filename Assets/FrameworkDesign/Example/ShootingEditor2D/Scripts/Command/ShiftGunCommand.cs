using QFramework;

namespace ShootingEditor2D
{
    public class ShiftGunCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            
            gunSystem.ShiftGun();
        }
    }
}