//
// TypeEventSystem观察者模式
// 
using QFramework;
using UnityEngine;

namespace TESObserver
{
    /// <summary>
    /// 通知消息
    /// </summary>
    struct NotifyEvent
    {
    }

    /// <summary>
    /// 主题
    /// </summary>
    class Subject
    {
        public void DoObserverInterestedThings()
        {
            Notify();
        }

        private void Notify()
        {
            TypeEventSystem.Global.Send<NotifyEvent>();
        }
    }

    /// <summary>
    /// 观察者
    /// </summary>
    class Observer
    {
        public Observer()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            TypeEventSystem.Global.Register<NotifyEvent>(OnEvent);
        }

        private void OnEvent(NotifyEvent obj)
        {
            Debug.Log("Execute");
        }

        private void Dispose()
        {
            TypeEventSystem.Global.UnRegister<NotifyEvent>(OnEvent);
        }
    }

    public class TESObserver : MonoBehaviour
    {
        private void Start()
        {
            var subject = new Subject();
            var observerA = new Observer();
            var observerB = new Observer();
            var observerC = new Observer();
            
            subject.DoObserverInterestedThings();
        }
    }
}