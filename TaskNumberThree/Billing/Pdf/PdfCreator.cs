using System;
using System.Collections.Generic;
using System.Globalization;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TaskNumberThree.Interfaces;
using TaskNumberThree.VirtualModel.Auxiliary;

namespace TaskNumberThree.Billing.Pdf
{
    public class PdfCreator : IPdfCreator
    {
        public void PdfCreate(ICollection<CallString> callStrings, string fileName)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Helvetica", 12, XFontStyle.Regular);

            gfx.DrawString(fileName, new XFont("Helvetica", 20, XFontStyle.Regular), XBrushes.Black, new XRect(0, 50, page.Width, page.Height), XStringFormats.TopCenter);

            int i = 100;
            foreach (var item in callStrings)
            {
                string inLine;
                if (item.CallInOut == CallInOut.Incoming)
                {
                    inLine = String.Format("{0}       <---{1}     " + (item.Duration / 60) + " min. " + (item.Duration % 60) + " s. " + "     {2} BYN", item.DateTime.ToString(CultureInfo.InvariantCulture), item.AnyMobileNumber, item.ToWriteOff);
                }
                else
                {
                    inLine = String.Format("{0}            {1}     " + (item.Duration / 60) + " min. " + (item.Duration % 60) + " s. " + "     {2} BYN", item.DateTime.ToString(CultureInfo.InvariantCulture), item.AnyMobileNumber, item.ToWriteOff);
                }
                gfx.DrawString(inLine, font, XBrushes.Black, new XRect(50, i, page.Width, page.Height), XStringFormats.TopLeft);
                i += 50;
            }

            XFont font2 = new XFont("Times New Roman", 70, XFontStyle.Bold);
            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);
            XBrush brush = new XSolidBrush(XColor.FromArgb(70, 255, 0, 0));
            gfx.DrawString("EPAMTaskNumberThree", font2, brush, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            document.Save(fileName + ".pdf");
        }
    }
}
