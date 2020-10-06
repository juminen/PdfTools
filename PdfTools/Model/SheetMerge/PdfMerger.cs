using JMI.General.Logging;
using JMI.General.Sorting;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PdfTools.Model.SheetMerge
{
    class PdfMerger
    {
        private static Logger logger = SingletonLogger.Instance;

        public static void MergePdfs(IEnumerable<SheetSet> sets, MergeSettings settings)
        {
            foreach (SheetSet set in sets)
            {
                string msg = $"Creating pdf '{set.NameWithExtension}'...";
                logger.Log(LogFactory.CreateNormalMessage(msg));
                PdfDocument outputPdf = new PdfDocument();
                foreach (Sheet sheet in set.GetSheetsInOrder())
                {
                    PdfDocument inputPdf = PdfReader.Open(sheet.FullPath, PdfDocumentOpenMode.Import);
                    for (int i = 0; i < inputPdf.PageCount; i++)
                    {
                        PdfPage page = inputPdf.Pages[i];
                        outputPdf.Pages.Add(page);
                        PdfOutline sheetBookMark = new PdfOutline
                        {
                            Title = sheet.NameWithoutExtension,
                            TextColor = PdfSharp.Drawing.XColor.FromKnownColor(PdfSharp.Drawing.XKnownColor.Blue)
                        };

                        if (inputPdf.PageCount > 1)
                        {
                            sheetBookMark.Title = $"{sheetBookMark.Title}_{i + 1}";
                        }
                        sheetBookMark.DestinationPage = outputPdf.Pages[outputPdf.PageCount - 1];
                        outputPdf.Outlines.Add(sheetBookMark);
                    }
                }

                outputPdf.Info.Title = set.NameWithoutExtension;
                outputPdf.Info.Author = settings.Author;
                outputPdf.Info.Creator = settings.Creator;
                outputPdf.Info.Subject = settings.Subject;
                outputPdf.Info.Keywords = settings.Keywords;

                SavePdfFile(outputPdf, set.FullPath);
            }

            if (settings.CreateCombinationFile)
            {
                PdfDocument combinationPdf = new PdfDocument();
                foreach (SheetSet set in sets.OrderBy(x => x.NameWithoutExtension, new AlphanumStringComparatorFast()))
                {
                    PdfOutline sheetSetBookMark = new PdfOutline
                    {
                        Title = set.NameWithoutExtension,
                        TextColor = PdfSharp.Drawing.XColor.FromKnownColor(PdfSharp.Drawing.XKnownColor.Green)
                    };
                    bool addSheetSetBookMark = true;
                    combinationPdf.Outlines.Add(sheetSetBookMark);
                    foreach (Sheet sheet in set.GetSheetsInOrder())
                    {
                        PdfDocument inputPdf = PdfReader.Open(sheet.FullPath, PdfDocumentOpenMode.Import);
                        for (int i = 0; i < inputPdf.PageCount; i++)
                        {
                            PdfPage page = inputPdf.Pages[i];
                            combinationPdf.Pages.Add(page);

                            if (i == 0 && addSheetSetBookMark)
                            {
                                sheetSetBookMark.DestinationPage = combinationPdf.Pages[combinationPdf.PageCount - 1];
                                addSheetSetBookMark = false;
                            }

                            PdfOutline sheetBookMark = new PdfOutline
                            {
                                Title = sheet.NameWithoutExtension,
                                TextColor = PdfSharp.Drawing.XColor.FromKnownColor(PdfSharp.Drawing.XKnownColor.Blue)
                            };

                            if (inputPdf.PageCount > 1)
                            {
                                sheetBookMark.Title = $"{sheetBookMark.Title}_{i + 1}";
                            }
                            sheetBookMark.DestinationPage = combinationPdf.Pages[combinationPdf.PageCount - 1];
                            sheetSetBookMark.Outlines.Add(sheetBookMark);
                        }
                    }
                }

                combinationPdf.Info.Author = settings.Author;
                combinationPdf.Info.Creator = settings.Creator;
                combinationPdf.Info.Subject = settings.Subject;
                combinationPdf.Info.Keywords = settings.Keywords;

                SavePdfFile(combinationPdf, settings.CombinationFilePath);
            }
        }

        private static void SavePdfFile(PdfDocument document, string fullPathToFile)
        {
            try
            {
                document.Save(fullPathToFile);
                string msg = $"Pdf file '{fullPathToFile}' saved.";
                logger.Log(LogFactory.CreateNormalMessage(msg));
            }
            catch (Exception ex)
            {
                string msg = $"Saving pdf '{fullPathToFile}' failed:\n{ex.Message}";
                logger.Log(LogFactory.CreateErrorMessage(msg));
            }
        }
    }
}
