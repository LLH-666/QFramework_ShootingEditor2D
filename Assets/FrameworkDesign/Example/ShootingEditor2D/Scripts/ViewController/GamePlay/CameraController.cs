using System;
using UnityEngine;

namespace ShootingEditor2D
{
    public class CameraController : MonoBehaviour
    {
        private Transform mPlayerTran;

        private float mMinX = -5;
        private float mMaX = 5;
        private float mMinY = -5;
        private float mMaxY = 5;

        private Vector3 mTargetPos;

        private void Awake()
        {
            mPlayerTran = GameObject.FindWithTag("Player").transform;
        }

        private void LateUpdate()
        {
            var cameraPos = transform.position;
            var isRight = Mathf.Sign(mPlayerTran.transform.localScale.x);
            
            var playerPos = mPlayerTran.position;
            mTargetPos.x = playerPos.x + 3 * isRight;
            mTargetPos.y = playerPos.y + 2;
            mTargetPos.z = -10;

            var smoothSpeed = 5;
            
            //增加一个平滑处理
            var position = cameraPos;
            position = Vector3.Lerp(position, mTargetPos, smoothSpeed * Time.deltaTime);
            
            //锁定在一个固定区域
            transform.position = new Vector3(Mathf.Clamp(position.x, mMinX, mMaX),
                Mathf.Clamp(position.y, mMinY, mMaxY), position.z);
        }
    }
}