
namespace SvgToPdf
{

    public class SvgToPdf
    {

        public System.Xml.XmlDocument doc;

        public SvgToPdf()
            : this(@"C:\Users\Administrator\Documents\Visual Studio 2013\Projects\SvgConverter\SvgEdit\0001_GB01_OG14_0000_Aperture.svg")
        {
            // doc = new System.Xml.XmlDocument();
            // doc.XmlResolver = null;
        }

        public SvgToPdf(string fileName)
        {
            doc = new System.Xml.XmlDocument();
            doc.XmlResolver = null;
            doc.Load(fileName);


            System.Console.WriteLine("test");
        }



        public static void xxx()
        {
            // Create new PDF document
            PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
            document.Info.CreationDate = System.DateTime.Now;
            document.Info.Title = "PDFsharp SVG";
            document.Info.Author = "Stefan Steiger";
            document.Info.Subject = "SVG";

            // Create new page
            PdfSharp.Pdf.PdfPage page = document.AddPage();

            page.Width = PdfSharp.Drawing.XUnit.FromMillimeter(200);
            page.Height = PdfSharp.Drawing.XUnit.FromMillimeter(200);

            PdfSharp.Drawing.XPen pen = PdfSharp.Drawing.XPens.Black;
            PdfSharp.Drawing.XBrush brush = PdfSharp.Drawing.XBrushes.Red;

            PdfSharp.Drawing.XGraphics gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);

            double x = 100;
            double y = 100;
            double width = 20;
            double height = 20;


            gfx.DrawEllipse(pen, brush, x, y, width, height);




            // Send PDF to browser
            // System.IO.MemoryStream stream = new System.IO.MemoryStream();
            // document.Save(stream, false);
            document.Save(@"");


            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-length", stream.Length.ToString());
            //Response.BinaryWrite(stream.ToArray());
            //Response.Flush();
            //stream.Close();
            //Response.End();
        }



        public static void DrawPath(System.Xml.XmlNode xn)
        {
            string d = xn.Attributes["d"].Value;
            string f = xn.Attributes["fill"].Value;
            string s = xn.Attributes["stroke"].Value;

            System.Console.WriteLine(d);
            // http://www.pdfsharp.net/wiki/Clock-sample.ashx
        }



        public void TraverseNodes()
        {
            TraverseNodes(doc.DocumentElement);
        }


        public static void TraverseNodes(System.Xml.XmlNode xn)
        {
            System.Console.WriteLine(xn.Name);

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(xn.Name, "path"))
            {
                DrawPath(xn);
            }


            if (xn.HasChildNodes)
            {
                foreach (System.Xml.XmlNode cn in xn.ChildNodes)
                {
                    if (cn.NodeType != System.Xml.XmlNodeType.Text)
                        TraverseNodes(cn);
                }
            }

        }


    }



    static class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
#if true

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Form1());
#endif


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(@" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }


    }


}
