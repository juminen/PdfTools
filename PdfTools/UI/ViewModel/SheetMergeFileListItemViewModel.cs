using JMI.General.Selections;
using JMI.General.VM.Selections;
using PdfTools.Model.SheetMerge;

namespace PdfTools.UI.ViewModel
{
    class SheetMergeFileListItemViewModel : SelectionItemViewModel<Sheet>
    {
        #region constructors
        public SheetMergeFileListItemViewModel(ISelectionItem<Sheet> item)
            : base(item)
        {
            sheet = item.Target;
        }
        #endregion

        #region properties
        private readonly Sheet sheet;

        public string FileName { get { return sheet.NameWithExtension; } }
        public string SheetSet { get { return sheet.SheetSet; } }
        public string Sheet { get { return sheet.SheetIdentifier; } }
        public string SheetSeparator { get { return sheet.SheetSeparator; } }
        public string FilePath { get { return sheet.FullPath; } }
        #endregion

        #region commands
        #endregion

        #region methods
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}
