using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TD.Veritas.Sdk.Di
{
    /// <summary>
    /// This interface is used by DI to automatically create class-to-interface/class-to-class mappings
    /// You don't have to instantiate this class after implementing it -- DI will automatically detect and 
    /// execute the .CreateMapping() method upon initialization.
    /// </summary>
    public interface IMapper
    {
        void CreateMapping();
    }
}
