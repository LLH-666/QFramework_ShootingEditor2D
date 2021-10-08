using System;
using FrameworkDesign;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class UIGamePass : MonoBehaviour, IController
    {
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle
        {
            fontSize = 80,
            alignment = TextAnchor.MiddleCenter
        });
        
        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        });
        
        private void OnGUI()
        {
            var labelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f, 400, 10);

            GUI.Label(labelRect, "游戏通关", mLabelStyle.Value);
            
            var buttonRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f + 150, 200, 10);

            if (GUI.Button(buttonRect, "返回首页", mButtonStyle.Value))
            {
                SceneManager.LoadScene("ShootingGameStart");
            }
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }
    }
}