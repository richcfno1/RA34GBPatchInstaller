using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RA34GBPatchInstaller
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                var maybeRA3 = args.FirstOrDefault(s => s.EndsWith("RA3.exe", StringComparison.OrdinalIgnoreCase));
                if (maybeRA3 is string ra3Path)
                    Registry.SetRA3Path(ra3Path[0..^8]);
            }
            catch
            {
                MessageBox.Show("未知原因错误，请尝试手动以管理员身份重启！");
            }
            Application.Run(new Form1());
        }
    }
}
