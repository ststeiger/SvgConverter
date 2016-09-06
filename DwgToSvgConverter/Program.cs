
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Drawing;

using WW.Math;


namespace DwgToSvgConverter
{

    
    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
#if false
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new Form1());
            return;
#endif

            string filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // filename = System.IO.Path.Combine(filename, "..", "..", "Zimmertyp_1_.dwg");
            //filename = System.IO.Path.Combine(filename, "..", "..", "..", "SvgConverter", "drawing.dxf");
            //filename = System.IO.Path.Combine(filename, "..", "..", "..", "ApertureService", "0001_GB01_OG14_0000_Aperture.dxf");
            filename = System.IO.Path.Combine(filename, "..", "..", "0001_GB01_OG14_0000_Aperture_dxf13.dxf");
            

            filename = System.IO.Path.GetFullPath(filename);
            // filename = string.Format( @"D:\{0}\Downloads\7602_GB01_OG01_0000_Aperture.dxf", Environment.UserName);
            // filename = string.Format(@"D:\{0}\Downloads\8001_GB01_EG00_0000.dxf", Environment.UserName); // JB
            // filename = string.Format(@"D:\{0}\Downloads\3507_GB01_EG00_0000.dxf", Environment.UserName); // WC
            // filename = string.Format(@"D:\{0}\Downloads\7602_GB01_EG00_0000.dxf", Environment.UserName); // WC
            


            MessageBoxHandler.CloseNextMessageBoxByTitle("Wout Ware trial"); // Annoying
            DxfModel model;
            string extension = System.IO.Path.GetExtension(filename);
            if (string.Compare(extension, ".dwg", true) == 0)
            {
                model = DwgReader.Read(filename);
            }
            else
            {
                model = DxfReader.Read(filename);
            }



            /*
            foreach (WW.Cad.Model.Objects.DxfGroup thisGroup in model.Groups)
            {
                
            } // Next thisGroup 
            



            foreach (WW.Cad.Model.Tables.DxfBlock thisBlock in model.AnonymousBlocks)
            {
                foreach (WW.Cad.Model.Entities.DxfEntity ent in thisBlock.Entities)
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", ent.Layer))
                    {
                        System.Console.WriteLine(ent.AcClass);
                        System.Console.WriteLine(ent.LineWeight);
                        System.Console.WriteLine(ent.EntityType);
                        System.Console.WriteLine(ent.LineType);
                        System.Console.WriteLine(ent.LineTypeScale);
                        System.Console.WriteLine(ent.Layer);
                    }
                } // Next ent
                
            } // Next thisBlock

            
            foreach (WW.Cad.Model.Tables.DxfBlock thisBlock in model.Blocks)
            {
                foreach (WW.Cad.Model.Entities.DxfEntity ent in thisBlock.Entities)
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", ent.Layer))
                    {
                        System.Console.WriteLine(ent.AcClass);
                        System.Console.WriteLine(ent.LineWeight);
                        System.Console.WriteLine(ent.EntityType);
                        System.Console.WriteLine(ent.LineType);
                        System.Console.WriteLine(ent.LineTypeScale);
                        System.Console.WriteLine(ent.Layer);
                    }
                } // Next ent

            } // Next thisBlock
            */


            foreach (WW.Cad.Model.Entities.DxfEntity ent in model.Entities)
            {

                if(System.StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", ent.Layer.Name))
                {
                    //System.Console.WriteLine(ent.AcClass);
                    //System.Console.WriteLine(ent.LineWeight);
                    //System.Console.WriteLine(ent.EntityType);
                    //System.Console.WriteLine(ent.LineType);
                    //System.Console.WriteLine(ent.LineTypeScale);
                    //System.Console.WriteLine(ent.Layer.Name);


					// WW.Cad.Model.Entities.DxfLwPolyline


                    // http://www.woutware.com/doc/cadlib3.5/html/3a2347ab-838e-26ca-5aed-889ec5f96526.htm
                    WW.Cad.Model.Entities.DxfPolyline2D dp = (WW.Cad.Model.Entities.DxfPolyline2D)ent;

                    //ent.LineTypeScale = -1;
                    // System.Console.WriteLine(dp.LineWeight);
                    dp.DefaultStartWidth = 0.0;
                    dp.DefaultEndWidth = 0.0;
                    // dp.LineWeight = (short)0.0;

                    // dp.LineType = WW.Cad.Model


                    foreach (WW.Cad.Model.Entities.DxfVertex2D thisVertex in dp.Vertices)
                    {
                        thisVertex.StartWidth = 0.0;
                        thisVertex.EndWidth = 0.0;
                        // thisVertex.LineTypeScale = -1;
                        // thisVertex.LineWeight = (short)0.0;
                    } // Next thisVertex
                    
                    // System.Console.WriteLine(dp.DefaultStartWidth);
                    // System.Console.WriteLine(dp.DefaultEndWidth);

                } // End if(StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", ent.Layer.Name))

            } // Next ent




            // System.Drawing.Printing.PaperSize ps = new System.Drawing.Printing.PaperSize();





            ExportPdf.WriteDefaultLayoutToPdf(model, 0.393701f, @"d:\testme.pdf", true, 10);


			string ExportName = null;

			if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
				ExportName = System.IO.Path.Combine ("/root", System.IO.Path.GetFileNameWithoutExtension (filename) + ".svg");
			else if (StorageHelper.DriveExists (@"D:\"))
				ExportName = System.IO.Path.Combine (@"D:\", System.IO.Path.GetFileNameWithoutExtension (filename) + ".svg");
            else
				ExportName = StorageHelper.GetDesktopPath (System.IO.Path.GetFileNameWithoutExtension (filename) + ".svg");

			Export.ExportToSvg(model, ExportName);
			Modifier.ModifySVG(ExportName);


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


        // SaveAImage(model, @"test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        public static void SaveAImage(DxfModel model, string path, System.Drawing.Imaging.ImageFormat fmt)
        {
            // Convert the CAD drawing to a bitmap.
            System.Drawing.Bitmap bitmap = ImageExporter.CreateAutoSizedBitmap(
                model,
                Matrix4D.Identity,
                GraphicsConfig.WhiteBackgroundCorrectForBackColor,
                System.Drawing.Drawing2D.SmoothingMode.HighQuality,
                new System.Drawing.Size(500, 400)
            );

            // Send the bitmap to the client.
            // context.Response.ContentType = "image/jpeg";
            // bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            bitmap.Save(path, fmt);
        } // End Sub SaveAImage 


    } // End Class Program 


} // End Namespace DwgToSvgConverter 
