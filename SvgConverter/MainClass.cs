
using netDxf;


namespace SvgConverter
{


	class MainClass
	{


		public static void Convert()
		{
            string file = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            file = System.IO.Path.Combine(file, "..");
            file = System.IO.Path.Combine(file, "..");

            file = System.IO.Path.Combine(file, "Zimmertyp_1_.dxf");
            // file = System.IO.Path.Combine(file, "drawing.dxf");

            file = System.IO.Path.GetFullPath(file);


			InfoDump.DumpFile (file);

			// DxfDocument doc = Test(file, "output.log");
			// DxfDocument doc = Test(file);
			netDxf.DxfDocument doc = netDxf.DxfDocument.Load(file);


			//  foreach (string ln in doc.Layers.Names) System.Console.WriteLine (ln);


			System.Console.WriteLine("\t{0}; count: {1}", netDxf.Entities.EntityType.Line, doc.Lines.Count);


			foreach (netDxf.Entities.Line l in doc.Lines)
			{
				System.Console.WriteLine (l.StartPoint);
				System.Console.WriteLine (l.EndPoint);
			} // Next l

			System.Xml.XmlDocument doc2 = new System.Xml.XmlDocument();

			System.Console.WriteLine (" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Convert 


	} // End Class MainClass


} // End Namespace SvgConverter
