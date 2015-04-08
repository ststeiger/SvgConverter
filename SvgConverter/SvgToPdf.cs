
namespace SvgConverter
{


    public class SvgToPdf
    {

        public System.Xml.XmlDocument Xml;
		public PdfSharp.Pdf.PdfDocument Pdf;
		public PdfSharp.Drawing.XGraphics gfxPdf;


		public static string GetTestFileName()
		{
			string fn = System.Reflection.Assembly.GetExecutingAssembly().Location;
			fn = System.IO.Path.GetDirectoryName (fn);
			fn = System.IO.Path.Combine (fn, "..");
			fn = System.IO.Path.Combine (fn, "..");
			fn = System.IO.Path.Combine (fn, "0001_GB01_OG14_0000_Aperture.svg");

			fn = System.IO.Path.GetFullPath (fn);

			return fn;
		} // End Function GetTestFileName 


		public void disp()
		{
            this.gfxPdf.Dispose();
			this.Pdf.Dispose();
		}


        public SvgToPdf()
			: this(GetTestFileName())
        {
            // Xml = new System.Xml.XmlDocument();
            // Xml.XmlResolver = null;
        } // End Constructor SvgToPdf 


        public SvgToPdf(string fileName)
        {
            Xml = new System.Xml.XmlDocument();
            if(System.Environment.OSVersion.Platform != System.PlatformID.Unix)
                Xml.XmlResolver = null; // .NET Framework


			// string str = System.IO.File.ReadAllText (fileName, System.Text.Encoding.ASCII);
            // System.Console.WriteLine (str);
            // Xml.LoadXml(str);

            Xml.Load(fileName);
			
            System.Console.WriteLine("test");
        } // End Constructor SvgToPdf 


        public System.Drawing.Size Dimension
        {
            get
            {
                int deltaX = System.Math.Abs( BottomRight.X - TopLeft.X);
                int deltaY = System.Math.Abs(BottomRight.Y - TopLeft.Y);

                return new System.Drawing.Size(deltaX, deltaY);
            }
        }


        private System.Drawing.Point m_TopLeft;
        private System.Drawing.Point m_BottomRight;


        private void GetDimensions()
        {
            m_TopLeft = new System.Drawing.Point(-500, -500);
            m_BottomRight = new System.Drawing.Point(500, 500);
        }


        public System.Drawing.Point TopLeft
        {
            get
            {
                if (m_TopLeft != null)
                    return m_TopLeft;

                GetDimensions();

                return m_TopLeft;
            }
        }


        public System.Drawing.Point BottomRight
        {
            get
            {
                if (m_BottomRight != null)
                    return m_BottomRight;

                GetDimensions();

                return m_BottomRight;
            }
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

            this.gfxPdf = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
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

            this.gfxPdf.DrawEllipse(pen, brush, x, y, width, height);
			// this.gfx.DrawArc

			/*
			this.gfx.DrawPolygon (pen, new System.Drawing.Point[]{ 
				new System.Drawing.Point(150,150)
				,new System.Drawing.Point(170,170)
				,new System.Drawing.Point(190,150)

			});
			*/

            this.gfxPdf.DrawPolygon(pen, brush
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


            string fn = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            fn = System.IO.Path.Combine(fn, "foosvgbar.pdf");

            this.Pdf.Save(fn);

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
            TraverseNodes(Xml.DocumentElement);
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
