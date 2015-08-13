using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace TD.Veritas.Sdk.Di
{
    public class BootstrapperRegistrationModule
    {
        public static void Register(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();

            RegisterInternal<TdVeritasAssemblyFinder>(container);
            RegisterInternal<SelfRegisteringModuleExecutor>(container);
            RegisterInternal<FileRegisteringModuleExecutor>(container);
            RegisterInternal<AssemblyRegistrar>(container);
        }

        private static void RegisterInternal<T>(IUnityContainer container)
        {
            if (!container.IsRegistered<T>())
            {
                var instance = container.Resolve<T>();
                container.RegisterInstance<T>(instance, new ContainerControlledLifetimeManager());
            }
        }
    }
}
