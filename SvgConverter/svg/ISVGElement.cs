// <copyright file="ISVGElement.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{
    public interface ISVGElement
    {
        System.Xml.XmlElement Element
        {
            get;
            set;
        }
    }
}