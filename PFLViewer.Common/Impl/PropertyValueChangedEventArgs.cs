using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl
{
    public class PropertyValueChangedEventArgs : PropertyChangedEventArgs
    {
        protected readonly object _oldValue;
        protected readonly object _newValue;

        public PropertyValueChangedEventArgs(string propertyName, object oldValue, object newValue) 
            : base(propertyName)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public virtual object OldValue => _oldValue;
        public virtual object NewValue => _newValue;
    }
}
