// <copyright file="SVGDefinitions.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{
    public class SVGDefinitions : ISVGElement
    {
        public SVGDefinitions(ISVGElement parent)
        {
            System.Xml.XmlElement parentElement = parent.Element;
            System.Xml.XmlElement element = parentElement.OwnerDocument.CreateElement("defs");
            parentElement.AppendChild(element);

            (this as ISVGElement).Element = element;
        }

        System.Xml.XmlElement ISVGElement.Element
        {
            get;
            set;
        }
    }
}