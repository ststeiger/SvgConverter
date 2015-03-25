// <copyright file="HTMLDocument.cs" company="Mark Final">
//  Opus
// </copyright>
// <summary>Opus Core</summary>
// <author>Mark Final</author>
namespace Opus.Core
{

#if false


    public class HTMLDocument
    {
        private static string SVGNameSpace = "http://www.w3.org/2000/svg";

        public HTMLDocument(string pathName)
        {
            this.PathName = pathName;
            this.Document = new System.Xml.XmlDocument();

            System.Xml.XmlElement htmlElement = this.Document.CreateElement("html");
            this.Document.AppendChild(htmlElement);
            System.Xml.XmlElement bodyElement = this.Document.CreateElement("body");
            htmlElement.AppendChild(bodyElement);

            this.BodyElement = bodyElement;
        }

        private string PathName
        {
            get;
            set;
        }

        private System.Xml.XmlDocument Document
        {
            get;
            set;
        }

        private System.Xml.XmlElement BodyElement
        {
            get;
            set;
        }

        public void CreateSVGDependencyGraph(DependencyGraph graph)
        {
            SVG svg = new SVG(this.Document, SVGNameSpace, "1.1");
            this.BodyElement.AppendChild((svg as ISVGElement).Element);

            SVGDefinitions definitions = new SVGDefinitions(svg);

            System.Collections.Generic.Dictionary<DependencyNode, SVGGroup> nodeToSVGGroup = new System.Collections.Generic.Dictionary<DependencyNode, SVGGroup>();

            int rankCount = graph.RankCount;
            int numVerticalSlots = 2 * rankCount + 1;
            float verticalStride = 100 / numVerticalSlots;
            int y = (int)verticalStride; ;
            for (int i = rankCount - 1; i >= 0; --i)
            {
                DependencyNodeCollection rank = graph[i];

                int nodeCount = rank.Count;
                int numHorizontalSlots = 2 * nodeCount + 1;
                float horizontalStride = 100 / numHorizontalSlots;
                int x = (int)horizontalStride;

                System.Text.StringBuilder rankIdName = new System.Text.StringBuilder();
                rankIdName.AppendFormat("Rank{0}", rank.Rank);

                SVGGroup rankGroup = new SVGGroup(svg, rankIdName.ToString());
                SVGAttributes.Fill(rankGroup, "red");

                SVGText rankText = new SVGText(rankGroup, rankIdName.ToString(), 0, y, 100, (int)verticalStride, "%");
                SVGAttributes.Fill(rankText, "black");

                foreach (DependencyNode node in rank)
                {
                    SVGGroup nodeGroup = new SVGGroup(rankGroup, node.UniqueModuleName);

                    SVGRect nodeRect = new SVGRect(nodeGroup, x, y, (int)horizontalStride, (int)verticalStride, "%");

                    SVGText nodeText = new SVGText(nodeGroup, node.UniqueModuleName, x, y + (int)verticalStride / 2, (int)horizontalStride, (int)verticalStride, "%");
                    SVGAttributes.Fill(nodeText, "black");

                    nodeToSVGGroup.Add(node, nodeGroup);

                    x += (int)(2 * horizontalStride);
                }

                y += (int)(verticalStride * 2);
            }

            SVGArrow arrow = new SVGArrow(svg, 0, 0, 100, 100, "");
            SVGAttributes.Stroke(arrow, "black");
            SVGAttributes.StrokeWidth(arrow, 5);

#if false
            System.Xml.XmlElement circle = this.Document.CreateElement("circle");
            System.Xml.XmlAttribute cx = this.Document.CreateAttribute("cx");
            cx.Value = "100";
            System.Xml.XmlAttribute cy = this.Document.CreateAttribute("cy");
            cy.Value = "50";
            System.Xml.XmlAttribute r = this.Document.CreateAttribute("r");
            r.Value = "40";
            circle.Attributes.Append(cx);
            circle.Attributes.Append(cy);
            circle.Attributes.Append(r);
            svgElement.AppendChild(circle);
#endif
        }

        public void Write()
        {
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.CloseOutput = true;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = false;

            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(this.PathName, settings))
            {
                this.Document.Save(writer);
            }
        }
    }
#endif

}
