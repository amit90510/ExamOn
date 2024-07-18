using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ExamOn.ServiceLayer
{
    public class iTextPdfExportService
    {
        byte[] pdfData = null;
        public byte[] ExportPdfData(IElement[] pdfElement)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Add content to the PDF
                foreach (IElement element in pdfElement)
                {
                    document.Add(element);
                }

                document.Close();
                writer.Close();

                byte[] pdfData = ms.ToArray();
            }

            return pdfData;
        }
    }
}