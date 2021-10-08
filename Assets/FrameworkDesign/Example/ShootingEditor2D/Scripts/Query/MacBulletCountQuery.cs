using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class MacBulletCountQuery : AbstractQuery<int>
    {
        private readonly string mGunName;

        public MacBulletCountQuery(string gunName)
        {
            mGunName = gunName;
        }

        protected override int OnDo()
        {
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            var gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.BulletMaxCount;
        }
    }
}