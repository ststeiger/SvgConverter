using System;
using System.Collections.Generic;
using System.Text;

namespace ApertureService
{
    class SvgUnDuplicate
    {



        public static void TransformPath()
        {
            string file = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            file = System.IO.Path.Combine(file, "..");
            file = System.IO.Path.Combine(file, "..");
            file = System.IO.Path.Combine(file, "0001_GB01_OG14_0000_Aperture.svg");
            file = System.IO.Path.GetFullPath(file);

            file = @"d:\8001_GB01_EG00_0000.svg";
            file = @"d:\7602_GB01_EG00_0000.svg";
            file = @"d:\3507_GB01_EG00_0000.svg";

            TransformPath(file);
        } // End Sub TransformPath 


        public static void TransformPath(string file)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            if (System.Environment.OSVersion.Platform != System.PlatformID.Unix)
                doc.XmlResolver = null; // .NET Framework

            doc.Load(file);

            System.Xml.XmlNamespaceManager nspmgr = new System.Xml.XmlNamespaceManager(doc.NameTable);
            nspmgr.AddNamespace(doc.DocumentElement.Name, doc.DocumentElement.NamespaceURI);

            bool bOnlyRoomLayer = true;

            if (bOnlyRoomLayer)
            {
                System.Xml.XmlNodeList pathLayers = doc.SelectNodes("//svg:g[@id!='FM_OBJEKT_RAUM']", nspmgr);
                foreach (System.Xml.XmlNode layer in pathLayers)
                {
                    layer.ParentNode.RemoveChild(layer);
                } // Next layer
            } // End if (bOnlyRoomLayer)



            System.Xml.XmlElement xe = doc.CreateElement("style", doc.DocumentElement.NamespaceURI);
            xe.SetAttribute("type", "text/css");

            xe.InnerText = @"
path:hover {
    fill: orange;
    stroke-width: 10;
    stroke: gold;
    transition: all .2s ease-in-out; 
    transform: scale(1.0);
}
    
path
{
    transform: scale(1.0);
}
";

            if (doc.DocumentElement.FirstChild == null)
                doc.DocumentElement.AppendChild(xe);
            else
                doc.DocumentElement.InsertBefore(xe, doc.DocumentElement.FirstChild);



            // System.Xml.XmlNode nd = doc.SelectSingleNode("//svg:g[@id='FM_OBJEKT_RAUM']", nspmgr);
            // System.Xml.XmlNodeList paths = nd.SelectNodes ("./svg:path", nspmgr);
            System.Xml.XmlNodeList paths = doc.SelectNodes("//svg:g[@id='FM_OBJEKT_RAUM']/svg:path", nspmgr);
            // System.Xml.XmlNodeList paths = doc.SelectNodes("//svg:g[@id='FM_OBJEKT_RAUM']/svg:path[@data-handle='4']", nspmgr);

            System.Console.WriteLine(paths);


            int r = 20;

            foreach (System.Xml.XmlNode path in paths)
            {

                // string col = string.Format("#0000{0:X2}", r);
                // path.Attributes["fill"].Value = "#FF0000";
                path.Attributes["fill"].Value = string.Format("#00{0:X2}00", r);
                r += 37;
                r = r % 255;

                System.Xml.XmlAttribute da = path.Attributes["d"];
                if (da == null)
                    continue;


                string d = da.Value.Trim();
                if (d.StartsWith("M"))
                    d = d.Substring(1);
                if (d.EndsWith("z"))
                    d = d.Substring(0, d.Length - 1);

                string[] coords = d.Split('L');


                System.Collections.Generic.List<System.Drawing.Point> points =
                    new System.Collections.Generic.List<System.Drawing.Point>();


                foreach (string coord in coords)
                {
                    string[] xyc = coord.Split(' ');

                    float fX = 0;
                    float fY = 0;
                    float.TryParse(xyc[0], out fX);
                    float.TryParse(xyc[1], out fY);

                    // points.Add(new double[] { dblX, dblY });
                    points.Add(new System.Drawing.Point((int)fX, (int)fY));
                    System.Console.WriteLine("Pxy: [{0},{1}]", fX, fY);
                } // Next coord


                int j = points.Count - 1;
                for (int i = 0; i < points.Count; ++i)
                {
                    if (j <= i)
                        break;

                    if (PointsEqual(points[i], points[j]))
                        points.RemoveAt(j);
                    else
                    {
                        System.Console.WriteLine(points[i] == points[j]);
                        break;
                    }


                    j--;
                } // Next i

                string str = "";
                for (int i = 0; i < points.Count; ++i)
                {
                    if (i == 0 && points.Count == 1)
                        str += string.Format("M{0} {1} z", points[i].X, points[i].Y);
                    else if (i == 0)
                        str += string.Format("M{0} {1}", points[i].X, points[i].Y);
                    else if (i == points.Count - 1)
                        str += string.Format("L{0} {1} z", points[i].X, points[i].Y);
                    else
                        str += string.Format("L{0} {1}", points[i].X, points[i].Y);
                } // Next i

                // System.Console.WriteLine(str);
                path.Attributes["d"].Value = str;
            } // Next path

            string newFileName = null;
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                newFileName = @"/root/" + "Rooms_" + System.IO.Path.GetFileNameWithoutExtension(file) + @".svg";
            else
                newFileName = @"D:\" + "Rooms_" + System.IO.Path.GetFileNameWithoutExtension(file) + @".svg";

            doc.Save(newFileName);

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub TransformPath


        public static bool PointsEqual(System.Drawing.Point pt1, System.Drawing.Point pt2)
        {
            // return (pt1 == pt2);

            int epsilonX = 500;
            int epsilonY = epsilonX;

            int xmin = pt1.X - epsilonX;
            int xmax = pt1.X + epsilonX;

            int ymin = pt1.Y - epsilonY;
            int ymax = pt1.Y + epsilonY;

            if (pt2.X >= xmin && pt2.X <= xmax && pt2.Y >= ymin && pt2.Y <= ymax)
                return true;

            return false;
        } // End Function PointsEqual 


    }
}
