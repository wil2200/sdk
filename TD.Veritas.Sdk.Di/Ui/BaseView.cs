using System;
using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace TD.Veritas.Sdk.Di.Ui
{
    public abstract class BaseView<TViewModel> : UserControl, IDisposable
    {
        protected BaseView()
        {
            var viewModelType = this.GetType().BaseType.GetGenericArguments()[0];
            DataContext = UnityBootstrapper.Instance.Container.Resolve(viewModelType);
        }

        public virtual void Dispose()
        {
            var dispose = DataContext as IDisposable;
            if (dispose != null)
                dispose.Dispose();
        }
    }
}
