using PdfTools.UI.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfTools.UI.View
{
    /// <summary>
    /// Interaction logic for SheetMergeFileListView.xaml
    /// </summary>
    public partial class SheetMergeFileListView : UserControl
    {
        public SheetMergeFileListView()
        {
            InitializeComponent();
        }

        private void DragEnterFiles(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void DropFiles(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                SheetMergeFileListViewModel vm = (SheetMergeFileListViewModel)DataContext;
                vm.AddFiles(files);
            }
        }
    }
}
