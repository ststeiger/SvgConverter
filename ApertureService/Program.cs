
using System.Windows.Forms;


namespace ApertureService
{


    static class Program
    {


        public static ApertureCrap.ApScriptingService GetApertureWebService()
        {
            ApertureCrap.ApScriptingService ass = new ApertureCrap.ApScriptingService();
            ass.Url = "https://www6.cor-asp.ch/ApWebServices/ApScriptingService.asmx";
            return ass;
        }


        public static ApertureBounds GetBounds(string d, string projname)
        {
            ApertureCrap.ApScriptingService ass = GetApertureWebService();

            string cmdname = "DrawingBounds";
            string parameters = "<DrawingName>" + d + "</DrawingName>";

            string strBounds = ass.ExecuteCommand(cmdname, projname, parameters);
            return new ApertureBounds(strBounds);
        }


        public static string ClickDrawing(string d, string projname)
        {

            ApertureBounds b = GetBounds(d, projname);
            System.Console.WriteLine(b);

            return ClickDrawing(d, projname, b);
        }


        public static double GetFitFactor(double svgWidth, double svgHeight, double boxWidth, double boxHeight)
        {
            // TODO: Check for arguments (for null and <=0)
            var widthScale = boxWidth / (double)svgWidth;
            var heightScale = boxHeight / (double)svgHeight;
            double scale = (double) System.Math.Min(widthScale, heightScale);
            return scale;
        }



        // Nur mit IE
        // https://www6.cor-asp.ch/FM_COR_Demo/test/DWG.html
        public static string ClickDrawing(string d, string projname, ApertureBounds b)
        {
            string l = "RaumNutzung"; // Layerset

            double w = 800; // Bildgrösse 
            double h = 600; // Bildgrösse 

            double x = 336; // Click-Position
            double y = 292; // Click-Position

            x = 625;
            y = 182;


            

            // SVG
            w = 2098; // Bildgrösse 
            h = 2969; // Bildgrösse 

            w = 790;
            h = 1121;

            x = 190;
            y = 490;

            //double mx = w / 2.0;
            //double my = h / 2.0;

            //x = mx - x;
            //y = my - y;
            

            //double s = GetFitFactor(2098, 2969, 800, 600);
            //double sx = 800.0 / w;
            //double sy = 600.0 / h;

            //x = x * sx;
            //y = y * sy;

            //w = 800;
            //h = 600;



            ApertureCrap.ApScriptingService ass = GetApertureWebService();


            // string mImageLinkTemplate = "{@domain}/ApWebServices/ApDrawingImages.aspx?p={@project}&d={@dwg}&xlr={@r}&ylr={@b}&yul={@t}&xul={@l}&dx={@width}&dy={@height}&L={@layerset}&S={@stylizer}&SEL={@obj}&F=PNG&uu={@cache}";
            string mParamClickTemplate = "<InParams><Drawing>{@dwg}</Drawing><l>{@l}</l><r>{@r}</r><t>{@t}</t><b>{@b}</b><Width>{@width}</Width><Height>{@height}</Height><ClickX>{@x}</ClickX><ClickY>{@y}</ClickY><LayerSet>{@layerset}</LayerSet><RecTypes>0</RecTypes></InParams>";


            string parameters = mParamClickTemplate;
            parameters = parameters
                .Replace("{@dwg}", d)
                .Replace("{@r}", b.sR.Replace(",", "."))

#if true // interchange b & t because Aperture is buggy...
                .Replace("{@b}", b.sT.Replace(",", "."))
                .Replace("{@t}", b.sB.Replace(",", "."))
#else
                .Replace("{@b}", b.sB.Replace(",", "."))
                .Replace("{@t}", b.sT.Replace(",", "."))
#endif

.Replace("{@l}", b.sL.Replace(",", "."))
                .Replace("{@width}", ToAwsString(w))
                .Replace("{@height}", ToAwsString(h))
                .Replace("{@layerset}", l)
                .Replace("{@y}", ToAwsString(y))
                .Replace("{@x}", ToAwsString(x))
                // .Replace("<", "&lt;")
                // .Replace(">", "&gt;")
                ;


            string cmdname = "_DrawingClick";
            string strRes = ass.ExecuteCommand(cmdname, projname, parameters);
            System.Console.WriteLine(strRes);


			System.Xml.XmlDocument doc = new System.Xml.XmlDocument ();
			doc.LoadXml (strRes);

			System.Xml.XmlNode nd = doc.SelectSingleNode ("//ObjectID");
			System.Console.WriteLine (nd);

			string strApertureObjId = null;
			if(nd != null)
				strApertureObjId = nd.InnerText;

			return strApertureObjId;
        }


