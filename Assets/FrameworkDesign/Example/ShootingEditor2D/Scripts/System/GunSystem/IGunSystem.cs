using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public interface IGunSystem : ISystem
    {
        GunInfo CurrentGun { get; }
    }

    public class GunSystem : AbstractSystem, IGunSystem
    {
        protected override void OnInit()
        {
            
        }

        public GunInfo CurrentGun { get; } = new GunInfo()
        {
            BulletCountInGun = new BindableProperty<int>()
            {
                Value = 3
            },
            
            Name = new BindableProperty<string>()
            {
                Value = "手枪"
            },
            
            GunState = new BindableProperty<GunState>()
            {
                Value = GunState.Idle
            },
            
            BulletCountOutGun = new BindableProperty<int>()
            {
                Value = 1
            }
        };
    }
}