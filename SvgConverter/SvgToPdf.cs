
namespace SvgConverter
{


    public class SvgToPdf
    {
        public string FileName;
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

            this.FileName = System.IO.Path.GetFileNameWithoutExtension(fileName);

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
            m_TopLeft = new System.Drawing.Point(0, 0);
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

            // Dimension
			page.Width = PdfSharp.Drawing.XUnit.FromMillimeter(1000);
			page.Height = PdfSharp.Drawing.XUnit.FromMillimeter(1000);

            this.gfxPdf = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
		}


        public void CreatePdf()
        {
			InitPdf();
            TraverseNodes();

            //// Pen: Line
            //PdfSharp.Drawing.XPen pen = PdfSharp.Drawing.XPens.Black;
            
            //// Brush: Fill
            //PdfSharp.Drawing.XBrush brush = PdfSharp.Drawing.XBrushes.Red;

            //double x = 100;
            //double y = 100;
            //double width = 20;
            //double height = 20;

            //this.gfxPdf.DrawEllipse(pen, brush, x, y, width, height);
            // this.gfxPdf.DrawArc

            /*
            this.gfxPdf.DrawPolygon (pen, new System.Drawing.Point[]{ 
                new System.Drawing.Point(150,150)
                ,new System.Drawing.Point(170,170)
                ,new System.Drawing.Point(190,150)

            });
            */


            // this.gfxPdf.DrawLine
            // this.gfxPdf.DrawRectangle

            // Send PDF to browser
            // System.IO.MemoryStream stream = new System.IO.MemoryStream();
			// this.Pdf.Save(stream, false);


            string fn = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            fn = System.IO.Path.Combine(fn, FileName + ".pdf");

            this.Pdf.Save(fn);

            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-length", stream.Length.ToString());
            //Response.BinaryWrite(stream.ToArray());
            //Response.Flush();
            //stream.Close();
            //Response.End();
        } // End Sub CreatePdf


        public void DrawPath(System.Xml.XmlNode xn)
        {
            string d = xn.Attributes["d"].Value;
            string f = xn.Attributes["fill"].Value;
            string s = xn.Attributes["stroke"].Value;


            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(f);


            System.Collections.Generic.List<System.Drawing.Point> pathPoints = GetPathPointsPoints(d);

            if (pathPoints == null)
                return;


            
            double width = 1.0;

            PdfSharp.Drawing.XPen pen = PdfSharp.Drawing.XPens.Black;// Pen: Line
            //PdfSharp.Drawing.XPen pen2 = new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColor.FromArgb(255, System.Drawing.Color.Black), width);
            PdfSharp.Drawing.XPen pen2 = new PdfSharp.Drawing.XPen(PdfSharp.Drawing.XColor.FromArgb(System.Drawing.Color.Black), width);


            
            

            // Brush: Fill
            // PdfSharp.Drawing.XBrush brush = PdfSharp.Drawing.XBrushes.Red;
            PdfSharp.Drawing.XBrush brush = new PdfSharp.Drawing.XSolidBrush(PdfSharp.Drawing.XColor.FromArgb(col));
            // System.Console.WriteLine(pen);
            // System.Console.WriteLine(pen2);

            this.gfxPdf.DrawPolygon(pen2, brush
                , pathPoints.ToArray()
                , PdfSharp.Drawing.XFillMode.Winding
            );

            System.Console.WriteLine(d);
            // http://www.pdfsharp.net/wiki/Clock-sample.ashx
        }


        public System.Collections.Generic.List<System.Drawing.Point> GetPathPointsPoints(string d)
        {
            System.Collections.Generic.List<System.Drawing.Point> points =
                new System.Collections.Generic.List<System.Drawing.Point>();

            if (string.IsNullOrEmpty(d))
                return null;

            d = d.Trim();

            if (d.StartsWith("M"))
                d = d.Substring(1);
            if (d.EndsWith("z"))
                d = d.Substring(0, d.Length - 1);

            string[] coords = d.Split('L');


            foreach (string coord in coords)
            {
                string[] xyc = coord.Split(' ');

                float fX = 0;
                float fY = 0;
                float.TryParse(xyc[0], out fX);
                float.TryParse(xyc[1], out fY);

                // points.Add(new double[] { dblX, dblY });
                points.Add(new System.Drawing.Point((int)fX, (int)fY));
                // System.Console.WriteLine("Pxy: [{0},{1}]", fX, fY);
            } // Next coord

            return points;
        }


        public void TraverseNodes()
        {
            TraverseNodes(Xml.DocumentElement);
        }


        public void TraverseNodes(System.Xml.XmlNode xn)
        {
            System.Console.WriteLine(xn.Name);

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(xn.Name, "path"))
            {
                DrawPath(xn);
            } // End if (System.StringComparer.InvariantCultureIgnoreCase.Equals(xn.Name, "path"))


            if (xn.HasChildNodes)
            {
                foreach (System.Xml.XmlNode childNode in xn.ChildNodes)
                {
                    if (childNode.NodeType != System.Xml.XmlNodeType.Text)
                        TraverseNodes(childNode);
                } // Next childNode 
            } // End if (xn.HasChildNodes)

        } // End Sub TraverseNodes


    }


}
