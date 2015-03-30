
// using WW.Cad.Base;
// using WW.Cad.Drawing;

// using WW.Cad.IO;
// using WW.Cad.Model;
// using WW.Math;


// https://www.woutware.com/doc/cadlib3.5/html/2c8ca807-0eef-14e1-fa3b-d4704091db3c.htm
namespace DwgToSvgConverter
{


    public class Export
    {


        public static void ExportToSvg(WW.Cad.Model.DxfModel model, string path)
        {
            WW.Cad.Drawing.BoundsCalculator boundsCalculator = new WW.Cad.Drawing.BoundsCalculator();
            boundsCalculator.GetBounds(model);
            WW.Math.Bounds3D bounds = boundsCalculator.Bounds;

            System.Drawing.Printing.PaperSize paperSize = WW.Cad.Drawing.PaperSizes.GetPaperSize(System.Drawing.Printing.PaperKind.A4);

            // Lengths in hundredths of cm.
            const float hundredthsInchToCm = 2.54f / 100f;
            float pageWidth = paperSize.Width * hundredthsInchToCm * 100f;
            float pageHeight = paperSize.Height * hundredthsInchToCm * 100f;
            // float margin = 200f;
            float margin = 0f;

            // Scale and transform such that its fits max width/height
            // and the top left middle of the cad drawing will match the 
            // top middle of the svg page.
            WW.Math.Matrix4D to2DTransform = WW.Cad.Base.DxfUtil.GetScaleTransform(
                bounds.Corner1,
                bounds.Corner2,
                new WW.Math.Point3D(bounds.Center.X, bounds.Corner2.Y, 0d),
                new WW.Math.Point3D(margin, pageHeight - margin, 0d),
                new WW.Math.Point3D(pageWidth - margin, margin, 0d),
                new WW.Math.Point3D(pageWidth / 2d, margin, 0d)
            );

            using (System.IO.Stream stream = System.IO.File.Create(path))
            {
                WW.Cad.IO.SvgExporter exporter = new WW.Cad.IO.SvgExporter(stream, paperSize);

                exporter.Title = "tempSvg";
                exporter.ExportCadLayersAsSvgGroups = true;
                exporter.WriteSvgXmlElementAttributes += AdditionalAttribute;
                // exporter.WriteSvgXmlElementAttributes += OverwriteStrokeWidth;
                exporter.WriteBackgroundRectangle = false;
                

                WW.Cad.Drawing.GraphicsConfig gc = new WW.Cad.Drawing.GraphicsConfig();
                gc.FixedForegroundColor = WW.Drawing.ArgbColor.FromArgb(0, WW.Drawing.ArgbColor.FromRgb(255, 0, 0));
                gc.CorrectColorForBackgroundColor = false;
                gc.ShowDimensionDefinitionPoints = true;
                gc.BackColor = WW.Drawing.ArgbColor.FromArgb(0, WW.Drawing.ArgbColors.White);

                // exporter.Draw(model, WW.Cad.Drawing.GraphicsConfig.WhiteBackgroundCorrectForBackColor, to2DTransform);
                exporter.Draw(model, gc, to2DTransform);
            }

        }


        // For better visibility when testing.
        public static void OverwriteStrokeWidth(System.Xml.XmlTextWriter w, WW.Cad.Model.Entities.DxfEntity entity)
        {
            w.WriteAttributeString("stroke-width", "5");
        }


        public static void AdditionalAttribute(System.Xml.XmlTextWriter w, WW.Cad.Model.Entities.DxfEntity entity)
        {
            w.WriteAttributeString("data-handle", entity.Handle.ToString());
        }


    }


}
