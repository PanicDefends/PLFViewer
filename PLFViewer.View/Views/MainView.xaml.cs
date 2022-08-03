using PLFViewer.Common.Impl;
using PLFViewer.ViewModel.VMs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PLFViewer.View.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainVM(SerializationServiceFactory.Instance);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var vm = DataContext as MainVM;

            if (!vm.SerializationHelper.HasUnsavedChanges)
                return;

            var approveClosing = vm.OnClosing?.Invoke();
            e.Cancel = approveClosing != true;
        }
    }
}
