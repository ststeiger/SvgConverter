
namespace SvgConverter
{


    public class SvgToPdf
    {

        public System.Xml.XmlDocument doc;
		public PdfSharp.Pdf.PdfDocument Pdf;
		public PdfSharp.Drawing.XGraphics gfx;


		public static string GetTestFileName()
		{
			string fn = System.Reflection.Assembly.GetExecutingAssembly().Location;
			fn = System.IO.Path.GetDirectoryName (fn);
			fn = System.IO.Path.Combine (fn, "..");
			fn = System.IO.Path.Combine (fn, "..");
			fn = System.IO.Path.Combine (fn, "0001_GB01_OG14_0000_Aperture.svg");

			fn = System.IO.Path.GetFullPath (fn);

			return fn;
		}


		public void disp()
		{
			this.gfx.Dispose ();
			this.Pdf.Dispose();
		}


        public SvgToPdf()
			: this(GetTestFileName())
        {
            // doc = new System.Xml.XmlDocument();
            // doc.XmlResolver = null;
        }


        public SvgToPdf(string fileName)
        {
            doc = new System.Xml.XmlDocument();
            doc.XmlResolver = null;


			string str = System.IO.File.ReadAllText (fileName, System.Text.Encoding.ASCII);
			System.Console.WriteLine (str);
            // doc.Load(fileName);
			doc.LoadXml (str);

            System.Console.WriteLine("test");
        }

		public void InitPdf()
		{
			// Create new PDF document
			this.Pdf = new PdfSharp.Pdf.PdfDocument();
			this.Pdf.Info.CreationDate = System.DateTime.Now;
			this.Pdf.Info.Title = "PDFsharp SVG";
			this.Pdf.Info.Author = "Stefan Steiger";
			this.Pdf.Info.Subject = "SVG";

			// Create new page
			PdfSharp.Pdf.PdfPage page = this.Pdf.AddPage();

			page.Width = PdfSharp.Drawing.XUnit.FromMillimeter(200);
			page.Height = PdfSharp.Drawing.XUnit.FromMillimeter(200);

			this.gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
		}


        public void CreatePdf()
        {
			InitPdf();

			// Pen: Line
            PdfSharp.Drawing.XPen pen = PdfSharp.Drawing.XPens.Black;
            
			// Brush: Fill
			PdfSharp.Drawing.XBrush brush = PdfSharp.Drawing.XBrushes.Red;

            double x = 100;
            double y = 100;
            double width = 20;
            double height = 20;

            this.gfx.DrawEllipse(pen, brush, x, y, width, height);
			// this.gfx.DrawArc

			/*
			this.gfx.DrawPolygon (pen, new System.Drawing.Point[]{ 
				new System.Drawing.Point(150,150)
				,new System.Drawing.Point(170,170)
				,new System.Drawing.Point(190,150)

			});
			*/

			this.gfx.DrawPolygon (pen, brush
				,new System.Drawing.Point[]
			     { 
				     new System.Drawing.Point(150,150)
				    ,new System.Drawing.Point(170,170)
				    ,new System.Drawing.Point(190,150)

			     }
				,PdfSharp.Drawing.XFillMode.Winding
			);


			// this.gfx.DrawLine
			// this.gfx.DrawRectangle

            // Send PDF to browser
            // System.IO.MemoryStream stream = new System.IO.MemoryStream();
			// this.Pdf.Save(stream, false);
			this.Pdf.Save(@"/root/Desktop/foosvgbar.pdf");


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


}
