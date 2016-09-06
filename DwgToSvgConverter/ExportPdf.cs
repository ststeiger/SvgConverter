
using WW.Math;
using WW.Cad.Model;

using WW.Cad.Model.Objects;
using WW.Cad.IO;
using WW.Cad.Base;
// using WW.Cad.Drawing.;


namespace DwgToSvgConverter
{


    // https://www.woutware.com/Forum/Topic/1207/drawing-explanation-in-dwg-doesnt-export-to-p?returnUrl=%2FForum%2FUserPosts%3FuserId%3D2100797360
    class ExportPdf
    {


        public static System.Drawing.Printing.PaperSize CreatePaperSize(string paperFormat)
        {
            System.Drawing.Printing.PaperSize paperSize = null;

            // http://www.papersizes.org/a-paper-sizes.htm
            switch (paperFormat.ToString())
            {
                //constructor "name", inch, inch
                case "A3":
                    paperSize = new System.Drawing.Printing.PaperSize("A3", 1170, 1650);
                    break;
                case "A4":
                    // A4	210 x 297 mm	8.3 x 11.7 in
                    // Width/Height: in hundredths of an inch. https://msdn.microsoft.com/en-us/library/system.drawing.printing.papersize(v=vs.110).aspx
                    paperSize = new System.Drawing.Printing.PaperSize("A4", 830, 1170);
                    paperSize.RawKind = (int)System.Drawing.Printing.PaperKind.A4;
                    break;
                case "A5":
                    paperSize = new System.Drawing.Printing.PaperSize("A5", 580, 830);
                    break;
                case "A6":
                    paperSize = new System.Drawing.Printing.PaperSize("A6", 410, 580);
                    break;
                case "A7":
                    paperSize = new System.Drawing.Printing.PaperSize("A7", 290, 410);
                    break;
                case "A8":
                    paperSize = new System.Drawing.Printing.PaperSize("A8", 200, 290);
                    break;
                case "A9":
                    paperSize = new System.Drawing.Printing.PaperSize("A9", 150, 200);
                    break;
                case "A10":
                    paperSize = new System.Drawing.Printing.PaperSize("A10", 100, 150);
                    break;
                case "B3":
                    paperSize = new System.Drawing.Printing.PaperSize("B3", 1390, 1970);
                    break;
                case "B4":
                    paperSize = new System.Drawing.Printing.PaperSize("B4", 980, 1390);
                    break;
                case "B5":
                    paperSize = new System.Drawing.Printing.PaperSize("B5", 690, 980);
                    break;
                case "B6":
                    paperSize = new System.Drawing.Printing.PaperSize("B6", 490, 690);
                    break;
                case "B7":
                    paperSize = new System.Drawing.Printing.PaperSize("B7", 350, 490);
                    break;
                case "B8":
                    paperSize = new System.Drawing.Printing.PaperSize("B8", 240, 350);
                    break;
                case "B9":
                    paperSize = new System.Drawing.Printing.PaperSize("B9", 170, 240);
                    break;
                case "B10":
                    paperSize = new System.Drawing.Printing.PaperSize("B10", 120, 170);
                    break;
                case "C3":
                    paperSize = new System.Drawing.Printing.PaperSize("C3", 1280, 1800);
                    break;
                case "C4":
                    paperSize = new System.Drawing.Printing.PaperSize("C4", 900, 1280);
                    break;
                case "C5":
                    paperSize = new System.Drawing.Printing.PaperSize("C5", 640, 900);
                    break;
                case "C6":
                    paperSize = new System.Drawing.Printing.PaperSize("C6", 450, 640);
                    break;
                case "C7":
                    paperSize = new System.Drawing.Printing.PaperSize("C7", 320, 450);
                    break;
                case "C8":
                    paperSize = new System.Drawing.Printing.PaperSize("C8", 220, 320);
                    break;
                case "C9":
                    paperSize = new System.Drawing.Printing.PaperSize("C9", 160, 220);
                    break;
                case "C10":
                    paperSize = new System.Drawing.Printing.PaperSize("C10", 110, 160);
                    break;
                case "DL":
                    paperSize = new System.Drawing.Printing.PaperSize("C10", 430, 860);
                    break;
                default:
                    paperSize = new System.Drawing.Printing.PaperSize("A5", 580, 830);
                    break;
            }

            // paperSize.RawKind = (int)PaperKind.Custom;

            return paperSize;
        }


        
        
        public static void WriteDefaultLayoutToPdf(DxfModel model, 
                                                    float margin, string outfile,
                                                    bool embedFonts, short lineWeight)
        {
            System.Drawing.Printing.PaperSize paperSize = ExportPdf.CreatePaperSize("A4");

            WriteDefaultLayoutToPdf(model, paperSize, margin, outfile, embedFonts, lineWeight);
        }


        /// <summary>
        /// Write the default layout of a model to a PDF file.
        /// </summary>
        /// <remarks>
        /// Depending on the model's setting this either writes the active layout or the model space to the PDF.
        /// This code will center the model/layout on top of the page.
        /// </remarks>
        /// <param name="model">DXF model to write</param>
        /// <param name="paperSize">targeted paper size</param>
        /// <param name="margin">margin around model print in inches</param>
        /// <param name="outfile">path of PDF output file</param>
        /// <param name="embedFonts">embed fonts into PDF?</param>
        /// <param name="lineWeight">default line weight in 100th of mm</param>
        public static void WriteDefaultLayoutToPdf(DxfModel model, System.Drawing.Printing.PaperSize paperSize,
                                                    float margin, string outfile,
                                                    bool embedFonts, short lineWeight)
        {
            DxfLayout layout = model.Header.ShowModelSpace
                                   ? null
                                   : model.ActiveLayout;
            if (layout != null)
            {
                // output layout
                WriteLayoutToPdf(model, layout, paperSize, margin, outfile, embedFonts, lineWeight);
            }
            else
            {
                // output model
                WriteModelToPdf(model, paperSize, margin, outfile, embedFonts, lineWeight);
            }
        }


