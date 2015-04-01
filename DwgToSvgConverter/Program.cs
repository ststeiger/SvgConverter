using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;



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
        [STAThread]
        static void Main()
        {
#if false
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            return;
#endif

            string filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // filename = System.IO.Path.Combine(filename, "..", "..", "Zimmertyp_1_.dwg");
            //filename = System.IO.Path.Combine(filename, "..", "..", "..", "SvgConverter", "drawing.dxf");
            //filename = System.IO.Path.Combine(filename, "..", "..", "..", "ApertureService", "0001_GB01_OG14_0000_Aperture.dxf");
            filename = System.IO.Path.Combine(filename, "..", "..", "0001_GB01_OG14_0000_Aperture_dxf13.dxf");
            

            filename = System.IO.Path.GetFullPath(filename);
            // filename = string.Format( @"D:\{0}\Downloads\7602_GB01_OG01_0000_Aperture.dxf", Environment.UserName);

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
            foreach (WW.Cad.Model.Objects.DxfGroup x in model.Groups)
            {
                
            }
            



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
                }
            }

            
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
                }

            }
            */


            foreach (WW.Cad.Model.Entities.DxfEntity ent in model.Entities)
            {

                if(StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", ent.Layer.Name))
                {
                    
                    System.Console.WriteLine(ent.AcClass);
                    System.Console.WriteLine(ent.LineWeight);
                    System.Console.WriteLine(ent.EntityType);
                    System.Console.WriteLine(ent.LineType);
                    System.Console.WriteLine(ent.LineTypeScale);
                    System.Console.WriteLine(ent.Layer.Name);

                    // http://www.woutware.com/doc/cadlib3.5/html/3a2347ab-838e-26ca-5aed-889ec5f96526.htm
                    WW.Cad.Model.Entities.DxfPolyline2D dp = (WW.Cad.Model.Entities.DxfPolyline2D)ent;
                    System.Console.WriteLine(dp.LineWeight);

                }

            }


            

			if(System.Environment.OSVersion.Platform == PlatformID.Unix)
                Export.ExportToSvg(model, System.IO.Path.Combine("/root", System.IO.Path.GetFileNameWithoutExtension(filename) + ".svg"));
            else if (DriveExists(@"D:\"))
                Export.ExportToSvg(model, System.IO.Path.Combine(@"D:\", System.IO.Path.GetFileNameWithoutExtension(filename) + ".svg"));
            //Export.ExportToSvg(model, @"d:\mytest.svg");
            else
                Export.ExportToSvg(model, GetDesktopPath(System.IO.Path.GetFileNameWithoutExtension(filename) + ".svg"));

            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }



        public static string GetDesktopPath(string filename)
        {
            string desk = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            return System.IO.Path.Combine(desk, filename);
        }


        public static bool DriveExists(string name)
        {
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

            // System.IO.DriveInfo di = new System.IO.DriveInfo("");
            
            foreach (System.IO.DriveInfo di in drives)
            {
                // System.Console.WriteLine(di.Name);
                if (StringComparer.InvariantCultureIgnoreCase.Equals(name, di.Name))
                    return true;
            }

            return false;
        }



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

        }



    }
}
