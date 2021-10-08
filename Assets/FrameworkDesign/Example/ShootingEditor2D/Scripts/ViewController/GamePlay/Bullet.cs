using System;
using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Bullet : MonoBehaviour, IController
    {
        private Rigidbody2D mRigidbody2D;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();

            Destroy(gameObject, 5);
        }

        private void Start()
        {
            var isRight = Mathf.Sign(transform.lossyScale.x);
            mRigidbody2D.velocity = Vector2.right * 10 * isRight;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                this.SendCommand<KillEnemyCommand>();

                Destroy(other.gameObject);

                Destroy(gameObject);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }
    }
}