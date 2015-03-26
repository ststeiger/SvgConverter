
namespace SvgConverter
{


	class MainClass
	{


		public static string ToSvgFile(string file)
		{
			file = System.IO.Path.GetFileNameWithoutExtension(file);
			file += ".svg";
			return file;
		}



		public static void Convert()
		{
			string file = System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location);
			file = System.IO.Path.Combine (file, "..");
			file = System.IO.Path.Combine (file, "..");

			// file = System.IO.Path.Combine (file, "Zimmertyp_1_.dxf");
			file = System.IO.Path.Combine(file, "drawing.dxf");

			// D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SvgConverter\SvgConverter\drawing.dxf

			file = System.IO.Path.GetFullPath (file);

#if true 


			// InfoDump.DumpFile (file);

			// DxfDocument doc = Test(file, "output.log");
			// DxfDocument doc = Test(file);
			netDxf.DxfDocument doc = netDxf.DxfDocument.Load(file);


			//  foreach (string ln in doc.Layers.Names) System.Console.WriteLine (ln);


			System.Console.WriteLine("\t{0}; count: {1}", netDxf.Entities.EntityType.Line, doc.Lines.Count);

            // https://www.alt-soft.com/tutorial/svg_tutorial/file_struct.html

            // <?xml version="1.0" standalone="no"?>
            // <!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">
            // <!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.0//EN" "http://www.w3.org/TR/2001/PR-SVG-20010719/DTD/svg10.dtd">


            // https://books.google.de/books?id=YemMcMyMIgEC&pg=PA115&lpg=PA115&source=bl&ots=-Uryzwrz1F&sig=vGiu9s87qjMXai0mV0TcVNiXmE4&hl=en&sa=X&ei=kqISVffTK8vtUomlhJAF&redir_esc=y#v=onepage&q&f=false
            

            // xmlns:dc="http://purl.org/dc/elements/1.1/"
            // xmlns:cc="http://creativecommons.org/ns#"
            // xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
            // xmlns:svg="http://www.w3.org/2000/svg"

			cSVG SVG = new cSVG ();


			foreach (netDxf.Entities.Line l in doc.Lines)
			{
				System.Console.WriteLine( l.Handle );
				System.Console.WriteLine (l.StartPoint);
				System.Console.WriteLine (l.EndPoint);

				SVG.AddLine(l.StartPoint, l.EndPoint);
			} // Next l


			SVG.Save(ToSvgFile(file));

#endif

			// CreateSampleDocument();


			// https://www.alt-soft.com/tutorial/svg_tutorial/file_struct.html
			// http://commons.wikimedia.org/wiki/SVG_examples

			// https://code.google.com/p/opus/source/browse/#svn%2Fbranches%2Fsvg%2FOpus.Core%2FSVG
			// https://code.google.com/p/opus/source/browse/branches/svg/Opus.Core/SVG/SVG.cs
            
			// http://kooboo.codeplex.com/


			// http://www.java2s.com/Tutorial/CSharp/0540__XML/GetNamespaceURIPrefixandLocalName.htm
			// Console.WriteLine(doc.DocumentElement.NamespaceURI);
			// Console.WriteLine(doc.DocumentElement.Prefix);
			// Console.WriteLine(doc.DocumentElement.LocalName);
		}


		public static void CreateSampleDocument()
		{
			cSVG SVG = new cSVG ();


			System.Xml.XmlElement textElement  = SVG.CreateText ("Hello & <World>", 123, 456);
			SVG.RootNode.AppendChild(textElement);


			// http://www.w3schools.com/svg/svg_ellipse.asp

			System.Xml.XmlElement line = SVG.CreateLine (0, 0, 123, 456);
			SVG.RootNode.AppendChild(line);



			System.Xml.XmlElement rect = SVG.CreateRectangle (100, 100, 20, 20);
			// rect.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");
			SVG.RootNode.AppendChild(rect);



			System.Xml.XmlElement circle = SVG.CreateCircle (50, 50, 50);
			// circle.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");
			// circle.SetAttribute ("stroke", "black");
			// circle.SetAttribute ("stroke-width", "3");
			// circle.SetAttribute ("fill", "red");
			SVG.RootNode.AppendChild(circle);



			System.Xml.XmlElement ellipse = SVG.CreateEllipse (200, 200, 20, 50);
			// ellipse.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");
			// ellipse.SetAttribute ("stroke", "black");
			// ellipse.SetAttribute ("stroke-width", "3");
			// ellipse.SetAttribute ("fill", "red");
			SVG.RootNode.AppendChild(ellipse);



			System.Xml.XmlElement polygon = SVG.CreatePolygon ("200,10 250,190 160,210");
			// polygon.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");
			// polygon.SetAttribute ("stroke", "black");
			// polygon.SetAttribute ("stroke-width", "3");
			// polygon.SetAttribute ("fill", "red");
			SVG.RootNode.AppendChild(polygon);



			System.Xml.XmlElement polyline = SVG.CreatePolyline ("300,300 320,320 340,300 360,350 380,300 400,400");
			// polyline.SetAttribute("style", "fill:none;stroke-width:3;stroke:rgb(0,0,0)");
			// polyline.SetAttribute ("stroke", "black");
			// polyline.SetAttribute ("stroke-width", "3");
			// polyline.SetAttribute ("fill", "red");
			SVG.RootNode.AppendChild(polyline);



			System.Xml.XmlElement path = SVG.CreatePath ("M150 0 L75 200 L225 200 Z");
			path.SetAttribute("style", "fill:rgb(255,0,0);stroke-width:3;stroke:rgb(0,0,0)");
			// path.SetAttribute ("stroke", "black");
			// path.SetAttribute ("stroke-width", "3");
			// path.SetAttribute ("fill", "red");
			SVG.RootNode.AppendChild(path);


			SVG.Save ("foo.xml.svg");

			System.Console.WriteLine (" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Convert 


	} // End Class MainClass


} // End Namespace SvgConverter
