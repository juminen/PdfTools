using JMI.General.Logging;
using JMI.General.VM.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PdfTools.UI.ViewModel
{
    class ApplicationMainViewModel : BaseApplicationViewModel
    {
        #region constructors
        public ApplicationMainViewModel()
        {
            WindowTitle = "Pdf Tools";
            ReadSettings();
            SheetMerger = new SheetMergeViewModel();
        }
        #endregion

        #region properties
        readonly Logger logger = SingletonLogger.Instance;

        private SheetMergeViewModel sheetMerger;
        public SheetMergeViewModel SheetMerger
        {
            get { return sheetMerger; }
            private set { SetProperty(ref sheetMerger, value); }
        }
        #endregion

        #region commands
        #endregion

        #region methods
        private void ReadSettings()
        {
            #region window place
            if (Properties.Settings.Default.WindowTop < 0 ||
                Properties.Settings.Default.WindowTop > SystemParameters.VirtualScreenHeight)
            {
                WindowTop = 0;
            }
            else
            {
                WindowTop = Properties.Settings.Default.WindowTop;
            }

            if (Properties.Settings.Default.WindowLeft < 0 ||
                Properties.Settings.Default.WindowLeft > SystemParameters.VirtualScreenWidth)
            {
                WindowLeft = 0;
            }
            else
            {
                WindowLeft = Properties.Settings.Default.WindowLeft;
            }
            #endregion window place

            #region window size
            if (Properties.Settings.Default.WindowHeight < 300)
            {
                WindowHeight = SystemParameters.PrimaryScreenHeight;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.WindowHeight;
            }

            if (Properties.Settings.Default.WindowWidht < 300)
            {
                WindowWidht = SystemParameters.PrimaryScreenWidth;
            }
            else
            {
                WindowWidht = Properties.Settings.Default.WindowWidht;
            }

            if (Properties.Settings.Default.WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = Properties.Settings.Default.WindowState;
            }
            #endregion window size

            RowHeightTop = new GridLength(Properties.Settings.Default.GridRowHeightTop, GridUnitType.Star);
            RowHeightBottom = new GridLength(Properties.Settings.Default.GridRowHeightBottom, GridUnitType.Star);
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.WindowTop = WindowTop;
            Properties.Settings.Default.WindowLeft = WindowLeft;
            Properties.Settings.Default.WindowHeight= WindowHeight;
            Properties.Settings.Default.WindowWidht = WindowWidht;
            Properties.Settings.Default.WindowState = WindowState;
            Properties.Settings.Default.GridRowHeightTop = RowHeightTop.Value;
            Properties.Settings.Default.GridRowHeightBottom = RowHeightBottom.Value;
            Properties.Settings.Default.Save();
            SheetMerger.SaveSettings();            
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}
