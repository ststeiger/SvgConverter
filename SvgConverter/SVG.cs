
namespace SvgConverter
{


	public class cSVG
	{
		private string SVGnamespace = "http://www.w3.org/2000/svg";


		public System.Xml.XmlDocument SvgDocument;
		public System.Xml.XmlElement SvgElement;
		public System.Xml.XmlNode RootNode;


		public cSVG()
		{
			CreateSvg();
		}


		public void CreateSvg()
		{
			CreateSvg("1.1");
		}


		public void CreateSvg(string version)
		{
			this.SvgDocument = new System.Xml.XmlDocument();
			this.SvgDocument.XmlResolver = null;

			//System.Xml.XmlDocumentType doctype = SvgDocument.CreateDocumentType("html", "-//W3C//DTD HTML 4.01//EN", "http://www.w3.org/TR/html4/strict.dtd", null);
			System.Xml.XmlDocumentType doctype = SvgDocument.CreateDocumentType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
			SvgDocument.AppendChild(doctype);

			this.SvgElement = SvgDocument.CreateElement("svg", SVGnamespace);

			this.SvgElement.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
			this.SvgElement.SetAttribute("xmlns:cc", "http://creativecommons.org/ns#");
			this.SvgElement.SetAttribute("xmlns:rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
			this.SvgElement.SetAttribute("xmlns:svg", SVGnamespace);

			// System.Xml.XmlAttribute attribute = svgDocument.CreateAttribute("version"); ;
			// attribute.Value = version;
			// this.SvgElement.Attributes.Append(attribute);

			this.SvgElement.SetAttribute("version", version);
			this.SvgElement.SetAttribute("width", "500");
			this.SvgElement.SetAttribute("height", "500");

			// http://tutorials.jenkov.com/svg/svg-viewport-view-box.html
			// svgElement.SetAttribute("viewBox", "0 0 500 500");
			this.SvgElement.SetAttribute("viewBox", "0 0 " 
				+ this.SvgElement.Attributes["width"].Value 
				+ " " 
				+ this.SvgElement.Attributes["height"].Value);

			// preserveAspectRatio="none"

			this.RootNode = SvgDocument.AppendChild(this.SvgElement);
		}


		public System.Xml.XmlElement CreateText(string text, int x, int y)
		{
			System.Xml.XmlElement textElement = this.SvgDocument.CreateElement("text", SVGnamespace);
			textElement.InnerText = text;
			textElement.SetAttribute("x", Int2String(x));
			textElement.SetAttribute("y", Int2String(y));

			return textElement;
		}


		public System.Xml.XmlNode AddLine(netDxf.Vector3 p1, netDxf.Vector3 p2)
		{
			System.Xml.XmlElement e = CreateLine(p1, p2);
			return this.RootNode.AppendChild (e);
		}


		public System.Xml.XmlNode AddLine(int x1, int y1, int x2, int y2)
		{
			System.Xml.XmlElement e =  CreateLine(x1, y2, x2, y2);
			return this.RootNode.AppendChild (e);
		}


		public System.Xml.XmlElement CreateLine(netDxf.Vector3 p1, netDxf.Vector3 p2)
		{
			return CreateLine((int) p1.X, (int) p1.Y, (int) p2.X, (int) p2.Y);
		}


		public System.Xml.XmlElement CreateLine(int x1, int y1, int x2, int y2)
		{
			System.Xml.XmlElement line = this.SvgDocument.CreateElement("line", SVGnamespace);
			line.SetAttribute("x1", Int2String(x1));
			line.SetAttribute("y1", Int2String(y1));
			line.SetAttribute("x2", Int2String(x2));
			line.SetAttribute("y2", Int2String(y2));
			line.SetAttribute("style", "stroke:rgb(255,0,0);stroke-width:2");

			return line;
		}


		public System.Xml.XmlElement CreateRectangle(int x, int y, int w, int h)
		{
			System.Xml.XmlElement rect = this.SvgDocument.CreateElement("rect", SVGnamespace);
			rect.SetAttribute("x", Int2String(x));
			rect.SetAttribute("y", Int2String(y));
			rect.SetAttribute("width", Int2String(w));
			rect.SetAttribute("height", Int2String(h));
			rect.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");

			return rect;
		}


		public System.Xml.XmlElement CreateCircle(int cx, int cy, int r)
		{
			System.Xml.XmlElement circle = this.SvgDocument.CreateElement("circle", SVGnamespace);
			circle.SetAttribute("cx", Int2String(cx));
			circle.SetAttribute("cy", Int2String(cy));
			circle.SetAttribute("r", Int2String(r));
			circle.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");
			// circle.SetAttribute ("stroke", "black");
			// circle.SetAttribute ("stroke-width", "3");
			// circle.SetAttribute ("fill", "red");

			return circle;
		}


		public System.Xml.XmlElement CreateEllipse(int cx, int cy, int rx, int ry)
		{
			System.Xml.XmlElement ellipse = this.SvgDocument.CreateElement("ellipse", SVGnamespace);
			ellipse.SetAttribute("cx", Int2String(cx));
			ellipse.SetAttribute("cy", Int2String(cy));
			ellipse.SetAttribute("rx", Int2String(rx));
			ellipse.SetAttribute("ry", Int2String(ry));
			ellipse.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");
			// ellipse.SetAttribute ("stroke", "black");
			// ellipse.SetAttribute ("stroke-width", "3");
			// ellipse.SetAttribute ("fill", "red");

			return ellipse;
		}


		public System.Xml.XmlElement CreatePolygon(string points)
		{
			System.Xml.XmlElement polygon = this.SvgDocument.CreateElement("polygon", SVGnamespace);
			polygon.SetAttribute("points", points);
			polygon.SetAttribute("style", "fill:rgb(0,0,255);stroke-width:3;stroke:rgb(0,0,0)");
			// polygon.SetAttribute ("stroke", "black");
			// polygon.SetAttribute ("stroke-width", "3");
			// polygon.SetAttribute ("fill", "red");

			return polygon;
		}


		public System.Xml.XmlElement CreatePolyline(string points)
		{
			System.Xml.XmlElement polyline = this.SvgDocument.CreateElement("polyline", SVGnamespace);
			polyline.SetAttribute("points", points);
			polyline.SetAttribute("style", "fill:none;stroke-width:3;stroke:rgb(0,0,0)");
			// polyline.SetAttribute ("stroke", "black");
			// polyline.SetAttribute ("stroke-width", "3");
			// polyline.SetAttribute ("fill", "red");

			return polyline;
		}


		public System.Xml.XmlElement CreatePath(string path)
		{
			System.Xml.XmlElement xp = this.SvgDocument.CreateElement("path", SVGnamespace);
			xp.SetAttribute("d", path);
			xp.SetAttribute("style", "fill:rgb(255,0,0);stroke-width:3;stroke:rgb(0,0,0)");
			// xp.SetAttribute ("stroke", "black");
			// xp.SetAttribute ("stroke-width", "3");
			// xp.SetAttribute ("fill", "red");

			return xp;
		}


		public bool Save(string path)
		{
			try
			{
				this.SvgDocument.Save (path);
				return false;
			}
			catch(System.Exception ex)
			{
				System.Console.WriteLine (ex.Message);
			}

			return true;
		}


		protected static string Int2String(int i)
		{
			return i.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}


	}


} 
