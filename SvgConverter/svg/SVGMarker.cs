// <copyright file="SVGMarker.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{
    public class SVGMarker : ISVGElement
    {
        public SVGMarker(ISVGElement parent, string id)
        {
            System.Xml.XmlElement parentElement = parent.Element;
            System.Xml.XmlDocument document = parentElement.OwnerDocument;
            System.Xml.XmlElement element = document.CreateElement("marker");
            parentElement.AppendChild(element);

            System.Xml.XmlAttribute idAttr = document.CreateAttribute("id");
            idAttr.Value = id;
            element.Attributes.Append(idAttr);

            (this as ISVGElement).Element = element;
        }

        System.Xml.XmlElement ISVGElement.Element
        {
            get;
            set;
        }
    }
}