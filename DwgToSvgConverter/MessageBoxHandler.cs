
namespace DwgToSvgConverter
{


    public class MessageBoxHandler
    {

        // protected static System.Threading.Thread m_killThread;

        protected const int WM_CLOSE = 0x0010;

        // WinCE
        // [System.Runtime.InteropServices.DllImport("coredll.dll", EntryPoint="FindWindowW", SetLastError=true)]  
        // protected static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);

        // [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        // protected static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        protected static extern int SendMessage(System.IntPtr hWnd, int wMsg, System.IntPtr wParam, System.IntPtr lParam);


        protected static System.IntPtr FindWindowManaged(string Title)
        {
            System.Diagnostics.Process[] processlist = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process process in processlist)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    // System.Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);

                    if (System.StringComparer.OrdinalIgnoreCase.Equals(process.MainWindowTitle, Title))
                    {
                        // Window found
                        return process.MainWindowHandle;
                    } // End if (System.StringComparer.OrdinalIgnoreCase.Equals(process.MainWindowTitle, "Wout Ware trial"))

                } // End if (!string.IsNullOrEmpty(process.MainWindowTitle))

            } // Next process

            return System.IntPtr.Zero;
        }


        protected static void ContinuousCloseMessageBoxCheck(object obj)
        {
            bool m_keepThreadAlive = true;
            bool m_killHim = true;
            string strWindowTitle = null;

            if (obj != null)
                strWindowTitle = System.Convert.ToString(obj);
            
            if (string.IsNullOrEmpty(strWindowTitle))
                return;

            while (m_keepThreadAlive)
            {
                System.Threading.Thread.Sleep(200);
                if (m_killHim)
                {
                    // System.IntPtr nextW = FindWindow(null, strWindowTitle);
                    System.IntPtr nextW = FindWindowManaged(strWindowTitle);
                    if (nextW != System.IntPtr.Zero)
                    {
                        // System.Windows.Forms.Message closeMessage = System.Windows.Forms.Message.Create(nextW, WM_CLOSE, System.IntPtr.Zero, System.IntPtr.Zero);
                        // MessageWindow.SendMessage(ref closeMessage);  
                        SendMessage(nextW, WM_CLOSE, System.IntPtr.Zero, System.IntPtr.Zero);
                        m_keepThreadAlive = false;
                    } // End if (nextW != System.IntPtr.Zero)

                } // End if (m_killHim)

            } // Whend 

        } // End Sub CloseTestMessageBox


        public static void CloseNextMessageBoxByTitle(string title)
        {
            System.Threading.ParameterizedThreadStart thisThreadStart = new System.Threading.ParameterizedThreadStart(ContinuousCloseMessageBoxCheck);

            //System.Threading.ThreadStart thisThreadStart = new System.Threading.ThreadStart(ContinuousCloseMessageBoxCheck);

            /*
            bool m_killHim = true;
            bool  m_threadAlive =true;
            
            
            System.Threading.ThreadStart thisThreadStart = () =>  
                                    {  
                                        while (m_threadAlive)  
                                        {
                                            System.Threading.Thread.Sleep(200);  
                                            if (m_killHim)  
                                            {  
                                                System.IntPtr nextW = FindWindow(null, "MyDialogConstant");  
                                                if (nextW != System.IntPtr.Zero)  
                                                      
                                                {
                                                    System.Windows.Forms.Message closeMessage = 
                                                        System.Windows.Forms.Message.Create(nextW, WM_CLOSE, System.IntPtr.Zero, System.IntPtr.Zero);

                                                    SendMessage(nextW, WM_CLOSE, System.IntPtr.Zero, System.IntPtr.Zero);
                                                    // MessageWindow.SendMessage(ref closeMessage); // .NET CF
                                                }  
                                            }  
       
                                        }  
                                      
                                      
                                    };
            */

            System.Threading.Thread m_killThread = new System.Threading.Thread(thisThreadStart);
            //m_killThread.Start();
            m_killThread.Start(title);
        }


        public static void Test()
        {
            // m_killHim = true;  
            CloseNextMessageBoxByTitle("MyDialogConstant");
            System.Windows.Forms.MessageBox.Show("MyT", "MyDialogConstant");
            // m_killHim = false;  
        }


    } // End Class MessageBoxHandler


} // End Namespace DwgToSvgConverter
