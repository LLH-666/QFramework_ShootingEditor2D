using QFramework;

namespace ShootingEditor2D
{
    public class AddBulletCommand : AbstractCommand
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

            if (gunConfigItem.NeedBullet)
            {
                gunInfo.BulletCountOutGun.Value += maxBulletCount;
            }
            else
            {
                gunInfo.BulletCountInGun.Value = maxBulletCount;
            }
        }
    }
}