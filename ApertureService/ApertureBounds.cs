
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



        public double XUL;
        public double YUL;
        public double XLR;
        public double YLR;


        public string LogText;


        public ApertureBounds()
        { }





        public ApertureBounds(double pL, double pR, double pT, double pB)
        {
            this.L = pL;
            this.R = pR;
            this.T = pT;
            this.B = pB;


            this.sL = this.L.ToString();
            this.sR = this.R.ToString();
            this.sT = this.T.ToString();
            this.sB = this.B.ToString();

            this.XUL = this.L;
            this.YUL = this.T;
            this.XLR = this.R;
            this.YLR = this.B;
        }




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

            this.XUL = System.Math.Round( this.L, 1);
            this.YUL = System.Math.Round( this.T, 1);
            this.XLR = System.Math.Round( this.R, 1);
            this.YLR = System.Math.Round( this.B, 1);

            System.Xml.XmlNode asl = doc.SelectSingleNode("//ApxScriptLog");
            this.LogText = asl.InnerText;
        }

    }


}
