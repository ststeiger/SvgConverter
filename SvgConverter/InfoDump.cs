
using netDxf;
using netDxf.Entities;
using netDxf.Header;

// using netDxf.Blocks;
// using netDxf.Collections;

// using netDxf.Objects;
// using netDxf.Tables;

// using Group = netDxf.Objects.Group;
// using Point = netDxf.Entities.Point;
// using Attribute = netDxf.Entities.Attribute;
// using Image = netDxf.Entities.Image;


namespace SvgConverter
{
	public class InfoDump
	{
		public InfoDump()
		{
		}

		public static void DumpFile(string file)
		{
			DumpFile (file, null);
		}


		public static void DumpFile(string file, string output)
		{
			// check if the dxf actually exists
			System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);

			// optionally you can save the information to a text file
			bool outputLog = !string.IsNullOrEmpty(output);
			System.IO.TextWriter writer = null;
			if (outputLog)
			{
				writer = new System.IO.StreamWriter(System.IO.File.Create(output));
				System.Console.SetOut(writer);
			}

			if (!fileInfo.Exists)
			{
				System.Console.WriteLine("THE FILE {0} DOES NOT EXIST", file);
				System.Console.WriteLine();

				System.Console.WriteLine("Press a key to continue...");
				System.Console.ReadLine();

				return;
			}

			bool isBinary;
			DxfVersion dxfVersion = netDxf.DxfDocument.CheckDxfFileVersion(file, out isBinary);

			// check if the file is a dxf
			if (dxfVersion == DxfVersion.Unknown)
			{
				System.Console.WriteLine("THE FILE {0} IS NOT A VALID DXF OR THE DXF DOES NOT INCLUDE VERSION INFORMATION IN THE HEADER SECTION", file);
				System.Console.WriteLine();

				System.Console.WriteLine("Press a key to continue...");
				System.Console.ReadLine();

				return;
			}

			// check if the dxf file version is supported
			if (dxfVersion < DxfVersion.AutoCad2000)
			{
				System.Console.WriteLine("THE FILE {0} IS NOT A SUPPORTED DXF", file);
				System.Console.WriteLine();

				System.Console.WriteLine("FILE VERSION: {0}", dxfVersion);
				System.Console.WriteLine();

				System.Console.WriteLine("Press a key to continue...");
				System.Console.ReadLine();

				return;
			}

			DxfDocument dxf = DxfDocument.Load(file);

			// check if there has been any problems loading the file,
			// this might be the case of a corrupt file or a problem in the library
			if (dxf == null)
			{
				System.Console.WriteLine("ERROR LOADING {0}", file);
				System.Console.WriteLine();

				System.Console.WriteLine("Press a key to continue...");
				System.Console.ReadLine();

				return;
			}

			// the dxf has been properly loaded, let's show some information about it
			System.Console.WriteLine("FILE NAME: {0}", file);
			System.Console.WriteLine("\tbinary dxf: {0}", isBinary);
			System.Console.WriteLine();            
			System.Console.WriteLine("FILE VERSION: {0}", dxf.DrawingVariables.AcadVer);
			System.Console.WriteLine();
			System.Console.WriteLine("FILE COMMENTS: {0}", dxf.Comments.Count);
			foreach (var o in dxf.Comments)
			{
				System.Console.WriteLine("\t{0}", o);
			}
			System.Console.WriteLine();
			System.Console.WriteLine("FILE TIME:");
			System.Console.WriteLine("\tdrawing created (UTC): {0}.{1}", dxf.DrawingVariables.TduCreate, dxf.DrawingVariables.TduCreate.Millisecond.ToString("000"));
			System.Console.WriteLine("\tdrawing last update (UTC): {0}.{1}", dxf.DrawingVariables.TduUpdate, dxf.DrawingVariables.TduUpdate.Millisecond.ToString("000"));
			System.Console.WriteLine("\tdrawing edition time: {0}", dxf.DrawingVariables.TdinDwg);
			System.Console.WriteLine();    
			System.Console.WriteLine("APPLICATION REGISTRIES: {0}", dxf.ApplicationRegistries.Count);

			foreach (var o in dxf.ApplicationRegistries)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.ApplicationRegistries.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("LAYERS: {0}", dxf.Layers.Count);
			foreach (var o in dxf.Layers)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Layers.GetReferences(o).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("LINE TYPES: {0}", dxf.LineTypes.Count);
			foreach (var o in dxf.LineTypes)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.LineTypes.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("TEXT STYLES: {0}", dxf.TextStyles.Count);
			foreach (var o in dxf.TextStyles)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.TextStyles.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("DIMENSION STYLES: {0}", dxf.DimensionStyles.Count);
			foreach (var o in dxf.DimensionStyles)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.DimensionStyles.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("MLINE STYLES: {0}", dxf.MlineStyles.Count);
			foreach (var o in dxf.MlineStyles)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.MlineStyles.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("UCSs: {0}", dxf.UCSs.Count);
			foreach (var o in dxf.UCSs)
			{
				System.Console.WriteLine("\t{0}", o.Name);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("BLOCKS: {0}", dxf.Blocks.Count);
			foreach (var o in dxf.Blocks)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Blocks.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("LAYOUTS: {0}", dxf.Layouts.Count);
			foreach (var o in dxf.Layouts)
			{
				System.Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Layouts.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("IMAGE DEFINITIONS: {0}", dxf.ImageDefinitions.Count);
			foreach (var o in dxf.ImageDefinitions)
			{
				System.Console.WriteLine("\t{0}; File name: {1}; References count: {2}", o.Name, o.FileName, dxf.ImageDefinitions.GetReferences(o.Name).Count);
			}
			System.Console.WriteLine();

			System.Console.WriteLine("GROUPS: {0}", dxf.Groups.Count);
			foreach (var o in dxf.Groups)
			{
				System.Console.WriteLine("\t{0}; Entities count: {1}", o.Name, o.Entities.Count);
			}
			System.Console.WriteLine();

			// the entities lists contain the geometry that has a graphical representation in the drawing across all layouts,
			// to get the entities that belongs to an specific layout you can get the references through the Layouts.GetReferences(name)
			// or check the EntityObject.Owner.Record.Layout property
			System.Console.WriteLine("ENTITIES:");
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Arc, dxf.Arcs.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.AttributeDefinition, dxf.AttributeDefinitions.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Circle, dxf.Circles.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Dimension, dxf.Dimensions.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Ellipse, dxf.Ellipses.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Face3D, dxf.Faces3d.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Hatch, dxf.Hatches.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Image, dxf.Images.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Insert, dxf.Inserts.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.LightWeightPolyline, dxf.LwPolylines.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Line, dxf.Lines.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Mesh, dxf.Meshes.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.MLine, dxf.MLines.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.MText, dxf.MTexts.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Point, dxf.Points.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.PolyfaceMesh, dxf.PolyfaceMeshes.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Polyline, dxf.Polylines.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Solid, dxf.Solids.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Spline, dxf.Splines.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Text, dxf.Texts.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Ray, dxf.Rays.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.Viewport, dxf.Viewports.Count);
			System.Console.WriteLine("\t{0}; count: {1}", EntityType.XLine, dxf.XLines.Count);
			System.Console.WriteLine();
		}


	}
}

