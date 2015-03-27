
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
            ClickDrawing(d, projname);
        }


    }


}
