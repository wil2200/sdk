#if DEBUG

namespace TD.Veritas.Sdk.Di.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Family;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using NUnit.Framework;
    using Ui;

    [TestFixture]
    public class BootstrapperTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            try
            {
                UnityBootstrapper.Instance.Container.RegisterType<IGreeting, Greeting>();
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                throw;
            }
            
        }

        [Test]
        public void VerifySimpleResolution()
        {
            // arrange
            var greeter = UnityBootstrapper.Instance.Container.Resolve<IGreeting>();

            // act
            var response = greeter.SayHello();

            // assert
            Assert.That(response,Is.EqualTo("Hello"));
        }

        [Test]
        public void VerifyInterception()
        {
            // arrange
            UnityBootstrapper.Instance.Container.RegisterType<ICrankyGreeter, CrankyGreeter>(new InterceptionBehavior<RudeGreeter>(), new Interceptor(new InterfaceInterceptor()));
            var greeter = UnityBootstrapper.Instance.Container.Resolve<ICrankyGreeter>();

            // act
            var response = greeter.SayHello();

            // assert
            Assert.That(response, Is.Not.EqualTo("Hello"));
        }

        [Test]
        public void VerifyNamespaceRegistration()
        {
            // arrange
            UnityBootstrapper.Instance.RegisterByNamespace(Assembly.GetAssembly(GetType()), "TD.Veritas.Sdk.Di.Tests.Family");

            // act
            var parent = UnityBootstrapper.Instance.Container.Resolve<Parent>();

            // assert
            Assert.That(parent, Is.Not.Null);
        }

        [Test]
        public void VerifyGetVersion()
        {
            // arrange
            var versions = UnityBootstrapper.Instance.GetVersion();
            var typeName = GetType().Assembly.FullName;

            Assert.That(versions.Contains(typeName));
        }

        [Test]
        [STAThread]
        public void VerifyBaseViewModelRegistration()
        {
            // arrange
            UnityBootstrapper.Instance.RegisterByNamespace(Assembly.GetAssembly(GetType()), "TD.Veritas.Sdk.Di.Tests.Family");

            var view = new TestView();
            
            // act
            var dc = view.DataContext as TestViewModel;

            // assert
            Assert.That(dc, Is.Not.Null);
        }
    }

    #region test types

    public interface IGreeting
    {
        string SayHello();
    }

    public class Greeting : IGreeting
    {
        public string SayHello()
        {
            return "Hello";
        }
    }

    public interface ICrankyGreeter 
    {
        string SayHello();
    }

    public class CrankyGreeter : ICrankyGreeter
    {
        public string SayHello()
        {
            return "Hello";
        }
    }

    public class RudeGreeter :IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var returnValue = input.CreateMethodReturn("Bugger Off Wanker!");

            return returnValue;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return new List<Type> {typeof (IGreeting)};
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }

    namespace Family
    {
        public class Parent
        {
            public class Child
            {

            }

        }

        public class TestView : BaseView<TestViewModel>
        {

        }
    }

    public class TestViewModel
    {
        
    }



    #endregion
}

#endif