        // To ApertureWebServiceString
        public static string ToAwsString(double d)
        {
            return d.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
#if false
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif

            string d = "0001_GB01_OG14_0000";
            string projname = "COR_Demo_Portal";
            // ClickDrawing(d, projname);
            TransformPath ();
        }


		public static void TransformPath()
		{
			string str = System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location);
			str = System.IO.Path.Combine (str, "..");
			str = System.IO.Path.Combine (str, "..");
			str = System.IO.Path.Combine (str, "0001_GB01_OG14_0000_Aperture.svg");
			str = System.IO.Path.GetFullPath (str);

			TransformPath (str);
		}

		public static void TransformPath(string file)
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument ();
			doc.Load (file);

			System.Xml.XmlNamespaceManager nspmgr = new System.Xml.XmlNamespaceManager (doc.NameTable);
            nspmgr.AddNamespace(doc.DocumentElement.Name, doc.DocumentElement.NamespaceURI);

            // System.Xml.XmlNode nd = doc.SelectSingleNode("//svg:g[@id='FM_OBJEKT_RAUM']", nspmgr);
            // System.Xml.XmlNodeList paths = nd.SelectNodes ("./svg:path", nspmgr);
            //System.Xml.XmlNodeList paths = doc.SelectNodes("//svg:g[@id='FM_OBJEKT_RAUM']/svg:path", nspmgr);
            System.Xml.XmlNodeList paths = doc.SelectNodes("//svg:g[@id='FM_OBJEKT_RAUM']/svg:path[@data-handle='4']", nspmgr);

            System.Console.WriteLine(paths);


            foreach (System.Xml.XmlNode path in paths)
            {
                System.Xml.XmlAttribute da = path.Attributes["d"];
                if (da == null)
                    continue;


                string d = da.Value.Trim();
                if(d.StartsWith("M"))
                    d = d.Substring(1);
                if (d.EndsWith("z"))
                    d = d.Substring(0, d.Length - 1);

                string[] coords = d.Split('L');


                System.Collections.Generic.List<System.Drawing.Point> points = 
                    new System.Collections.Generic.List<System.Drawing.Point>();

            
                foreach (string coord in coords)
                {
                    string[] xyc = coord.Split(' ');

                    float fX = 0;
                    float fY = 0;
                    float.TryParse(xyc[0], out fX);
                    float.TryParse(xyc[1], out fY);

                    // points.Add(new double[] { dblX, dblY });
                    points.Add(  new System.Drawing.Point((int)fX, (int)fY)  );
                    System.Console.WriteLine("Pxy: [{0},{1}]", fX, fY);
                } // Next coord


                int j = points.Count - 1;
                for (int i = 0; i < points.Count; ++i)
                {
                    if (j <= i)
                        break;

                    if (points[i] == points[j])
                        points.RemoveAt(j);
                    else
                    {
                        System.Console.WriteLine(points[i] == points[j]);
                        break;
                    }
                       

                    j--;
                } // Next i

                string str = "";
                for (int i = 0; i < points.Count; ++i)
                {
                    if (i == 0 && points.Count == 1)
                        str += string.Format("M{0} {1} z", points[i].X, points[i].Y);
                    else if (i == 0)
                        str += string.Format("M{0} {1}", points[i].X, points[i].Y);
                    else if (i == points.Count - 1)
                        str += string.Format("L{0} {1} z", points[i].X, points[i].Y);
                    else
                        str += string.Format("L{0} {1}", points[i].X, points[i].Y);
                }

                // System.Console.WriteLine(str);
                path.Attributes["d"].Value = str;
            } // Next path

            doc.Save(@"D:\lolipop.svg");

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
		} // End Sub TransformPath


    } // End Class 


} // End Namespace 
