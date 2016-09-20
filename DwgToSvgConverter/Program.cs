
#define COMPENSATE_TEIGHA_BUG  


using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Drawing;

using WW.Math;


namespace DwgToSvgConverter
{

    
    static class Program
    {


        public static void FixDoubleLines(WW.Cad.Model.DrawingCodePage drawingCodePage, WW.Cad.Model.Entities.DxfEntity entity)
        {
            //if (System.StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", model.Entities[i].Layer.Name))
            //{
            //    //System.Console.WriteLine(model.Entities[i].AcClass);
            //    //System.Console.WriteLine(model.Entities[i].LineWeight);
            //    //System.Console.WriteLine(model.Entities[i].EntityType);
            //    //System.Console.WriteLine(model.Entities[i].LineType);
            //    //System.Console.WriteLine(model.Entities[i].LineTypeScale);
            //    //System.Console.WriteLine(model.Entities[i].Layer.Name);
            //} //End if (System.StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", model.Entities[i].Layer.Name))

            
            if (entity is WW.Cad.Model.Entities.DxfInsert)
            {
                WW.Cad.Model.Entities.DxfInsert ins = (WW.Cad.Model.Entities.DxfInsert)entity;
                for(int i = 0; i < ins.Block.Entities.Count;++i)
                {
                    FixDoubleLines(drawingCodePage, ins.Block.Entities[i]);
                } // Next i 

            } // End if (entity is WW.Cad.Model.Entities.DxfInsert) 
            // https://www.woutware.com/Forum/Topic/1388/svg-export-path-goes-2x-around?returnUrl=%2FForum%2FBoard%2F2%2Fquestions-and-general-support&page=1
            else if (entity is WW.Cad.Model.Entities.DxfPolyline2D)
            {
                WW.Cad.Model.Entities.DxfPolyline2D dxfPolyline = (WW.Cad.Model.Entities.DxfPolyline2D)entity;
                dxfPolyline.DefaultStartWidth = 0.0;
                dxfPolyline.DefaultEndWidth = 0.0;

                foreach (WW.Cad.Model.Entities.DxfVertex2D thisVertex in dxfPolyline.Vertices)
                {
                    thisVertex.StartWidth = 0.0;
                    thisVertex.EndWidth = 0.0;
                } // Next thisVertex 
            }
            else if (entity is WW.Cad.Model.Entities.DxfLwPolyline)
            {
                WW.Cad.Model.Entities.DxfLwPolyline dwgPolyLine = (WW.Cad.Model.Entities.DxfLwPolyline)entity;

                // https://www.woutware.com/Forum/Topic/20/generating-thick-lines?returnUrl=%2FForum%2FUserPosts%3FuserId%3D743420688
                // set the property ConstantWidth. Don't confuse it with property Thickness though: the thickness is the extrusion along its z-axis
                dwgPolyLine.ConstantWidth = 0.0;
                dwgPolyLine.LineWeight = 0;
                dwgPolyLine.Thickness = 0.0;

                foreach (WW.Cad.Model.Entities.DxfLwPolyline.Vertex thisVertex in dwgPolyLine.Vertices)
                {
                    thisVertex.StartWidth = 0.0;
                    thisVertex.EndWidth = 0.0;
                } // Next tVertex 

            } // End else if (entity is WW.Cad.Model.Entities.DxfLwPolyline) 
#if  COMPENSATE_TEIGHA_BUG
            else if (entity is WW.Cad.Model.Entities.DxfText)
            {
                WW.Cad.Model.Entities.DxfText dxtext = (WW.Cad.Model.Entities.DxfText)entity;

                string corrected = null;
                if (FixTextWithWrongEncoding(drawingCodePage, dxtext.Text, ref corrected))
                {
                    dxtext.Text = corrected;
                } // End if (FixBrokenText(drawingCodePage, dxtext.Text, ref corrected)) 

            } // End else if (entity is WW.Cad.Model.Entities.DxfText) 
#endif


        } // End Sub FixDoubleLines 


        public static void FixModel(DxfModel model)
        {
#if COMPENSATE_TEIGHA_BUG
            FixLayerEncoding(model);
#endif
            

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
            

            //foreach (WW.Cad.Model.Tables.DxfBlock thisAnonymousBlock in model.AnonymousBlocks)
            //{
            //    for (int i = 0; i < thisAnonymousBlock.Entities.Count; ++i)
            //    {
            //        DoubleLineFix(thisAnonymousBlock.Entities[i]);
            //    }

            //} // Next thisBlock

            //foreach (WW.Cad.Model.Tables.DxfBlock thisBlock in model.Blocks)
            //{
            //    for (int i = 0; i < thisBlock.Entities.Count; ++i)
            //    {
            //        DoubleLineFix(thisBlock.Entities[i]);
            //    }
            //} // Next thisBlock

            // Use recursion instead of multiple loops 
            WW.Cad.Model.DrawingCodePage drawingCodePage = model.Header.DrawingCodePage;
            for (int i = 0; i < model.Entities.Count; ++i)
            {
                FixDoubleLines(drawingCodePage, model.Entities[i]);
            } // Next i 

        } // End Sub FixModel



