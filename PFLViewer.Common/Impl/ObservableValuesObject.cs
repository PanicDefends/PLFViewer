using Microsoft.Toolkit.Mvvm.ComponentModel;
using PLFViewer.Common.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl
{
    public class ObservableValuesObject : ObservableObject, INotifyPropertyValueChanged
    {
        public event PropertyValueChangedEventHandler PropertyValueChanged;

        protected void OnPropertyValueChanged(object oldValue, object newValue, [CallerMemberName] string propertyName = null)
        {
            var e = new PropertyValueChangedEventArgs(propertyName, oldValue, newValue);
            this.PropertyValueChanged?.Invoke(this, e);
        }
    }
}
