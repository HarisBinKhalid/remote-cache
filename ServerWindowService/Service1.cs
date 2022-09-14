using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServerWindowService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = @"D:\\C#\\RemoteCacheApp\\ServerSocket\\bin\\Debug\\net6.0-windows\\ServerSocket.exe";
            startinfo.CreateNoWindow = true;
            startinfo.UseShellExecute = true;
            Process myProcess = Process.Start(startinfo);
            myProcess.Start();

            myProcess.WaitForExit();
        }

        protected override void OnStop()
        {
        }
    }
}
