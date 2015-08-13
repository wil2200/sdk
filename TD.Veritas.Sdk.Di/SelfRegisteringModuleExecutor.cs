using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace TD.Veritas.Sdk.Di
{
    public class SelfRegisteringModuleExecutor
    {
        private readonly IUnityContainer container;
        
        public SelfRegisteringModuleExecutor(IUnityContainer container)
        {
            this.container = container;
        }

        public IList<string> Execute(IList<FileInfo> registerModuleFiles)
        {
            var registeredList = new List<string>();

            foreach (var assemblyFile in registerModuleFiles)
            {
                var assemblyToRegister = Assembly.LoadFrom(assemblyFile.FullName);

                var exportedTypes = assemblyToRegister.GetExportedTypes();
                var registerInterfaces =
                    exportedTypes.Where(t =>
                        typeof(IRegistrationModule).IsAssignableFrom(t) &&
                        !t.IsAbstract &&
                        !t.IsInterface).ToList();

                if (registerInterfaces.Any())
                {
                    var registerList =
                        registerInterfaces.Select(t => (IRegistrationModule)Activator.CreateInstance(t)).ToList();

                    foreach (var registerModule in registerList)
                    {
                        registerModule.Register(container);
                    }

                    registeredList.Add(assemblyFile.Name);
                }
            }

            return registeredList;
        }
    }
}