using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public interface IGunSystem : ISystem
    {
        GunInfo CurrentGun { get; }
        Queue<GunInfo> GunInfos { get; }
        void PickGun(string name, int bulletCountInGun, int bulletCountOutGun);
        void ShiftGun();
    }

    public struct OnCurrentGunChanged
    {
        public string Name { get; set; }
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

        public Queue<GunInfo> GunInfos => mGunInfos;

        private Queue<GunInfo> mGunInfos = new Queue<GunInfo>();

        public void ShiftGun()
        {
            if (mGunInfos.Count > 0)
            {
                var previousGun = mGunInfos.Dequeue();

                EnqueueCurrentGun(previousGun.Name.Value, previousGun.BulletCountInGun.Value,
                    previousGun.BulletCountOutGun.Value);
            }
        }
        
        public void PickGun(string name, int bulletCountInGun, int bulletCountOutGun)
        {
            //如果是当前枪
            if (CurrentGun.Name.Value == name)
            {
                CurrentGun.BulletCountInGun.Value += bulletCountInGun;
                CurrentGun.BulletCountOutGun.Value += bulletCountOutGun;
            }
            //队列里面已经缓存了这把枪
            else if (mGunInfos.Any(gunInfo => gunInfo.Name.Value == name))
            {
                var gunInfo = mGunInfos.First(gun => gun.Name.Value == name);
                gunInfo.BulletCountInGun.Value += bulletCountInGun;
                gunInfo.BulletCountOutGun.Value += bulletCountOutGun;
            }
            else
            {
                EnqueueCurrentGun(name, bulletCountInGun, bulletCountOutGun);
            }
        }

        private void EnqueueCurrentGun(string nextGunName, int nextBulletCountInGun, int nextBulletCountOutGun)
        {
            var currentGunInfo = new GunInfo
            {
                Name = new BindableProperty<string>()
                {
                    Value = CurrentGun.Name.Value
                },
                BulletCountInGun = new BindableProperty<int>()
                {
                    Value = CurrentGun.BulletCountInGun.Value
                },
                BulletCountOutGun = new BindableProperty<int>()
                {
                    Value = CurrentGun.BulletCountOutGun.Value
                },
                GunState = new BindableProperty<GunState>()
                {
                    Value = CurrentGun.GunState.Value
                }
            };

            mGunInfos.Enqueue(currentGunInfo);

            CurrentGun.Name.Value = nextGunName;
            CurrentGun.BulletCountInGun.Value = nextBulletCountInGun;
            CurrentGun.BulletCountOutGun.Value = nextBulletCountOutGun;

            this.SendEvent(new OnCurrentGunChanged()
            {
                Name = nextGunName
            });
        }
    }
}