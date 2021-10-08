using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class ShootCommand : AbstractCommand
    {
        public static readonly ShootCommand Single = new ShootCommand();
        
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var timeSystem = this.GetSystem<ITimeSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            var gunConfigItem = gunConfigModel.GetItemByName(gunSystem.CurrentGun.Name.Value);
            
            gunSystem.CurrentGun.BulletCountInGun.Value--;

            gunSystem.CurrentGun.GunState.Value = GunState.Shooting;

            timeSystem.AddDelayTask(1.0f / gunConfigItem.Frequency, () =>
            {
                gunSystem.CurrentGun.GunState.Value = GunState.Idle;

                if (gunSystem.CurrentGun.BulletCountOutGun.Value > 0 &&
                    gunSystem.CurrentGun.BulletCountInGun.Value == 0)
                {
                    this.SendCommand<ReloadCommand>();
                }
            });
        }
    }
}