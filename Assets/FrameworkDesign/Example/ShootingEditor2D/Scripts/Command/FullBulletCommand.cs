using FrameworkDesign;

namespace ShootingEditor2D
{
    public class FullBulletCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            
            var currentGunInfo = gunSystem.CurrentGun;
            AddBullet(currentGunInfo, gunConfigModel);

            var gunInfos = gunSystem.GunInfos;
            foreach (var item in gunInfos)
            {
                AddBullet(item, gunConfigModel);
            }
        }
        
        private void AddBullet(GunInfo gunInfo, IGunConfigModel gunConfigModel)
        {
            var gunConfigItem = gunConfigModel.GetItemByName(gunInfo.Name.Value);
            var maxBulletCount = gunConfigItem.BulletMaxCount;

            gunInfo.BulletCountInGun.Value = maxBulletCount;
        }
    }
}