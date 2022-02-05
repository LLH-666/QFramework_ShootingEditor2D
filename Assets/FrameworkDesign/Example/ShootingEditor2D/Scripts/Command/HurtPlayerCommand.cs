using QFramework;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class HurtPlayerCommand : AbstractCommand
    {
        private readonly int mHurt;
        public HurtPlayerCommand(int hurt = 1)
        {
            mHurt = hurt;
        }

        protected override void OnExecute()
        {
            var playModel = this.GetModel<IPlayerModel>();
            playModel.HP.Value -= mHurt;

            if (playModel.HP.Value <= 0)
            {
                SceneManager.LoadScene("ShootingGameOver");
            }
        }
    }
}