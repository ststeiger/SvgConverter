
namespace DwgToSvgConverter
{


	public class StorageHelper
	{


		public static string GetDesktopPath(string filename)
		{
			string desk = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
			return System.IO.Path.Combine(desk, filename);
		} // End Function GetDesktopPath 


		public static bool DriveExists(string name)
		{
			System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

			// System.IO.DriveInfo di = new System.IO.DriveInfo("");

			foreach (System.IO.DriveInfo di in drives)
			{
				// System.Console.WriteLine(di.Name);
				if (System.StringComparer.InvariantCultureIgnoreCase.Equals(name, di.Name))
					return true;
			} // Next di

			return false;
        } // End Function DriveExists 


    } // End Class StorageHelper 


} // End Namespace DwgToSvgConverter 
