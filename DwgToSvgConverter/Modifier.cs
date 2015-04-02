
namespace DwgToSvgConverter
{


	public class Modifier
	{


		public static string GetInlineStyle()
		{
			string style = @"
path:hover {
    fill: orange;
    stroke-width: 10;
    stroke: gold;
    transition: all .2s ease-in-out; 
    transform: scale(1.0);
}
    
path
{
    transform: scale(1.0);
}
";
			return style;
		} // End Function GetInlineStyle


        public static void ModifySVG(string file)
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument ();
			doc.XmlResolver = null;
			doc.Load (file);

			System.Xml.XmlNamespaceManager nspmgr = new System.Xml.XmlNamespaceManager (doc.NameTable);
			nspmgr.AddNamespace(doc.DocumentElement.Name, doc.DocumentElement.NamespaceURI);

			bool bOnlyRoomLayer = true;

			if (bOnlyRoomLayer)
			{
				System.Xml.XmlNodeList pathLayers = doc.SelectNodes("//svg:g[@id!='FM_OBJEKT_RAUM']", nspmgr);
				foreach (System.Xml.XmlNode layer in pathLayers)
				{
					layer.ParentNode.RemoveChild(layer);
				} // Next layer
			} // End if (bOnlyRoomLayer)



			System.Xml.XmlElement xe = doc.CreateElement("style", doc.DocumentElement.NamespaceURI);
			xe.SetAttribute("type", "text/css");

			xe.InnerText = GetInlineStyle ();

			if (doc.DocumentElement.FirstChild == null)
				doc.DocumentElement.AppendChild(xe);
			else
				doc.DocumentElement.InsertBefore(xe, doc.DocumentElement.FirstChild);

			// System.Xml.XmlNode nd = doc.SelectSingleNode("//svg:g[@id='FM_OBJEKT_RAUM']", nspmgr);
			// System.Xml.XmlNodeList paths = nd.SelectNodes ("./svg:path", nspmgr);
			System.Xml.XmlNodeList paths = doc.SelectNodes("//svg:g[@id='FM_OBJEKT_RAUM']/svg:path", nspmgr);
			// System.Xml.XmlNodeList paths = doc.SelectNodes("//svg:g[@id='FM_OBJEKT_RAUM']/svg:path[@data-handle='4']", nspmgr);

			// System.Console.WriteLine(paths);

			int r = 20;

			foreach (System.Xml.XmlNode path in paths)
			{

				// string col = string.Format("#0000{0:X2}", r);
				//path.Attributes["fill"].Value = "#FF0000";
				path.Attributes ["fill"].Value = string.Format ("#00{0:X2}00", r);
				r += 37;
				r = r % 255;
            } // Next path


			string newFileName = null;
			if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
				newFileName = @"/root/" + "Rooms_" + System.IO.Path.GetFileNameWithoutExtension(file) + @".svg";
			else if(StorageHelper.DriveExists(@"D:\"))
				newFileName = @"D:\" + "Rooms_" + System.IO.Path.GetFileNameWithoutExtension(file) + @".svg";
            else newFileName = StorageHelper.GetDesktopPath("Rooms_" + System.IO.Path.GetFileNameWithoutExtension(file) + @".svg");
			
            doc.Save(newFileName);
        } // End Sub ModifySVG


    } // End Class Modifier


} // End Namespace DwgToSvgConverter 
