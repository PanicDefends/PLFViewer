using PLFViewer.Common.Impl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLFViewer.Common.Impl
{
    internal class Win32MessageDialogService : IMessageDialogService
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DialogButtonSet ButtonSet { get; set; } = DialogButtonSet.OK;

        public bool? ShowMessage()
        {
            var messageBoxButton = Enum.TryParse(ButtonSet.ToString(), out MessageBoxButton buttonSet)
                ? buttonSet
                : MessageBoxButton.OK;
            var result = MessageBox.Show(Message, Title, messageBoxButton, MessageBoxImage.None, MessageBoxResult.Cancel);

            switch (result)
            {
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                case MessageBoxResult.Cancel:
                    return null;
            }

            return null;
        }
    }

}
