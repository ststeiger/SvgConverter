
namespace DxfLayerSeparation
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif

            string filename = @"F:\COR\DMS\POST_0020_GE01_OG01_0000 - Kopie.dxf";

            MessageBoxHandler.CloseNextMessageBoxByTitle("Wout Ware trial"); // Annoying
            WW.Cad.Model.DxfModel model = null;
            model = WW.Cad.IO.DxfReader.Read(filename);



            // System.Console.WriteLine(model.UnsupportedObjects.Count);


            for (int i = model.Layers.Count-1; i >= 0; --i)
            {
                System.Console.WriteLine(model.Layers[i].Name);

                // if (string.Equals(model.Layers[i].Name, "0", System.StringComparison.InvariantCultureIgnoreCase)) { continue; }

                // if (string.Equals(model.Layers[i].Name, "Grundriss", System.StringComparison.InvariantCultureIgnoreCase))
                if (string.Equals(model.Layers[i].Name, "Raumpolygon", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    model.Layers[i].Enabled = true;
                    continue;
                }


                model.Layers[i].Enabled = false;
                // continue;


                for (int j = model.Entities.Count - 1; j >= 0; --j)
                {

                    if (model.Entities[j].Layer == model.Layers[i])
                    {
                        // System.Console.WriteLine(model.Entities[j].Layer.Name);
                        // System.Console.WriteLine(model.Layers[i].Name);
                        model.Entities.RemoveAt(j);
                    } // End if (model.Entities[j].Layer == model.Layers[i])

                } // Next j 

                model.Layers.RemoveAt(i);
            } // Next i

            System.Collections.Generic.List<WW.Cad.Base.DxfMessage> ls = new System.Collections.Generic.List<WW.Cad.Base.DxfMessage>();
            model.Repair(ls);

            // System.Console.WriteLine(model.Layers[1].Name);
            // WW.Cad.Model.Tables.DxfLayer layer = model.Layers[1];
            // layer.Enabled = false;

            // WW.Cad.Example.LayerExtractorVisitor layerExtractorVisitor = new WW.Cad.Example.LayerExtractorVisitor();
            // layerExtractorVisitor.Run(model, layer);


            using (System.IO.FileStream fs = new System.IO.FileStream("oneLayer.dxf", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                WW.Cad.IO.DxfWriter dxfwriter = new WW.Cad.IO.DxfWriter(fs, model, false);
                dxfwriter.Write();
                // fs.Flush();
                // fs.Close();
            } // End Using fs 


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main


    } // End Class Program


} // End Namespace DxfLayerSeparation
