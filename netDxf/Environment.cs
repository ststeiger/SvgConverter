
namespace netDxf
{
    public class Env
    {

		public static bool StrictErrorChecking
		{
			get 
			{
				return false;
			}
		}


        public static string NewLine
        {
            get
			{
                return System.Environment.NewLine;
				// return "\n";
            }
        }

    }
}
