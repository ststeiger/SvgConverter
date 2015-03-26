
namespace ApertureService
{


    public class ApertureBounds
    {

        public double L;
        public double R;
        public double T;
        public double B;



        public string sL;
        public string sR;
        public string sT;
        public string sB;


        public string LogText;


        public ApertureBounds()
        { }

        public ApertureBounds(string s)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(s);

            System.Xml.XmlNode nL = doc.SelectSingleNode("//L");
            System.Xml.XmlNode nR = doc.SelectSingleNode("//R");
            System.Xml.XmlNode nT = doc.SelectSingleNode("//T");
            System.Xml.XmlNode nB = doc.SelectSingleNode("//B");

            sL = nL.InnerText;
            sR = nR.InnerText;
            sT = nT.InnerText;
            sB = nB.InnerText;


            double.TryParse(sL, out this.L);
            double.TryParse(sR, out this.R);
            double.TryParse(sT, out this.T);
            double.TryParse(sB, out this.B);

            System.Xml.XmlNode asl = doc.SelectSingleNode("//ApxScriptLog");
            this.LogText = asl.InnerText;
        }

    }


}
