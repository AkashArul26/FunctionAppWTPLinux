using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System.Net.Http;
using System.Net;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Http.Headers;

namespace WordToPDFAzureFunc
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            TraceWriter log)
        {
            //Get the input Word document as stream from request
            Stream stream = req.Content.ReadAsStreamAsync().Result;
            //Loads an existing Word document
            using (WordDocument document = new WordDocument(stream, FormatType.Docx))
            {
                document.LastParagraph.AppendText("New content");
                //Creates an instance of the DocToPDFConverter
                using (DocIORenderer render = new DocIORenderer())
                {
                    //Converts Word document into PDF document
                    using (PdfDocument pdfDocument = render.ConvertToPDF(document))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        //Saves the PDF file 
                        pdfDocument.Save(memoryStream);
                        //Reset the memory stream position
                        memoryStream.Position = 0;
                        //Create the response to return
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        //Set the Word document saved stream as content of response
                        response.Content = new ByteArrayContent(memoryStream.ToArray());
                        //Set the contentDisposition as attachment
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "Sample.Pdf"
                        };
                        //Set the content type as Word document mime type
                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                        //Return the response with output Word document stream
                        return response;
                    }
                }
            }
        }
    }
}
