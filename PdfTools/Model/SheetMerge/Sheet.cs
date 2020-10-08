using JMI.General.Identifiers;
using JMI.General.Selections;

namespace PdfTools.Model.SheetMerge
{
    class Sheet : BaseFile, ISelectionTarget
    {
        #region constructors
        public Sheet(string pathToFile, string sheetSeparator)
            : base(pathToFile)
        {
            Identifier = new StringIdentifier(pathToFile);
            SheetSeparator = sheetSeparator;
            ConstructProperties();
        }
        #endregion

        #region properties
        public IIdentifier Identifier { get; }
        /// <summary>
        /// Sheet set identifier (= file name without extension)
        /// </summary>
        public string SheetSet { get; private set; }
        /// <summary>
        /// String between sheet separator and file extension dot
        /// </summary>
        public string SheetIdentifier { get; private set; }
        /// <summary>
        /// String in file name that separates sheet set identifier from sheet identifier.
        /// </summary>
        public string SheetSeparator { get; set; }
        #endregion

        #region methods
        private void ConstructProperties()
        {
            //Can not use String.Split because this method fails 
            //if there are multiple sheet separator strings in file name.
            SheetSet = NameWithoutExtension.Substring(0, NameWithoutExtension.LastIndexOf(SheetSeparator));
            SheetIdentifier= NameWithoutExtension.Substring(NameWithoutExtension.LastIndexOf(SheetSeparator) + SheetSeparator.Length);
        }
        #endregion

        #region events
        #endregion

        #region event handlers
        #endregion
    }
}
