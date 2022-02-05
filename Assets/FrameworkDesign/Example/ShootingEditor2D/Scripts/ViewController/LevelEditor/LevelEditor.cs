using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        public enum OperateMode
        {
            Draw,
            Erase
        }

        public enum BrushType
        {
            Ground,
            Player
        } 
        
        public class LevelItemInfo
        {
            public string Name;
            public float X;
            public float Y;
        }

        public SpriteRenderer EmptyHighlight;

        private OperateMode mCurrentOperateMode = OperateMode.Draw;
        private BrushType mCurrentBrushType = BrushType.Ground;

        [SerializeField] private GameObject mCurrentMouseSelectObj;

        private Camera mCamera;

        private bool mCanDraw;

        private void Start()
        {
            mCamera = Camera.main;
        }

        private Lazy<GUIStyle> mModelLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 30,
            alignment = TextAnchor.MiddleCenter
        });

        private Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 30,
        });

        private void OnGUI()
        {
            var modelLabelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, 35, 300, 50);
            string showText = mCurrentOperateMode.ToString();
            switch (mCurrentOperateMode)
            {
                case OperateMode.Draw:
                    showText = mCurrentOperateMode + "/" + mCurrentBrushType;
                    break;
                case OperateMode.Erase:
                    showText = mCurrentOperateMode.ToString();
                    break;
            }
            GUI.Label(modelLabelRect, showText, mModelLabelStyle.Value);

            var drawButtonRect = new Rect(10, 10, 150, 50);
            if (GUI.Button(drawButtonRect, "绘制", mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Draw;
            }

            var eraseButtonRect = new Rect(10, 60, 150, 50);
            if (GUI.Button(eraseButtonRect, "擦除", mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Erase;
            }

            if (mCurrentOperateMode == OperateMode.Draw)
            {
                var groundButtonRect = new Rect(Screen.width - 150, 10, 150, 50);
                if (GUI.Button(groundButtonRect, "地板", mButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Ground;
                }

                var playerButtonRect = new Rect(Screen.width - 150, 60, 150, 50);
                if (GUI.Button(playerButtonRect, "主角", mButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Player;
                }
            }

            var saveButtonRect = new Rect(Screen.width - 150, Screen.height - 60, 150, 50);
            if (GUI.Button(saveButtonRect, "保存", mButtonStyle.Value)) 
            {
                Debug.Log("保存");

                Save();
            }
        }

        private void Save()
        {
            var infos = new List<LevelItemInfo>(transform.childCount);
            foreach (Transform child in transform)
            {
                var pos = child.position;
                infos.Add(new LevelItemInfo()
                {
                    Name = child.name,
                    X = pos.x,
                    Y = pos.y
                });
            }

            var document = new XmlDocument();
            var declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");
            document.AppendChild(declaration);

            var level = document.CreateElement("Level");
            document.AppendChild(level);
            
            foreach (var info in infos)
            {
                var levelItem = document.CreateElement("LevelItem");
                levelItem.SetAttribute("name", info.Name);
                levelItem.SetAttribute("x", info.X.ToString());
                levelItem.SetAttribute("y", info.Y.ToString());
                level.AppendChild(levelItem);
            }

            // var stringBuilder = new StringBuilder();
            // var stringWriter = new StringWriter(stringBuilder);
            // var xmlWriter = new XmlTextWriter(stringWriter);
            // xmlWriter.Formatting = Formatting.Indented;
            // document.WriteTo(xmlWriter);
            // Debug.Log(stringBuilder.ToString());

            var levelFilesFolder = Application.persistentDataPath + "/LevelFiles";

            Debug.Log(levelFilesFolder);

            if (!Directory.Exists(levelFilesFolder))
            {
                Directory.CreateDirectory(levelFilesFolder);
            }

            var levelFilePath = levelFilesFolder + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";

            document.Save(levelFilePath);
        }

        private void Update()
        {
            if (GUIUtility.hotControl > 0)
            {
                return;
            }
            
            var mousePosition = Input.mousePosition;
            var mouseWorldPos = mCamera.ScreenToWorldPoint(mousePosition);

            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;

            CheckRay(mousePosition);

            SetHighlightPos(mouseWorldPos);

            CheckInput(mouseWorldPos);
        }

        private void CheckRay(Vector3 mousePosition)
        {
            Ray ray = mCamera.ScreenPointToRay(mousePosition);
            var hit = Physics2D.Raycast(ray.origin, Vector2.zero, 20);
            if (hit.collider)
            {
                switch (mCurrentOperateMode)
                {
                    case OperateMode.Draw:
                        EmptyHighlight.color = new Color(1, 0, 0, 0.5f);
                        break;
                    case OperateMode.Erase:
                        EmptyHighlight.color = new Color(1, 0.5f, 0f, 0.5f);
                        break;
                }

                mCanDraw = false;
                mCurrentMouseSelectObj = hit.collider.gameObject;
            }
            else
            {
                switch (mCurrentOperateMode)
                {
                    case OperateMode.Draw:
                        EmptyHighlight.color = new Color(1, 1, 1, 0.5f);
                        break;
                    case OperateMode.Erase:
                        EmptyHighlight.color = new Color(0f, 0f, 1f, 0.5f);
                        break;
                }

                mCanDraw = true;
                mCurrentMouseSelectObj = null;
            }
        }

        private void SetHighlightPos(Vector3 mouseWorldPos)
        {
            //鼠标在相同位置
            if (Math.Abs(EmptyHighlight.transform.position.x - mouseWorldPos.x) < 0.1f &&
                Math.Abs(EmptyHighlight.transform.position.y - mouseWorldPos.y) < 0.1f)
            {
            }
            //鼠标在不同位置
            else
            {
                var emptyHighlightPos = mouseWorldPos;
                emptyHighlightPos.z = -9;
                EmptyHighlight.transform.position = emptyHighlightPos;
            }
        }

        private void CheckInput(Vector3 mouseWorldPos)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (mCanDraw)
                {
                    switch (mCurrentOperateMode)
                    {
                        case OperateMode.Draw:
                            var groundPrefab = Resources.Load<GameObject>("Ground");
                            var groundGameObj = Instantiate(groundPrefab, transform);
                            groundGameObj.transform.position = mouseWorldPos;
                            
                            switch (mCurrentBrushType)
                            {
                                case BrushType.Ground:
                                    groundGameObj.name = "Ground";
                                    break;
                                case BrushType.Player:
                                    groundGameObj.name = "Player";
                                    groundGameObj.GetComponent<SpriteRenderer>().color = Color.cyan;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    switch (mCurrentOperateMode)
                    {
                        case OperateMode.Erase:
                            if (mCurrentMouseSelectObj)
                            {
                                Destroy(mCurrentMouseSelectObj);

                                mCurrentMouseSelectObj = null;
                            }

                            break;
                    }
                }
            }
        }
    }
}