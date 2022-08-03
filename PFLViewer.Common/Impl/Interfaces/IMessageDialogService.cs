using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLFViewer.Common.Impl.Interfaces
{
    public interface IMessageDialogService
    {
        string Title { get; set; }
        string Message { get; set; }
        DialogButtonSet ButtonSet { get; set; }

        bool? ShowMessage();
    }

    public enum DialogButtonSet
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 3,
        YesNo = 4
    }
}
