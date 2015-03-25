// <copyright file="SVG.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{
    public class SVG : ISVGElement
    {
        public SVG(System.Xml.XmlDocument document, string SVGnamespace, string version)
        {
            System.Xml.XmlElement element = document.CreateElement("svg", SVGnamespace);

            System.Xml.XmlAttribute attribute = document.CreateAttribute("version");;
            attribute.Value = version;
            element.Attributes.Append(attribute);

            (this as ISVGElement).Element = element;
        }

        System.Xml.XmlElement ISVGElement.Element
        {
            get;
            set;
        }
    }
}