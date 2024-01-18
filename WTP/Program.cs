using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;

using (FileStream docStream = new FileStream("../../../Template.docx", FileMode.Open, FileAccess.Read))
{
    // Loads file stream into Word document
    using (WordDocument wordDocument = new WordDocument(docStream, Syncfusion.DocIO.FormatType.Automatic))
    {
        // Instantiation of DocIORenderer for Word to PDF conversion
        using (DocIORenderer render = new DocIORenderer())
        {
            // Sets true to embed complete TrueType fonts
            render.Settings.EmbedCompleteFonts = true;

            // Converts Word document into PDF document
            PdfDocument pdfDocument = render.ConvertToPDF(wordDocument);
            // Saves the PDF file
            using (FileStream outputStream = new FileStream(@"Output.pdf", FileMode.OpenOrCreate))
            {
                pdfDocument.Save(outputStream);
                // Closes the instance of PDF document object
                pdfDocument.Close();
            }
        }
    }
}