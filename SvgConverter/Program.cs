
namespace SvgConverter
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

			SvgToPdf SvgToPdfInstance = new SvgToPdf ();
			SvgToPdfInstance.CreatePdf ();

            // MainClass.Convert();

			System.Console.WriteLine(System.Environment.NewLine);
			System.Console.WriteLine(@" --- Press any key to continue --- ");
			System.Console.ReadKey();
        } // End Sub Main 


    } // End Class Program 


} // End Namespace SvgConverter 
