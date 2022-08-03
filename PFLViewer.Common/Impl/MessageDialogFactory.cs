using PLFViewer.Common.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl
{
    public class MessageDialogFactory
    {
        public static IMessageDialogService GetMessageDialog(string title, string message, DialogButtonSet buttonSet)
            => new Win32MessageDialogService() { Title = title, Message = message, ButtonSet = buttonSet };
    }
}
