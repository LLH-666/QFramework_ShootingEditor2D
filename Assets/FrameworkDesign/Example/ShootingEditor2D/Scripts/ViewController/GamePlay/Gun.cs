using System;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Gun : ShootingEditor2DController
    {
        private Bullet mBullet;

        private GunInfo mGunInfo;

        private int mMaxBulletCount;
        
        private void Awake()
        {
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();

            mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;

            mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunInfo.Name.Value));
        }

        private void OnDestroy()
        {
            mGunInfo = null;
        }

        public void Shoot()
        {
            if (mGunInfo.BulletCountInGun.Value > 0 && mGunInfo.GunState.Value == GunState.Idle) 
            {
                var bullet = Instantiate(mBullet.transform, mBullet.transform.position, mBullet.transform.rotation);
                //统一缩放值
                bullet.transform.localScale = mBullet.transform.lossyScale;
                bullet.gameObject.SetActive(true);

                this.SendCommand(ShootCommand.Single);
            }
        }
        
        public void Reload()
        {
            if (mGunInfo.GunState.Value == GunState.Idle &&
                mGunInfo.BulletCountInGun.Value != mMaxBulletCount &&
                mGunInfo.BulletCountOutGun.Value > 0)   
            {
                this.SendCommand<ReloadCommand>();
            }
        }
    }
}