using FrameworkDesign;

namespace ShootingEditor2D
{
    public class PickGunCommand : AbstractCommand
    {
        private readonly string mName;
        private readonly int mInGunBullets;
        private readonly int mOutGunBullets;

        public PickGunCommand(string name, int inGunBullets, int outGunBullets)
        {
            mName = name;
            mInGunBullets = inGunBullets;
            mOutGunBullets = outGunBullets;
        }

        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();

            gunSystem.PickGun(mName, mInGunBullets, mOutGunBullets);
        }
    }
}