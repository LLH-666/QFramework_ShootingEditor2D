using QFramework;
using UnityEngine;

namespace QFramework.Example
{
    public class GlobalEventExample : MonoBehaviour
        , IOnEvent<GlobalEventExample.GlobalEventA>
        , IOnEvent<GlobalEventExample.GlobalEventB>
    {
        public struct GlobalEventA
        {
        }

        public struct GlobalEventB
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            TypeEventSystem.Global.Register<GlobalEventA>(OnGlobalEventA).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<GlobalEventA>();
            this.RegisterEvent<GlobalEventB>();

            var value = new BindableProperty<int>(10);
            value.RegisterWithInitValue((int v) =>
            {
                Debug.Log(v);
            });
        }

        private void OnDestroy()
        {
            this.UnRegisterEvent<GlobalEventA>();
            this.UnRegisterEvent<GlobalEventB>();
        }

        private void OnGlobalEventA(GlobalEventA obj)
        {
            Debug.Log(obj.ToString());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TypeEventSystem.Global.Send<GlobalEventA>();
                TypeEventSystem.Global.Send<GlobalEventB>();
            }
        }

        public void OnEvent(GlobalEventA e)
        {
            Debug.Log($"onevent:" + e.ToString());
        }

        public void OnEvent(GlobalEventB e)
        {
            Debug.Log($"onevent:" + e.ToString());
        }
    }
}