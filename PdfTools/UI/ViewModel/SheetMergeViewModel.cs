using JMI.General;
using PdfTools.Model.SheetMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTools.UI.ViewModel
{
    class SheetMergeViewModel : ObservableObject
    {
        #region constructors
        public SheetMergeViewModel()
        {
            sheetMerger = new SheetMerger();
            MergeSettings = new SheetMergeSettingsViewModel(sheetMerger.Settings);
            FileList = new SheetMergeFileListViewModel(sheetMerger);
        }
        #endregion

        #region properties
        private SheetMerger sheetMerger;

        private SheetMergeSettingsViewModel mergeSettings;
        public SheetMergeSettingsViewModel MergeSettings
        {
            get { return mergeSettings; }
            private set { SetProperty(ref mergeSettings, value); }
        }

        private SheetMergeFileListViewModel fileListView;
        public SheetMergeFileListViewModel FileList
        {
            get { return fileListView; }
            private set { SetProperty(ref fileListView, value); }
        }
        #endregion

        #region commands
        #endregion

        #region methods
        public void SaveSettings()
        {
            sheetMerger.Settings.SaveSettings();
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}