        /// <summary>
        /// Write a complete model to a PDF file.
        /// </summary>
        /// <remarks>
        /// The model is scaled to fit the paper inside the margin.
        /// This code will center the model on top of the page.
        /// </remarks>
        /// <param name="model">DXF model to write</param>
        /// <param name="paperSize">targeted paper size</param>
        /// <param name="margin">margin around model print in inches</param>
        /// <param name="outfile">path of PDF output file</param>
        /// <param name="embedFonts">embed fonts into PDF?</param>
        /// <param name="lineWeight">default line weight in 100th of mm</param>
        private static void WriteModelToPdf(DxfModel model, System.Drawing.Printing.PaperSize paperSize, float margin, string outfile, bool embedFonts, short lineWeight)
        {
            WW.Cad.Drawing.BoundsCalculator boundsCalculator = new WW.Cad.Drawing.BoundsCalculator();
            boundsCalculator.GetBounds(model);
            Bounds3D bounds = boundsCalculator.Bounds;

            // Lengths in inches.
            float pageWidth = paperSize.Width / 100f;
            float pageHeight = paperSize.Height / 100f;
            // Scale and transform such that its fits max width/height
            // and the top middle of the cad drawing will match the 
            // top middle of the pdf page.
            // The transform transforms to pdf pixels.
            double scaling;
            Matrix4D to2DTransform = DxfUtil.GetScaleTransform(
                bounds.Corner1,
                bounds.Corner2,
                new Point3D(bounds.Center.X, bounds.Corner2.Y, 0d),
                new Point3D(new Vector3D(margin, margin, 0d) * PdfExporter.InchToPixel),
                new Point3D(new Vector3D(pageWidth - margin, pageHeight - margin, 0d) * PdfExporter.InchToPixel),
                new Point3D(new Vector3D(pageWidth / 2d, pageHeight - margin, 0d) * PdfExporter.InchToPixel),
                out scaling
                );
            using (System.IO.Stream stream = System.IO.File.Create(outfile))
            {
                PdfExporter pdfGraphics = new PdfExporter(stream);
                pdfGraphics.ExportLayers = true;
                pdfGraphics.UseMultipleLayers = false;
                pdfGraphics.EmbedFonts = embedFonts;
                WW.Cad.Drawing.GraphicsConfig config = (WW.Cad.Drawing.GraphicsConfig)WW.Cad.Drawing.GraphicsConfig.AcadLikeWithWhiteBackground.Clone();
                config.TryDrawingTextAsText = embedFonts;
                config.DefaultLineWeight = lineWeight;
                pdfGraphics.DrawPage(
                    model,
                    config,
                    to2DTransform,
                    scaling,
                    null,
                    null,
                    paperSize
                    );
                pdfGraphics.EndDocument();
            }
        }

        /// <summary>
        /// Write a layout to a PDF file.
        /// </summary>
        /// <remarks>
        /// The layout is scaled to fit the paper inside the margin.
        /// This code will center the layout on top of the page.
        /// </remarks>
        /// <param name="model">DXF model to write</param>
        /// <param name="layout">DXF layout to write</param>
        /// <param name="paperSize">targeted paper size</param>
        /// <param name="margin">margin around model print in inches</param>
        /// <param name="outfile">path of PDF output file</param>
        /// <param name="embedFonts">embed fonts into PDF?</param>
        /// <param name="lineWeight">default line weight in 100th of mm</param>
        private static void WriteLayoutToPdf(DxfModel model, DxfLayout layout, System.Drawing.Printing.PaperSize paperSize, float margin, string outfile, bool embedFonts, short lineWeight)
        {

            WW.Cad.Drawing.BoundsCalculator boundsCalculator = new WW.Cad.Drawing.BoundsCalculator();
            boundsCalculator.GetBounds(model, layout);
            Bounds3D bounds = boundsCalculator.Bounds;

            // Lengths in inches.
            float pageWidth = paperSize.Width / 100f;
            float pageHeight = paperSize.Height / 100f;
            // Scale and transform such that its fits max width/height
            // and the top left middle of the cad drawing will match the 
            // top middle of the pdf page.
            // The transform transforms to pdf pixels.
            double scaling;
            Matrix4D to2DTransform = DxfUtil.GetScaleTransform(
                bounds.Corner1,
                bounds.Corner2,
                new Point3D(bounds.Center.X, bounds.Corner2.Y, 0d),
                new Point3D(new Vector3D(margin, margin, 0d) * PdfExporter.InchToPixel),
                new Point3D(new Vector3D(pageWidth - margin, pageHeight - margin, 0d) * PdfExporter.InchToPixel),
                new Point3D(new Vector3D(pageWidth / 2d, pageHeight - margin, 0d) * PdfExporter.InchToPixel),
                out scaling
                );
            using (System.IO.Stream stream = System.IO.File.Create(outfile))
            {
                PdfExporter pdfGraphics = new PdfExporter(stream);
                pdfGraphics.EmbedFonts = embedFonts;
                // pdfGraphics.PackContent = false;
                WW.Cad.Drawing.GraphicsConfig config = new WW.Cad.Drawing.GraphicsConfig(System.Drawing.Color.White, true);
                config.TryDrawingTextAsText = embedFonts;
                config.DefaultLineWeight = lineWeight;
                pdfGraphics.DrawPage(
                    model,
                    config,
                    to2DTransform,
                    scaling,
                    layout,
                    null,
                    paperSize
                    );
                pdfGraphics.EndDocument();
            }
        }


    }
}
