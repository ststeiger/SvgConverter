
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Drawing;

using WW.Math;


namespace DwgToSvgConverter
{

    
    static class Program
    {


        private static void DoubleLineFix(WW.Cad.Model.Entities.DxfEntity entity)
        {
            //if (System.StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", model.Entities[i].Layer.Name))
            //{
            //    //System.Console.WriteLine(model.Entities[i].AcClass);
            //    //System.Console.WriteLine(model.Entities[i].LineWeight);
            //    //System.Console.WriteLine(model.Entities[i].EntityType);
            //    //System.Console.WriteLine(model.Entities[i].LineType);
            //    //System.Console.WriteLine(model.Entities[i].LineTypeScale);
            //    //System.Console.WriteLine(model.Entities[i].Layer.Name);
            //}

            // https://www.woutware.com/Forum/Topic/1388/svg-export-path-goes-2x-around?returnUrl=%2FForum%2FBoard%2F2%2Fquestions-and-general-support&page=1
            if (entity is WW.Cad.Model.Entities.DxfPolyline2D)
            {
                WW.Cad.Model.Entities.DxfPolyline2D dxfPolyline = (WW.Cad.Model.Entities.DxfPolyline2D)entity;
                dxfPolyline.DefaultStartWidth = 0.0;
                dxfPolyline.DefaultEndWidth = 0.0;

                foreach (WW.Cad.Model.Entities.DxfVertex2D tVertex in dxfPolyline.Vertices)
                {
                    tVertex.StartWidth = 0.0;
                    tVertex.EndWidth = 0.0;
                }
            }
            else if (entity is WW.Cad.Model.Entities.DxfLwPolyline)
            {
                WW.Cad.Model.Entities.DxfLwPolyline dwgPolyLine = (WW.Cad.Model.Entities.DxfLwPolyline)entity;

                // https://www.woutware.com/Forum/Topic/20/generating-thick-lines?returnUrl=%2FForum%2FUserPosts%3FuserId%3D743420688
                // set the property ConstantWidth. Don't confuse it with property Thickness though: the thickness is the extrusion along its z-axis
                dwgPolyLine.ConstantWidth = 0.0;
                dwgPolyLine.LineWeight = 0;
                dwgPolyLine.Thickness = 0.0;

                foreach (WW.Cad.Model.Entities.DxfLwPolyline.Vertex tVertex in dwgPolyLine.Vertices)
                {
                    tVertex.StartWidth = 0.0;
                    tVertex.EndWidth = 0.0;
                }
            }
        }


        public static void FixModel(DxfModel model)
        {


            //foreach (WW.Cad.Model.Objects.DxfGroup thisGroup in model.Groups)
            //{

            //} // Next thisGroup 


            //foreach (WW.Cad.Model.Entities.DxfEntity ent in model.ModelLayout.Entities)
            //{
            //    if (!ls.Contains(ent.Layer.Name))
            //        ls.Add(ent.Layer.Name);
            //} // Next ent 


            //for (int i = 0; i < model.Layouts.Count; ++i)
            //{

            //    foreach (WW.Cad.Model.Entities.DxfEntity ent in model.Layouts[i].Entities)
            //    {
            //        if (!ls.Contains(ent.Layer.Name))
            //            ls.Add(ent.Layer.Name);
            //    }

            //} // Next i 
            

            foreach (WW.Cad.Model.Tables.DxfBlock thisAnonymousBlock in model.AnonymousBlocks)
            {
                for (int i = 0; i < thisAnonymousBlock.Entities.Count; ++i)
                {
                    DoubleLineFix(thisAnonymousBlock.Entities[i]);
                }

            } // Next thisBlock

            foreach (WW.Cad.Model.Tables.DxfBlock thisBlock in model.Blocks)
            {
                for (int i = 0; i < thisBlock.Entities.Count; ++i)
                {
                    DoubleLineFix(thisBlock.Entities[i]);
                }
            } // Next thisBlock

            for (int i = 0; i < model.Entities.Count; ++i)
            {
                DoubleLineFix(model.Entities[i]);
            }

        }



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
            // filename = System.IO.Path.Combine(filename, "..", "..", "..", "SvgConverter", "drawing.dxf");
            // filename = System.IO.Path.Combine(filename, "..", "..", "..", "ApertureService", "0001_GB01_OG14_0000_Aperture.dxf");
            // filename = System.IO.Path.Combine(filename, "..", "..", "0001_GB01_OG14_0000_Aperture_dxf13.dxf");
            // filename = @"D:\Temp\Test\6260_GB01_OG02.dwg";
            // filename = @"D:\Temp\Test\0001_GB01_OG14_0000.dxf";
            filename = @"D:\Temp\Test\0001_GB01_OG14_0000.dwg";

            filename = System.IO.Path.GetFullPath(filename);


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


            FixModel(model);

            // ExportPdf.WriteDefaultLayoutToPdf(model, 0.393701f, @"d:\testme.pdf", true, 10);


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
