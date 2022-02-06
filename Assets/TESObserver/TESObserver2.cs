//
// 经典观察者模式
// 

using System.Collections.Generic;
using UnityEngine;

namespace TESObserver2
{
    /// <summary>
    /// 主题
    /// </summary>
    abstract class Subject
    {
        private List<Observer> mObservers = new List<Observer>();

        public void Attach(Observer observer)
        {
            mObservers.Add(observer);
        }

        public void Remove(Observer observer)
        {
            mObservers.Remove(observer);
        }
        
        protected void Notify()
        {
            mObservers.ForEach(observer => observer.Update());
        }
    }

    /// <summary>
    /// 观察者
    /// </summary>
    abstract class Observer
    {
        public abstract void Update();
    }

    class ConCreteSubject : Subject
    {
        private string mState;

        public void SetState(string state)
        {
            mState = state;
            
            Notify();
        }

        public string GetState()
        {
            return mState;
        }
    }

    class ConCreteObserver : Observer
    {
        private ConCreteSubject mSubject;

        public ConCreteObserver(ConCreteSubject subject)
        {
            mSubject = subject;
        }

        public override void Update()
        {
            Debug.Log(mSubject.GetState());
        }
    }

    public class TESObserver2 : MonoBehaviour
    {
        private void Start()
        {
            var subject = new ConCreteSubject();
            var observer = new ConCreteObserver(subject);
            
            subject.Attach(observer);
            subject.SetState("test");
        }
    }
}