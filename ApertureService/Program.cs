
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


        // Nur mit IE
        // https://www6.cor-asp.ch/FM_COR_Demo/test/DWG.html
        public static string ClickDrawing(string d, string projname)
        {
            string l = "RaumNutzung"; // Layerset

            string w = "800"; // Bildgrösse 
            string h = "600"; // Bildgrösse 

            string x = "336"; // Click-Position
            string y = "292"; // Click-Position

            x = "625";
            y = "182";


            ApertureBounds b = GetBounds(d, projname);
            System.Console.WriteLine(b);

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
                .Replace("{@width}", w)
                .Replace("{@height}", h)
                .Replace("{@layerset}", l)
                .Replace("{@y}", y)
                .Replace("{@x}", x)
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
			nspmgr.AddNamespace ("svg", doc.DocumentElement.NamespaceURI);

            // System.Xml.XmlNode nd = doc.SelectSingleNode("//svg:g[@id='FM_OBJEKT_RAUM']", nspmgr);
            // System.Xml.XmlNodeList paths = nd.SelectNodes ("./svg:path", nspmgr);
            System.Xml.XmlNodeList paths = doc.SelectNodes("//svg:g[@id='FM_OBJEKT_RAUM']/svg:path", nspmgr);

            System.Console.WriteLine(paths);


            System.Collections.Generic.List<System.Collections.Generic.List<double[]>> RoomList = new System.Collections.Generic.List<System.Collections.Generic.List<double[]>>();


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

                System.Collections.Generic.List<double[]> points = new System.Collections.Generic.List<double[]>();

                foreach (string coord in coords)
                {
                    string[] xyc = coord.Split(' ');

                    double dblX = 0;
                    double dblY = 0;
                    double.TryParse(xyc[0], out dblX);
                    double.TryParse(xyc[1], out dblY);

                    points.Add(new double[] { dblX, dblY });


                    System.Console.WriteLine("Pxy: [{0},{1}]", dblX, dblY);
                } // Next coord

                RoomList.Add(points);
            } // Next path

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
		} // End Sub TransformPath


    } // End Class 


} // End Namespace 
