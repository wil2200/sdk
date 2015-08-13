using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace TD.Veritas.Sdk.Di
{
    /// <summary>
    /// This interface is used by DI to automatically create Unity registrations. Add any injectable dependencies into the 
    /// IUnityContainer parameter.
    /// You don't have to instantiate this class after implementing it -- DI will automatically detect and 
    /// execute the .Register() method upon initialization.
    /// </summary>
    public interface IRegistrationModule
    {
        void Register(IUnityContainer container);
    }
}
