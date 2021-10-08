using FrameworkDesign;

namespace ShootingEditor2D
{
    public class KillEnemyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IStatSystem>().KillCount.Value++;

            var randomIndex = UnityEngine.Random.Range(0, 100);

            if (randomIndex < 80)
            {
                this.GetSystem<IGunSystem>().CurrentGun.BulletCountInGun.Value += UnityEngine.Random.Range(1, 4);
            }
        }
    }
}