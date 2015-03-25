
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using netDxf;
using netDxf.Blocks;
using netDxf.Collections;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Objects;
using netDxf.Tables;

using Group = netDxf.Objects.Group;
using Point = netDxf.Entities.Point;
using Attribute = netDxf.Entities.Attribute;
using Image = netDxf.Entities.Image;


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
			FileInfo fileInfo = new FileInfo(file);

			// optionally you can save the information to a text file
			bool outputLog = !String.IsNullOrEmpty(output);
			TextWriter writer = null;
			if (outputLog)
			{
				writer = new StreamWriter(File.Create(output));
				Console.SetOut(writer);
			}

			if (!fileInfo.Exists)
			{
				Console.WriteLine("THE FILE {0} DOES NOT EXIST", file);
				Console.WriteLine();

				Console.WriteLine("Press a key to continue...");
				Console.ReadLine();

				return;
			}

			bool isBinary;
			DxfVersion dxfVersion = DxfDocument.CheckDxfFileVersion(file, out isBinary);

			// check if the file is a dxf
			if (dxfVersion == DxfVersion.Unknown)
			{
				Console.WriteLine("THE FILE {0} IS NOT A VALID DXF OR THE DXF DOES NOT INCLUDE VERSION INFORMATION IN THE HEADER SECTION", file);
				Console.WriteLine();

				Console.WriteLine("Press a key to continue...");
				Console.ReadLine();

				return;
			}

			// check if the dxf file version is supported
			if (dxfVersion < DxfVersion.AutoCad2000)
			{
				Console.WriteLine("THE FILE {0} IS NOT A SUPPORTED DXF", file);
				Console.WriteLine();

				Console.WriteLine("FILE VERSION: {0}", dxfVersion);
				Console.WriteLine();

				Console.WriteLine("Press a key to continue...");
				Console.ReadLine();

				return;
			}

			DxfDocument dxf = DxfDocument.Load(file);

			// check if there has been any problems loading the file,
			// this might be the case of a corrupt file or a problem in the library
			if (dxf == null)
			{
				Console.WriteLine("ERROR LOADING {0}", file);
				Console.WriteLine();

				Console.WriteLine("Press a key to continue...");
				Console.ReadLine();

				return;
			}

			// the dxf has been properly loaded, let's show some information about it
			Console.WriteLine("FILE NAME: {0}", file);
			Console.WriteLine("\tbinary dxf: {0}", isBinary);
			Console.WriteLine();            
			Console.WriteLine("FILE VERSION: {0}", dxf.DrawingVariables.AcadVer);
			Console.WriteLine();
			Console.WriteLine("FILE COMMENTS: {0}", dxf.Comments.Count);
			foreach (var o in dxf.Comments)
			{
				Console.WriteLine("\t{0}", o);
			}
			Console.WriteLine();
			Console.WriteLine("FILE TIME:");
			Console.WriteLine("\tdrawing created (UTC): {0}.{1}", dxf.DrawingVariables.TduCreate, dxf.DrawingVariables.TduCreate.Millisecond.ToString("000"));
			Console.WriteLine("\tdrawing last update (UTC): {0}.{1}", dxf.DrawingVariables.TduUpdate, dxf.DrawingVariables.TduUpdate.Millisecond.ToString("000"));
			Console.WriteLine("\tdrawing edition time: {0}", dxf.DrawingVariables.TdinDwg);
			Console.WriteLine();    
			Console.WriteLine("APPLICATION REGISTRIES: {0}", dxf.ApplicationRegistries.Count);
			foreach (var o in dxf.ApplicationRegistries)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.ApplicationRegistries.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("LAYERS: {0}", dxf.Layers.Count);
			foreach (var o in dxf.Layers)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Layers.GetReferences(o).Count);
			}
			Console.WriteLine();

			Console.WriteLine("LINE TYPES: {0}", dxf.LineTypes.Count);
			foreach (var o in dxf.LineTypes)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.LineTypes.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("TEXT STYLES: {0}", dxf.TextStyles.Count);
			foreach (var o in dxf.TextStyles)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.TextStyles.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("DIMENSION STYLES: {0}", dxf.DimensionStyles.Count);
			foreach (var o in dxf.DimensionStyles)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.DimensionStyles.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("MLINE STYLES: {0}", dxf.MlineStyles.Count);
			foreach (var o in dxf.MlineStyles)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.MlineStyles.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("UCSs: {0}", dxf.UCSs.Count);
			foreach (var o in dxf.UCSs)
			{
				Console.WriteLine("\t{0}", o.Name);
			}
			Console.WriteLine();

			Console.WriteLine("BLOCKS: {0}", dxf.Blocks.Count);
			foreach (var o in dxf.Blocks)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Blocks.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("LAYOUTS: {0}", dxf.Layouts.Count);
			foreach (var o in dxf.Layouts)
			{
				Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Layouts.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("IMAGE DEFINITIONS: {0}", dxf.ImageDefinitions.Count);
			foreach (var o in dxf.ImageDefinitions)
			{
				Console.WriteLine("\t{0}; File name: {1}; References count: {2}", o.Name, o.FileName, dxf.ImageDefinitions.GetReferences(o.Name).Count);
			}
			Console.WriteLine();

			Console.WriteLine("GROUPS: {0}", dxf.Groups.Count);
			foreach (var o in dxf.Groups)
			{
				Console.WriteLine("\t{0}; Entities count: {1}", o.Name, o.Entities.Count);
			}
			Console.WriteLine();

			// the entities lists contain the geometry that has a graphical representation in the drawing across all layouts,
			// to get the entities that belongs to an specific layout you can get the references through the Layouts.GetReferences(name)
			// or check the EntityObject.Owner.Record.Layout property
			Console.WriteLine("ENTITIES:");
			Console.WriteLine("\t{0}; count: {1}", EntityType.Arc, dxf.Arcs.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.AttributeDefinition, dxf.AttributeDefinitions.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Circle, dxf.Circles.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Dimension, dxf.Dimensions.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Ellipse, dxf.Ellipses.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Face3D, dxf.Faces3d.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Hatch, dxf.Hatches.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Image, dxf.Images.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Insert, dxf.Inserts.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.LightWeightPolyline, dxf.LwPolylines.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Line, dxf.Lines.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Mesh, dxf.Meshes.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.MLine, dxf.MLines.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.MText, dxf.MTexts.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Point, dxf.Points.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.PolyfaceMesh, dxf.PolyfaceMeshes.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Polyline, dxf.Polylines.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Solid, dxf.Solids.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Spline, dxf.Splines.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Text, dxf.Texts.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Ray, dxf.Rays.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.Viewport, dxf.Viewports.Count);
			Console.WriteLine("\t{0}; count: {1}", EntityType.XLine, dxf.XLines.Count);
			Console.WriteLine();
		}


	}
}

