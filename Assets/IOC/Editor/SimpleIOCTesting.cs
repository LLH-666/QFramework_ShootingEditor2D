using System.Collections;
using System.Collections.Generic;
using FrameworkDesign;
using NUnit.Framework;
using UnityEngine;

namespace SimpleIOCTests
{
    public class SimpleIOCTesting : MonoBehaviour
    {
        [Test]
        public void SimpleIOCRegisterResolveTest()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.Register<SimpleIOC>();

            var obj = simpleIOC.Resolve<SimpleIOC>();
            
            // 是否创建了实例
            Assert.IsNotNull(obj);

            // 不相同 说明是 创建了实例
            Assert.AreNotEqual(simpleIOC, obj);
        }

        [Test]
        public void SimpleIOCResolveRegisteredType()
        {
            var simpleIOC = new SimpleIOC();
            
            // 不进行注册
            var obj = simpleIOC.Resolve<SimpleIOC>();
            
            // 为空值时才应该测试通过
            Assert.IsNull(obj);
        }

        [Test]
        public void SimpleIOCRegisterTwice()
        {
            var simpleIOC = new SimpleIOC();

            simpleIOC.Register<SimpleIOC>();
            simpleIOC.Register<SimpleIOC>();
            
            Assert.IsTrue(true);
        }


        [Test]
        public void SimpleIOCRegisterInstance()
        {
            var simpleIOC = new SimpleIOC();

            simpleIOC.RegisterInstance(new SimpleIOC());
            
            var instanceA = simpleIOC.Resolve<SimpleIOC>();
            var instanceB = simpleIOC.Resolve<SimpleIOC>();

            Assert.AreEqual(instanceA, instanceB);
        }

        [Test]
        public void SimpleIOCRegisterDependency()
        {
            var simpleIOC = new SimpleIOC();

            simpleIOC.Register<ISimpleIOC, SimpleIOC>();

            var ioc = simpleIOC.Resolve<ISimpleIOC>();

            Assert.AreEqual(ioc.GetType(), typeof(SimpleIOC));
        }

        [Test]
        public void SimpleIOCRegisterInstanceDependency()
        {
            var simpleIOC = new SimpleIOC();

            simpleIOC.RegisterInstance<ISimpleIOC>(simpleIOC);

            var iocA = simpleIOC.Resolve<ISimpleIOC>();
            var iocB = simpleIOC.Resolve<ISimpleIOC>();

            Assert.AreEqual(iocA, simpleIOC);
            Assert.AreEqual(iocA, iocB);
        }

        class SomeDependencyA { }

        class SomeDependencyB { }

        class SomeCtrl
        {
            [SimpleIOCInject]
            public SomeDependencyA A { get; set; }
            
            [SimpleIOCInject]
            public SomeDependencyB B { get; set; }
        }

        [Test]
        public void SimpleIOCInject()
        {
            var simpleIOC = new SimpleIOC();

            simpleIOC.RegisterInstance(new SomeDependencyA());
            simpleIOC.Register<SomeDependencyB>();

            var someCtrl = new SomeCtrl();

            simpleIOC.Inject(someCtrl);
            
            Assert.IsNotNull(someCtrl.A);
            Assert.IsNotNull(someCtrl.B);

            Assert.AreEqual(someCtrl.A.GetType(), typeof(SomeDependencyA));
            Assert.AreEqual(someCtrl.B.GetType(), typeof(SomeDependencyB));
        }
        
        [Test]
        public void SimpleIOCClear()
        {
            var simpleIOC = new SimpleIOC();

            simpleIOC.RegisterInstance(new SomeDependencyA());
            simpleIOC.RegisterInstance<ISimpleIOC>(simpleIOC);
            simpleIOC.Register<SomeDependencyB>();

            simpleIOC.Clear();

            var someDependencyA = simpleIOC.Resolve<SomeDependencyA>();
            var someDependencyB = simpleIOC.Resolve<SomeDependencyB>();
            var ioc = simpleIOC.Resolve<ISimpleIOC>();

            Assert.IsNull(someDependencyA);
            Assert.IsNull(someDependencyB);
            Assert.IsNull(ioc);
        }
    }
}