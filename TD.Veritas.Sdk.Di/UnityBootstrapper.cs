using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace TD.Veritas.Sdk.Di
{
    public class UnityBootstrapper
    {
        private readonly static UnityBootstrapper instance = new UnityBootstrapper();
        public static UnityBootstrapper Instance { get { return instance; } }
        public IUnityContainer Container { get; private set; }

        private UnityBootstrapper()
        {
            Container = new UnityContainer();
            BootstrapperRegistrationModule.Register(Container);

            var registrar = Container.Resolve<AssemblyRegistrar>();
            registrar.RegisterVeritasModules();
        }

        public IEnumerable<string> GetVersion()
        {
            var list = new List<string>();

            foreach (var reg in Container.Registrations)
            {
                if (!list.Contains(reg.RegisteredType.Assembly.FullName))
                    list.Add(reg.RegisteredType.Assembly.FullName);
            }

            return list;
        }

        public void RegisterByNamespace(Assembly assembly,string ns)
        {
            var types = from t in assembly.GetTypes()
                    where t.IsClass && t.Namespace == ns
                    select t;

            foreach (var type in types)
                Container.RegisterType(type, type);
        }
    }
}
