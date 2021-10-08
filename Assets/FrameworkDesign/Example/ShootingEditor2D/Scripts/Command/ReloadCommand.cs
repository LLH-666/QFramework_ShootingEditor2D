using FrameworkDesign;

namespace ShootingEditor2D
{
    public class ReloadCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var timeSystem = this.GetSystem<ITimeSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            var currentGun = gunSystem.CurrentGun;

            var gunConfigItem = gunConfigModel.GetItemByName(currentGun.Name.Value);

            var needBulletCount = gunConfigItem.BulletMaxCount - currentGun.BulletCountInGun.Value;
            var currentBulletCountOutGun = currentGun.BulletCountOutGun.Value;
            if (needBulletCount > 0)
            {
                if (currentBulletCountOutGun > 0)
                {
                    //状态切换
                    currentGun.GunState.Value = GunState.Reload;
                    //状态返回
                    timeSystem.AddDelayTask(gunConfigItem.ReloadSeconds, () =>
                    {
                        //如果枪内子弹充足
                        if (currentBulletCountOutGun >= needBulletCount)
                        {
                            currentGun.BulletCountOutGun.Value -= needBulletCount;
                            currentGun.BulletCountInGun.Value += needBulletCount;
                        }
                        //子弹不足
                        else
                        {
                            currentGun.BulletCountOutGun.Value = 0;
                            currentGun.BulletCountInGun.Value += currentBulletCountOutGun;
                        }
                        
                        currentGun.GunState.Value = GunState.Idle;
                    });
                }
            }
        }
    }
}