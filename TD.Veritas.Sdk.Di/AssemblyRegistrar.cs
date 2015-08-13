
namespace TD.Veritas.Sdk.Di
{
    public class AssemblyRegistrar
    {
        private readonly TdVeritasAssemblyFinder assemblyFinder;
        private readonly SelfRegisteringModuleExecutor selfRegisteringModuleExecutor;
        private readonly FileRegisteringModuleExecutor fileRegisteringModuleExecutor;

        public AssemblyRegistrar(TdVeritasAssemblyFinder assemblyFinder, 
            SelfRegisteringModuleExecutor selfRegisteringModuleExecutor, 
            FileRegisteringModuleExecutor fileRegisteringModuleExecutor)
        {
            this.assemblyFinder = assemblyFinder;
            this.selfRegisteringModuleExecutor = selfRegisteringModuleExecutor;
            this.fileRegisteringModuleExecutor = fileRegisteringModuleExecutor;
        }

        public void RegisterVeritasModules()
        {
            var assemblyFiles = assemblyFinder.GetAssemblies();
            var skipList = selfRegisteringModuleExecutor.Execute(assemblyFiles);
            fileRegisteringModuleExecutor.Execute(skipList);
        }
    }
}