        public static string HandleAllowedAccentCharacters(string stIn)
        {
            string retVal = null;
            // --- Begin German ---

            System.Text.StringBuilder sb = new System.Text.StringBuilder(stIn);

            sb = sb.Replace("ä", "ae");
            sb = sb.Replace("ö", "oe");
            sb = sb.Replace("ü", "ue");

            sb = sb.Replace("Ä", "Ae");
            sb = sb.Replace("Ö", "Oe");
            sb = sb.Replace("Ü", "Ue");

            sb = sb.Replace("ß", "ss");
            sb = sb.Replace("ß", "SS");
            
            // --- End German ---


            // --- Begin French ---

            sb = sb.Replace("à", "a");
            sb = sb.Replace("á", "a");
            sb = sb.Replace("â", "a");

            sb = sb.Replace("ç", "c");

            sb = sb.Replace("é", "e");
            sb = sb.Replace("è", "e");
            sb = sb.Replace("ë", "e");
            sb = sb.Replace("ê", "e");

            sb = sb.Replace("ï", "i");
            sb = sb.Replace("î", "i");

            sb = sb.Replace("ò", "o");
            sb = sb.Replace("ó", "o");
            sb = sb.Replace("ô", "o");

            sb = sb.Replace("ú", "u");
            sb = sb.Replace("ù", "u");
            sb = sb.Replace("û", "u");
            
            sb = sb.Replace("æ", "ae");



            sb = sb.Replace("À", "A");
            sb = sb.Replace("Á", "A");
            sb = sb.Replace("Â", "A");

            sb = sb.Replace("Ç", "C");

            sb = sb.Replace("É", "E");
            sb = sb.Replace("È", "E");
            sb = sb.Replace("Ê", "E");
            sb = sb.Replace("Ë", "E");

            sb = sb.Replace("Ï", "I");
            sb = sb.Replace("Î", "I");

            sb = sb.Replace("Ò", "O");
            sb = sb.Replace("Ó", "O");
            sb = sb.Replace("Ô", "O");

            sb = sb.Replace("Û", "U");
            sb = sb.Replace("Ú", "U");
            sb = sb.Replace("Ù", "U");

            sb = sb.Replace("Æ", "AE");

            // --- End French ---


            // --- Begin Spanish ---
            sb = sb.Replace("ñ", "n");
            sb = sb.Replace("õ", "o");
            sb = sb.Replace("í", "i");
            sb = sb.Replace("ì", "i");

            sb = sb.Replace("Ñ", "N");
            sb = sb.Replace("Õ", "O");
            sb = sb.Replace("Í", "I");
            sb = sb.Replace("Ì", "I");


            sb = sb.Replace("¿", "?");
            sb = sb.Replace("¡", "!");
            // --- End Spanish ---


            
            // --- Begin Swedish/Norwegian/Danish ---
            sb = sb.Replace("å", "a");
            sb = sb.Replace("ø", "o");

            sb = sb.Replace("Å", "A");
            sb = sb.Replace("Ø", "O");
            // --- End Swedish/Norwegian/Danish ---

            retVal = sb.ToString();
            sb.Clear();
            sb = null;

            return retVal;
        } // End Function HandleAllowedAccentCharacters 


        // string str = ApertureSucks.Latinize("(æøå âôû?aè");
        public static string Latinize(string stIn)
        {
            // Special treatment for German + French accents
            stIn = HandleAllowedAccentCharacters(stIn);

            string stFormD = stIn.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);

                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                } // End if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)

            } // Next ich


            //return (sb.ToString().Normalize(System.Text.NormalizationForm.FormC));
            return (sb.ToString().Normalize(System.Text.NormalizationForm.FormKC));
        } // End Function Latinize


        public static bool FixTextWithWrongEncoding(WW.Cad.Model.DrawingCodePage drawingCodePage, string text, ref string correctedText)
        {
            string normalizedText = HandleAllowedAccentCharacters(text);
            string latinText = Latinize(text);

            if (!System.StringComparer.OrdinalIgnoreCase.Equals(latinText, normalizedText))
            {
                System.Console.WriteLine("Invalid characters in text '" + text + "'");
                System.Console.WriteLine("  - Latin: " + latinText);
                byte[] encoded = System.Text.Encoding.GetEncoding((int)drawingCodePage).GetBytes(text);
                correctedText = System.Text.Encoding.GetEncoding("iso-8859-1").GetString(encoded);
                System.Console.WriteLine("  - Actual: " + correctedText);
                System.Console.WriteLine("fixed");
                return true;
            } // End if (!System.StringComparer.OrdinalIgnoreCase.Equals(latin, layerName)) 
            // else System.Console.WriteLine(text);
            return false;
        } // End Function FixTextWithWrongEncoding 


        public static void FixLayerEncoding(DxfModel model)
        {
            foreach (WW.Cad.Model.Tables.DxfLayer thisLayer in model.Layers)
            {
                string corrected = null;
                if (FixTextWithWrongEncoding(model.Header.DrawingCodePage, thisLayer.Name, ref corrected))
                {
                    thisLayer.Name = corrected;
                } // End if (FixTextWithWrongEncoding(model.Header.DrawingCodePage, thisLayer.Name, ref corrected)) 

            } // Next thisLayer 

        } // End Sub FixLayerEncoding 


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
