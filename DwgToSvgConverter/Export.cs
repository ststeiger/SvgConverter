
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
            
            /*
            using (System.IO.Stream stream = System.IO.File.Create(path))
            {
                WW.Cad.IO.SvgExporter exporter = new WW.Cad.IO.SvgExporter(stream, paperSize);

                exporter.Title = "tempSvg";
                exporter.ExportCadLayersAsSvgGroups = true;
                exporter.WriteSvgXmlElementAttributes += AdditionalAttribute;
                // exporter.WriteSvgXmlElementAttributes += OverwriteStrokeWidth;
                exporter.WriteBackgroundRectangle = true;
                

                WW.Cad.Drawing.GraphicsConfig gc = new WW.Cad.Drawing.GraphicsConfig();
                gc.FixedForegroundColor = WW.Drawing.ArgbColor.FromArgb(0, WW.Drawing.ArgbColor.FromRgb(0, 0, 0)); // stroke
                // gc.FixedForegroundColor = WW.Drawing.ArgbColors.HotPink; // Stroke
                gc.CorrectColorForBackgroundColor = false;
                
                gc.NodeColor = WW.Drawing.ArgbColors.HotPink;
                // gc.BackColor = WW.Drawing.ArgbColors.HotPink;
                
                gc.ShowDimensionDefinitionPoints = false;
                gc.BackColor = WW.Drawing.ArgbColor.FromArgb(0, WW.Drawing.ArgbColors.White);

                // exporter.Draw(model, WW.Cad.Drawing.GraphicsConfig.WhiteBackgroundCorrectForBackColor, to2DTransform);
                exporter.Draw(model, gc, to2DTransform);
            } // End using stream 
            */


            
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                WW.Cad.IO.SvgExporter exporter = new WW.Cad.IO.SvgExporter(stream, paperSize);

                exporter.Title = "tempSvg";
                exporter.ExportCadLayersAsSvgGroups = true;
                exporter.WriteSvgXmlElementAttributes += AdditionalAttribute;
                // exporter.WriteSvgXmlElementAttributes += OverwriteStrokeWidth;
                exporter.WriteBackgroundRectangle = true;
                

                WW.Cad.Drawing.GraphicsConfig gc = new WW.Cad.Drawing.GraphicsConfig();
                gc.FixedForegroundColor = WW.Drawing.ArgbColor.FromArgb(0, WW.Drawing.ArgbColor.FromRgb(0, 0, 0)); // stroke
                // gc.FixedForegroundColor = WW.Drawing.ArgbColors.HotPink; // Stroke
                gc.CorrectColorForBackgroundColor = false;
                gc.TryDrawingTextAsText = true;
                
                gc.NodeColor = WW.Drawing.ArgbColors.HotPink;
                // gc.BackColor = WW.Drawing.ArgbColors.HotPink;
                
                gc.ShowDimensionDefinitionPoints = false;
                gc.BackColor = WW.Drawing.ArgbColor.FromArgb(0, WW.Drawing.ArgbColors.White);

                // exporter.Draw(model, WW.Cad.Drawing.GraphicsConfig.WhiteBackgroundCorrectForBackColor, to2DTransform);
                exporter.Draw(model, gc, to2DTransform);

                byte[] ba  = stream.ToArray();
                string str = System.Text.Encoding.UTF8.GetString(ba);
                // string str = System.Text.Encoding.UTF8.GetString(ba);
                // string str = System.Text.Encoding.Unicode.GetString(ba);
                // string str = System.Text.Encoding.Default.GetString(ba);
                // string str = System.Text.Encoding.ASCII.GetString(ba);
                // string str = System.Text.Encoding.UTF7.GetString(ba);
                // string str = System.Text.Encoding.UTF32.GetString(ba);
                // string str = System.Text.Encoding.GetEncoding("iso-8859-1").GetString(ba);
                // string str = System.Text.Encoding.GetEncoding("iso-8859-2").GetString(ba);
                // string str = System.Text.Encoding.GetEncoding(850).GetString(ba);


                // System.Console.WriteLine(str);
                // This is "not scalable for many different encodings"...
                byte[] encoded = System.Text.Encoding.GetEncoding((int)model.Header.DrawingCodePage).GetBytes(str);
                string corrected = System.Text.Encoding.GetEncoding("iso-8859-1").GetString(encoded);


                // System.Console.WriteLine(corrected);
                System.IO.File.WriteAllText(@"d:\testfile_OG14.svg", corrected, System.Text.Encoding.UTF8);
                System.Console.WriteLine("Finished");

                //ba = System.Text.Encoding.UTF8.GetBytes(str);
            } // End using stream 

        } // End Sub ExportToSvg


        // For better visibility when testing.
        public static void OverwriteStrokeWidth(System.Xml.XmlTextWriter w, WW.Cad.Model.Entities.DxfEntity entity)
        {
            w.WriteAttributeString("stroke-width", "5");
        } // End Sub OverwriteStrokeWidth


        public static void AdditionalAttribute(System.Xml.XmlTextWriter w, WW.Cad.Model.Entities.DxfEntity entity)
        {
            /*
            if (System.StringComparer.OrdinalIgnoreCase.Equals("FM_OBJEKT_RAUM", entity.Layer.Name))
                System.Console.WriteLine(entity.Layer);


            WW.Cad.Model.DxfHandledObject abc = (WW.Cad.Model.DxfHandledObject)entity;
            if (abc.ExtendedDataCollection.Count > 0)
            {
                System.Console.WriteLine(abc.ExtendedDataCollection);
                foreach (WW.Cad.Model.DxfExtendedData data in abc.ExtendedDataCollection)
                {
                    
                    

                    foreach (WW.Cad.Model.IExtendedDataValue x in data.Values)
                    {
                        System.Type t = x.GetType();

                        // https://www.woutware.com/doc/cadlib3.5/html/8afa4797-7737-1192-ac52-da39a3c02d4e.htm
                        // https://www.woutware.com/Forum/Topic/1039/retrieving-attributes?returnUrl=%2FForum%2FUserPosts%3FuserId%3D1963963672
                        if (object.ReferenceEquals(t, typeof(WW.Cad.Model.DxfExtendedData.String)))
                        {
                            WW.Cad.Model.DxfExtendedData.String xx = (WW.Cad.Model.DxfExtendedData.String)x;
                            System.Console.WriteLine(xx);
                        }
                        else if (object.ReferenceEquals(t, typeof(WW.Cad.Model.DxfExtendedData.Int16)))
                        {
                            WW.Cad.Model.DxfExtendedData.Int16 xx = (WW.Cad.Model.DxfExtendedData.Int16)x;
                            System.Console.WriteLine(xx);
                        }
                        else if (object.ReferenceEquals(t, typeof(WW.Cad.Model.DxfExtendedData.Int32)))
                        {
                            WW.Cad.Model.DxfExtendedData.Int32 xx = (WW.Cad.Model.DxfExtendedData.Int32)x;
                            System.Console.WriteLine(xx);
                        }
                        else if (object.ReferenceEquals(t, typeof(WW.Cad.Model.DxfExtendedData.DatabaseHandle)))
                        {
                            if (x != null)
                            { 
                                WW.Cad.Model.DxfExtendedData.DatabaseHandle xx = (WW.Cad.Model.DxfExtendedData.DatabaseHandle)x;
                                if(xx != null)
                                    System.Console.WriteLine(xx);
                            }
                        }
                        else
                        {
                            System.Console.WriteLine(t);
                        }

                        
                    }


                    
                }
            }
                */

            // if (abc.ExtensionDictionary != null && abc.ExtensionDictionary.Entries.Count > 0)
            //     System.Console.WriteLine(abc.ExtensionDictionary.Entries);


            //// https://www.woutware.com/Forum/Topic/1039/retrieving-attributes?returnUrl=%2FForum%2FUserPosts%3FuserId%3D1963963672
            //if (entity.AcClass == "AcDbMInsertBlock")
            //{
            //    WW.Cad.Model.Entities.DxfInsert blockref = (WW.Cad.Model.Entities.DxfInsert)entity;
            //    System.Console.WriteLine(blockref.Block.Name);

            //    foreach (WW.Cad.Model.Entities.DxfAttribute attr in blockref.Attributes)
            //    {
            //        System.Console.WriteLine("\t" + attr.TagString);
            //        System.Console.WriteLine("\t\t" + attr.Text);
            //    } // Next attr 
            //} // End if (entity.AcClass == "AcDbMInsertBlock") 

            w.WriteAttributeString("data-handle", entity.Handle.ToString(System.Globalization.CultureInfo.InvariantCulture));
        } // End Sub AdditionalAttribute 


    } // End Class Export 


} // End Namespace DwgToSvgConverter 
