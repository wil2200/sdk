using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TD.Veritas.Sdk.Di
{
    public class TdVeritasAssemblyFinder
    {
        public IList<FileInfo> GetAssemblies()
        {
            var executingAssmDirs = new[]
            {
                new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath.ToUpper(),
                new Uri(Assembly.GetCallingAssembly().GetName().CodeBase).LocalPath.ToUpper()
            };

            var assemblyFiles = executingAssmDirs.SelectMany(
                dir =>
                    (new DirectoryInfo(Path.GetDirectoryName(dir))).GetFiles("td.veritas.*.dll")
                        .Concat((new DirectoryInfo(Path.GetDirectoryName(dir)).GetFiles("td.veritas.*.exe"))));

            return assemblyFiles.ToList();
        }
    }
}
