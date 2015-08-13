using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace TD.Veritas.Sdk.Di
{
    public class FileRegisteringModuleExecutor
    {
        private readonly IUnityContainer container;

        public FileRegisteringModuleExecutor(IUnityContainer container)
        {
            this.container = container;
        }

        public void Execute(IList<string> skipList = null)
        {
            var skipListToLower = (skipList == null || !skipList.Any())
                ? new List<string>()
                : skipList.Select(s => s.ToLower()).ToList();

            TraverseUnityConfigs(ResolveAssemblyDirectory(Assembly.GetExecutingAssembly()), skipListToLower);
            TraverseUnityConfigs(ResolveAssemblyDirectory(Assembly.GetCallingAssembly()), skipListToLower);
        }

        private IList<string> ResolveAssemblyDirectory(Assembly assembly)
        {
            var fullUriPath = new Uri(assembly.GetName().CodeBase).LocalPath.ToUpper();
            var pathWithoutUnityFolder = fullUriPath.Replace(assembly.ManifestModule.Name.ToUpper(), "");
            var path = pathWithoutUnityFolder + @"Unity";

            if (!Directory.Exists(path)) return null;

            var files = Directory.GetFiles(path);
            return files;
        }

        private void TraverseUnityConfigs(IList<string> files, IList<string> skipList)
        {
            if (files == null || !files.Any()) return;

            foreach (var file in files)
            {
                if (!file.ToLower().EndsWith("xml")) continue;

                // if the skip list matches the xml file name (w/o the xml), then it's already been self registered - so skip it.
                var checkName = file.ToLower().Replace(".xml", "");
                if (skipList.Contains(checkName)) continue;

                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = file };

                var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

                container.LoadConfiguration(unitySection);
            }
        }
    }
}