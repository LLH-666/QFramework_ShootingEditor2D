using System;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelPlayer : MonoBehaviour
    {
        //public TextAsset LevelFile;

        private enum State
        {
            Selection,
            Playing
        }

        private static string mLevelFilesFolder;

        private State mCurrentState = State.Selection;

        private void Awake()
        {
            mLevelFilesFolder = Application.persistentDataPath + "/LevelFiles";
        }

        private void OnGUI()
        {
            if (mCurrentState == State.Selection)
            {
                var filePaths = Directory.GetFiles(mLevelFilesFolder);
                int y = 10;
                foreach (var filePath in filePaths.Where(f => f.EndsWith("xml")))
                {
                    var fileName = Path.GetFileName(filePath);

                    var buttonRect = new Rect(10, y, 150, 50);
                    if (GUI.Button(buttonRect, fileName))
                    {
                        var xml = File.ReadAllText(filePath);

                        ParseAndRun(xml);

                        mCurrentState = State.Playing;
                    }

                    y += 50;
                }
            }
        }

        private void ParseAndRun(string xml)
        {
            var document = new XmlDocument();

            document.LoadXml(xml);

            var levelNode = document.SelectSingleNode("Level");

            foreach (XmlElement levelItemNode in levelNode.ChildNodes)
            {
                var levelItemName = levelItemNode.Attributes["name"].Value;
                var levelItemX = int.Parse(levelItemNode.Attributes["x"].Value);
                var levelItemY = int.Parse(levelItemNode.Attributes["y"].Value);

                var levelItemPrefab = Resources.Load<GameObject>(levelItemName);
                var levelItemGameObj = Instantiate(levelItemPrefab, transform);
                levelItemGameObj.transform.position = new Vector3(levelItemX, levelItemY);
            }
        }
    }
}