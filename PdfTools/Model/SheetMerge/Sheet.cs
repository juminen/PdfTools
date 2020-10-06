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
            Identifier = new Identifier();
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
            //string[] splitter = new string[] { SheetSeparator };
            //string[] split = NameWithoutExtension.Split(splitter, System.StringSplitOptions.RemoveEmptyEntries);

            //if (split.Length == 2)
            //{
            //    SheetSet = split[0];
            //    SheetIdentifier = split[1];
            //}
            //else if (split.Length > 2)
            //{
            //    for (int i = 0; i < split.Length - 2; i++)
            //    {
            //        SheetSet = SheetSet + split[i];
            //    }
            //    SheetIdentifier = split[split.Length - 1];
            //}
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
