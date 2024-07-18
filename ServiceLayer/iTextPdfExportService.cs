using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;

namespace ExamOn.ServiceLayer
{
    public class iTextPdfExportService
    {
        byte[] pdfData = null;
        public byte[] ExportPdfData(List<IElement> pdfElement)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 2f, 2f, 2f, 2f);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                var content = writer.DirectContent;
                var pageBorderRect = new iTextSharp.text.Rectangle(document.PageSize);
                pageBorderRect.Left += document.LeftMargin;
                pageBorderRect.Right -= document.RightMargin;
                pageBorderRect.Top -= document.TopMargin;
                pageBorderRect.Bottom += document.BottomMargin;
                content.SetColorStroke(BaseColor.BLACK);
                content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                content.Stroke();              

                //default content
                PdfPTable pdfPTableHeader = new PdfPTable(2);
                float[] widths = new float[] { 40f, 60f};
                pdfPTableHeader.SetWidths(widths);
                // add a image
                iTextSharp.text.Image jpgFASTagLog = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Content/logo/logo.png"));
                jpgFASTagLog.ScaleToFit(40f, 40f);
                PdfPCell imageCellFASTagLog = new PdfPCell(jpgFASTagLog);
                imageCellFASTagLog.Border = 0;
                imageCellFASTagLog.HorizontalAlignment = Element.ALIGN_RIGHT;
                imageCellFASTagLog.VerticalAlignment = Element.ALIGN_CENTER;
                pdfPTableHeader.AddCell(imageCellFASTagLog);
                //add punchLine now
                PdfPCell punchLineCell = new PdfPCell(new Phrase("ExamOn - lets automate exam", new iTextSharp.text.Font(bfTimes, 15, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                punchLineCell.Border = 0;
                punchLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                punchLineCell.VerticalAlignment = Element.ALIGN_BASELINE;
                pdfPTableHeader.AddCell(punchLineCell);
                pdfPTableHeader.WidthPercentage = 100;
                document.Add(pdfPTableHeader);

                //add border
                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                document.Add(p);
               
                // Add content to the PDF
                if (pdfElement != null && pdfElement.Any())
                {
                    foreach (IElement element in pdfElement)
                    {                  
                        document.Add(element);
                    }
                }
                else
                {
                    document.Add(new Paragraph("No data found", times));
                }

                PdfPTable tbfooter = new PdfPTable(3);
                tbfooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                tbfooter.DefaultCell.Border = 0;
                tbfooter.AddCell(new Paragraph());
                tbfooter.AddCell(new Paragraph());
                var _cell2 = new PdfPCell(new Paragraph(new Chunk($"© 2020-{DateTime.Now.Year.ToString()} ExamOn, Inc. All rights reserved.", times)));
                _cell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                _cell2.Border = 0;
                tbfooter.AddCell(_cell2);
                tbfooter.AddCell(new Paragraph());
                tbfooter.AddCell(new Paragraph());
                var _celly = new PdfPCell(new Paragraph(writer.PageNumber.ToString(),times));//For page no.
                _celly.HorizontalAlignment = Element.ALIGN_RIGHT;
                _celly.Border = 0;
                tbfooter.AddCell(_celly);
                float[] widths1 = new float[] { 20f, 20f, 60f };
                tbfooter.SetWidths(widths1);
                tbfooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin), writer.DirectContent);
                document.Add(tbfooter);

                document.Close();
                writer.Close();
                pdfData = ms.ToArray();
            }

            return pdfData;
        }
    }
}