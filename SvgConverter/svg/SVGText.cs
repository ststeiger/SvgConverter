// <copyright file="SVGText.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{
    public class SVGText : ISVGElement
    {
        public SVGText(ISVGElement parent, string text, int x, int y, int width, int height, string units)
        {
            System.Xml.XmlElement parentElement = parent.Element;
            System.Xml.XmlDocument document = parentElement.OwnerDocument;
            System.Xml.XmlElement element = document.CreateElement("text");
            parentElement.AppendChild(element);

            element.InnerText = text;

            System.Xml.XmlAttribute xAttr = document.CreateAttribute("x");
            xAttr.Value = System.String.Format("{0}{1}", x, units);
            element.Attributes.Append(xAttr);

            System.Xml.XmlAttribute yAttr = document.CreateAttribute("y");
            yAttr.Value = System.String.Format("{0}{1}", y, units);
            element.Attributes.Append(yAttr);

            System.Xml.XmlAttribute widthAttr = document.CreateAttribute("width");
            widthAttr.Value = System.String.Format("{0}{1}", width, units);
            element.Attributes.Append(widthAttr);

            System.Xml.XmlAttribute heightAttr = document.CreateAttribute("height");
            heightAttr.Value = System.String.Format("{0}{1}", height, units);
            element.Attributes.Append(heightAttr);

            (this as ISVGElement).Element = element;
        }

        System.Xml.XmlElement ISVGElement.Element
        {
            get;
            set;
        }
    }
}