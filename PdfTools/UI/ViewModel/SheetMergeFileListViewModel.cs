using JMI.General;
using JMI.General.Selections;
using JMI.General.Sorting;
using JMI.General.VM.Commands;
using JMI.General.VM.Selections;
using PdfTools.Model.SheetMerge;
using System.Collections;
using System.Collections.Generic;

namespace PdfTools.UI.ViewModel
{
    class SheetMergeFileListViewModel : SelectionCollectionViewModel<Sheet, SheetMergeFileListItemViewModel>
    {
        #region constructors
        public SheetMergeFileListViewModel(SheetMerger merger) : base(merger.Sheets)
        {
            SetSorting();
            sheetMerger = merger;
            AddCommandGroup(MergeGroup);
        }
        #endregion

        #region properties
        private readonly SheetMerger sheetMerger;
        #endregion

        #region commands
        private CommandViewModel mergeFilesCommand;
        public CommandViewModel MergeFilesCommand
        {
            get
            {
                if (mergeFilesCommand == null)
                {
                    RelayCommand clearListRelay =
                        new RelayCommand(
                            param => sheetMerger.MergeSheets(),
                            param => sheetMerger.Sheets.AllItems.Count > 1);
                    mergeFilesCommand = new CommandViewModel("Merge files", clearListRelay);
                }
                return mergeFilesCommand;
            }
        }
        
        private CommandGroupViewModel mergeGroup;
        public CommandGroupViewModel MergeGroup
        {
            get
            {
                if (mergeGroup == null)
                {
                    mergeGroup = new CommandGroupViewModel("Merge");
                    foreach (CommandViewModel item in CreateCommands())
                    {
                        mergeGroup.Commands.Add(item);
                    }
                }
                return mergeGroup;
            }
        }
        #endregion

        #region methods
        protected override SheetMergeFileListItemViewModel CreateViewModel(ISelectionItem<Sheet> item)
        {
            return new SheetMergeFileListItemViewModel(item);
        }

        private IList<CommandViewModel> CreateCommands()
        {
            List<CommandViewModel> list = new List<CommandViewModel>()
            {
                MergeFilesCommand
            };
            return list;
        }

        private void SetSorting()
        {
            ClearSorting();
            AllItems.CustomSort = new SheetComparer();
        }

        public void AddFiles(IEnumerable<string> filePaths)
        {
            List<string> list = new List<string>();
            foreach (string item in filePaths)
            {
                if (System.IO.File.Exists(item))
                {
                    sheetMerger.AddSheet(item);
                }
            }
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion

        private class SheetComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                SheetMergeFileListItemViewModel a = x as SheetMergeFileListItemViewModel;
                SheetMergeFileListItemViewModel b = y as SheetMergeFileListItemViewModel;
                if (a != null && b != null)
                {
                    AlphanumStringComparatorFast comp = new AlphanumStringComparatorFast();
                    return comp.Compare(a.FilePath, b.FilePath);
                }
                return -1;
            }
        }
    }
}
