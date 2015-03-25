
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

            // D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SvgConverter\SvgConverter\drawing.dxf

            file = System.IO.Path.GetFullPath(file);


#if false


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
            //xmlns:cc="http://creativecommons.org/ns#"
            //xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
            //xmlns:svg="http://www.w3.org/2000/svg"

			foreach (netDxf.Entities.Line l in doc.Lines)
			{
				System.Console.WriteLine (l.StartPoint);
				System.Console.WriteLine (l.EndPoint);
			} // Next l

#endif

            // https://www.alt-soft.com/tutorial/svg_tutorial/file_struct.html
            // http://commons.wikimedia.org/wiki/SVG_examples

            // https://code.google.com/p/opus/source/browse/#svn%2Fbranches%2Fsvg%2FOpus.Core%2FSVG
            // https://code.google.com/p/opus/source/browse/branches/svg/Opus.Core/SVG/SVG.cs
            
            // http://kooboo.codeplex.com/


            // http://www.java2s.com/Tutorial/CSharp/0540__XML/GetNamespaceURIPrefixandLocalName.htm
            // Console.WriteLine(doc.DocumentElement.NamespaceURI);
            // Console.WriteLine(doc.DocumentElement.Prefix);
            // Console.WriteLine(doc.DocumentElement.LocalName);

            string SVGnamespace = "http://www.w3.org/2000/svg";
            string version = "1.1";

			System.Xml.XmlDocument svgDocument = new System.Xml.XmlDocument();
            svgDocument.XmlResolver = null;

            //System.Xml.XmlDocumentType doctype = svgDocument.CreateDocumentType("html", "-//W3C//DTD HTML 4.01//EN", "http://www.w3.org/TR/html4/strict.dtd", null);
            System.Xml.XmlDocumentType doctype = svgDocument.CreateDocumentType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            svgDocument.AppendChild(doctype);

            System.Xml.XmlElement svgElement = svgDocument.CreateElement("svg", SVGnamespace);
            
            svgElement.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            svgElement.SetAttribute("xmlns:cc", "http://creativecommons.org/ns#");
            svgElement.SetAttribute("xmlns:rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            svgElement.SetAttribute("xmlns:svg", SVGnamespace);

            // System.Xml.XmlAttribute attribute = svgDocument.CreateAttribute("version"); ;
            // attribute.Value = version;
            // svgElement.Attributes.Append(attribute);

            svgElement.SetAttribute("version", version);



            svgElement.SetAttribute("width", "500");
            svgElement.SetAttribute("height", "500");

            // http://tutorials.jenkov.com/svg/svg-viewport-view-box.html
            // svgElement.SetAttribute("viewBox", "0 0 500 500");
            svgElement.SetAttribute("viewBox", "0 0 " + svgElement.Attributes["width"].Value + " " + svgElement.Attributes["height"].Value);
            
            // preserveAspectRatio="none"







            System.Xml.XmlNode rootNode = svgDocument.AppendChild(svgElement);

            System.Xml.XmlElement textElement = svgDocument.CreateElement("text", SVGnamespace);
            textElement.InnerText = "Hello & <World>";
            textElement.SetAttribute("x", "123");
            textElement.SetAttribute("y", "456");

            rootNode.AppendChild(textElement);

            


            svgDocument.Save("foo.xml.svg");

			System.Console.WriteLine (" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Convert 


	} // End Class MainClass


} // End Namespace SvgConverter
