// <copyright file="SVGAttributes.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{
    public static class SVGAttributes
    {
        private static System.Xml.XmlAttribute GetAttribute(ISVGElement svgElement, string name)
        {
            System.Xml.XmlElement element = (svgElement as ISVGElement).Element;
            System.Xml.XmlDocument document = element.OwnerDocument;
            System.Xml.XmlAttribute attribute = element.Attributes[name];
            if (null == attribute)
            {
                attribute = document.CreateAttribute(name);
                element.Attributes.Append(attribute);
            }

            return attribute;
        }

        public static System.Xml.XmlAttribute Fill(ISVGElement svgElement, string value)
        {
            System.Xml.XmlAttribute attribute = GetAttribute(svgElement, "fill");
            attribute.Value = value;
            return attribute;
        }

        public static System.Xml.XmlAttribute Stroke(ISVGElement svgElement, string value)
        {
            System.Xml.XmlAttribute attribute = GetAttribute(svgElement, "stroke");
            attribute.Value = value;
            return attribute;
        }

        public static System.Xml.XmlAttribute StrokeWidth(ISVGElement svgElement, int value)
        {
            System.Xml.XmlAttribute attribute = GetAttribute(svgElement, "stroke-width");
            attribute.Value = value.ToString();
            return attribute;
        }
    }
}