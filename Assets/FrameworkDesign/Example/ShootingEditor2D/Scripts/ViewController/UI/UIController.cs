using System;
using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class UIController : MonoBehaviour, IController
    {
        private IPlayerModel mPlayerModel;
        private IStatSystem mStatSystem;
        private IGunSystem mGunSystem;

        private int mMaxBulletCount;

        private void Awake()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
            mStatSystem = this.GetSystem<IStatSystem>();
            mGunSystem = this.GetSystem<IGunSystem>();

            mMaxBulletCount = this.SendQuery(new MacBulletCountQuery(mGunSystem.CurrentGun.Name.Value));
        }

        private void OnDestroy()
        {
            mPlayerModel = null;
            mStatSystem = null;
            mGunSystem = null;
        }

        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle
        {
            fontSize = 40
        });

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 100), $"生命:{mPlayerModel.HP.Value}/3", mLabelStyle.Value);
            GUI.Label(new Rect(10, 60, 300, 100),
                $"枪内子弹:{mGunSystem.CurrentGun.BulletCountInGun.Value}/{mMaxBulletCount}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 110, 300, 100), $"枪外子弹:{mGunSystem.CurrentGun.BulletCountOutGun.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 160, 300, 100), $"枪械名字:{mGunSystem.CurrentGun.Name.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 210, 300, 100), $"枪械状态:{mGunSystem.CurrentGun.GunState.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(Screen.width - 10 - 300, 10, 300, 100),
                $"击杀次数:{mStatSystem.KillCount.Value}", mLabelStyle.Value);
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }
    }
}