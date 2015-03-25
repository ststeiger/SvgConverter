// <copyright file="SVGArrow.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{
    public class SVGArrow : ISVGElement
    {
        public SVGArrow(ISVGElement parent, int x1, int y1, int x2, int y2, string units)
        {
            System.Xml.XmlElement parentElement = parent.Element;
            System.Xml.XmlDocument document = parentElement.OwnerDocument;
            System.Xml.XmlElement element = document.CreateElement("line");
            parentElement.AppendChild(element);

            System.Xml.XmlAttribute x1Attr = document.CreateAttribute("x1");
            x1Attr.Value = System.String.Format("{0}{1}", x1, units);
            element.Attributes.Append(x1Attr);

            System.Xml.XmlAttribute y1Attr = document.CreateAttribute("y1");
            y1Attr.Value = System.String.Format("{0}{1}", y1, units);
            element.Attributes.Append(y1Attr);

            System.Xml.XmlAttribute x2Attr = document.CreateAttribute("x2");
            x2Attr.Value = System.String.Format("{0}{1}", x2, units);
            element.Attributes.Append(x2Attr);

            System.Xml.XmlAttribute y2Attr = document.CreateAttribute("y2");
            y2Attr.Value = System.String.Format("{0}{1}", y2, units);
            element.Attributes.Append(y2Attr);

            (this as ISVGElement).Element = element;
        }

        System.Xml.XmlElement ISVGElement.Element
        {
            get;
            set;
        }
    }
}