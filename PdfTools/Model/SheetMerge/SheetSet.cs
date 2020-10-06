using JMI.General.Selections;
using JMI.General.Sorting;
using System.Collections.Generic;
using System.Linq;

namespace PdfTools.Model.SheetMerge
{
    class SheetSet : BaseFile
    {
        #region constructors
        public SheetSet(string pathToFile)
            : base(pathToFile)
        {
            Sheets = new SelectionCollection<Sheet>();
        }
        #endregion

        #region properties
        public ISelectionCollection<Sheet> Sheets { get; private set; }
        #endregion

        #region methods
        public bool ContainsSheet(Sheet sheet)
        {
            bool result = false;

            foreach (Sheet s in Sheets.GetAllTargetItems())
            {
                if (s.FullPath.Equals(sheet.FullPath))
                {
                    return true;
                }
            }
            return result;
        }

        public IEnumerable<Sheet> GetSheetsInOrder()
        {
            return Sheets.GetAllTargetItems().ToList()
                .OrderBy(x => x.SheetIdentifier, new AlphanumStringComparatorFast())
                .ToList();
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}