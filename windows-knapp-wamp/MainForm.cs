using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace windows_knapp_wamp
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        Process apacheProcess;
        Process mysqlProcess;

        Thread processMonitorThread;

        public MainForm()
        {
            InitializeComponent();

            UpdateApacheStatus();
            UpdateMySqlStatus();
            StartMonitoringProcesses();
            GetHtdocsDir();
        }

        private void startApacheButton_Click(object sender, EventArgs e)
        {
            StopMonitoringProcesses();

            FixDirectories();

            // Create the apache process.
            apacheProcess = new Process();
            apacheProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Tools\\apache\\bin\\httpd.exe";
            apacheProcess.StartInfo.Arguments = "";
            apacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            apacheProcess.Start();

            UpdateApacheStatus();
            StartMonitoringProcesses();
        }

        private void stopApacheButton_Click(object sender, EventArgs e)
        {
            StopMonitoringProcesses();

            if (apacheProcess != null)
            {
                // Stop the process
                if (!apacheProcess.HasExited)
                {
                    apacheProcess.Kill();
                }

                // Dispose the process
                apacheProcess.Dispose();
                apacheProcess = null;
            }

            UpdateApacheStatus();
            StartMonitoringProcesses();
        }

        private void startMySqlButton_Click(object sender, EventArgs e)
        {
            StopMonitoringProcesses();

            FixDirectories();

            // Create the apache process.
            Process mysqlProcess = new Process();
            mysqlProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Tools\\mysql\\mysql_start.bat";
            mysqlProcess.StartInfo.Arguments = "";
            mysqlProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            mysqlProcess.Start();

            UpdateMySqlStatus();
            StartMonitoringProcesses();
        }

        private void stopMySqlButton_Click(object sender, EventArgs e)
        {
            StopMonitoringProcesses();

            if (mysqlProcess != null)
            {
                // Stop the process
                if (!mysqlProcess.HasExited)
                {
                    mysqlProcess.Kill();
                }

                // Dispose the process
                mysqlProcess.Dispose();
                mysqlProcess = null;
            }

            UpdateMySqlStatus();
            StartMonitoringProcesses();
        }

        private void StartMonitoringProcesses()
        {
            processMonitorThread = new Thread(() =>
            {
                while (true)
                {
                    // Check if there is a running process
                    if (apacheProcess == null)
                    {
                        // Try to fetch an apache process
                        Process[] processes = Process.GetProcessesByName("httpd");
                        if (processes.Length > 0)
                        {
                            // Start monitoring the first process found
                            apacheProcess = processes[0];

                            Invoke(new Action(() =>
                            {
                                UpdateApacheStatus();
                            }));
                        }
                    }

                    // Check if current process was killed
                    else if (apacheProcess != null && apacheProcess.HasExited)
                    {
                        // Dispose the process
                        apacheProcess.Dispose();
                        apacheProcess = null;

                        Invoke(new Action(() =>
                        {
                            UpdateApacheStatus();
                        }));
                    }

                    // Check if there is a running process
                    if (mysqlProcess == null)
                    {
                        // Try to fetch an apache process
                        Process[] processes = Process.GetProcessesByName("mysqld");
                        if (processes.Length > 0)
                        {
                            // Start monitoring the first process found
                            mysqlProcess = processes[0];

                            Invoke(new Action(() =>
                            {
                                UpdateMySqlStatus();
                            }));
                        }
                    }

                    // Check if current process was killed
                    else if (mysqlProcess != null && mysqlProcess.HasExited)
                    {
                        // Dispose the process
                        mysqlProcess.Dispose();
                        mysqlProcess = null;

                        Invoke(new Action(() =>
                        {
                            UpdateMySqlStatus();
                        }));
                    }
                }
            });
            processMonitorThread.Start();
        }

        private void StopMonitoringProcesses()
        {
            processMonitorThread.Abort();
            processMonitorThread = null;
        }

        private void UpdateApacheStatus()
        {
            if (apacheProcess != null)
            {
                startApacheButton.Enabled = false;
                stopApacheButton.Enabled = true;
            } else
            {
                startApacheButton.Enabled = true;
                stopApacheButton.Enabled = false;
            }
        }

        private void UpdateMySqlStatus()
        {
            if (mysqlProcess != null)
            {
                startMySqlButton.Enabled = false;
                stopMySqlButton.Enabled = true;
            }
            else
            {
                startMySqlButton.Enabled = true;
                stopMySqlButton.Enabled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            processMonitorThread.Abort();
            processMonitorThread = null;
        }

        private void FixDirectories()
        {
            string oldDirectory = File.ReadAllText("Tools\\mysql\\data\\my.ini");
            oldDirectory = oldDirectory.Substring(18);
            oldDirectory = oldDirectory.Substring(0, oldDirectory.IndexOf("/Tools/mysql/data\r\n"));

            //oldDirectory = "[[BASE_DIRECTORY]]";
            string newDirectoryBackslash = AppDomain.CurrentDomain.BaseDirectory;
            newDirectoryBackslash = newDirectoryBackslash.Substring(0, newDirectoryBackslash.Length - 1);
            string newDirectory = newDirectoryBackslash.Replace("\\", "/");

            string oldDirectoryBackslash = "[[BASE_DIRECTORY_BACKSLASH]]";
            if (oldDirectory != "[[BASE_DIRECTORY]]")
            {
                oldDirectoryBackslash = oldDirectory.Replace("/", "\\");
            }

            if (oldDirectory == newDirectory)
            {
                return;
            }

            var files = new string[] {
            "Tools\\apache\\conf\\httpd.conf",
            "Tools\\apache\\conf\\extra\\httpd-autoindex.conf",
            "Tools\\apache\\conf\\extra\\httpd-dav.conf",
            "Tools\\apache\\conf\\extra\\httpd-manual.conf",
            "Tools\\apache\\conf\\extra\\httpd-multilang-errordoc.conf",
            "Tools\\apache\\conf\\extra\\httpd-ssl.conf",
            "Tools\\apache\\conf\\extra\\httpd-vhosts.conf",
            "Tools\\apache\\conf\\extra\\httpd-xampp.conf",
            "Tools\\mysql\\backup\\my.ini",
            "Tools\\mysql\\bin\\my.ini",
            "Tools\\mysql\\data\\my.ini"
            };
            
            foreach(string file in files)
            {
                string contents = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + file);
                contents = contents.Replace(oldDirectory, newDirectory);
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + file);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + file, contents);
               // System.Diagnostics.Debugger.Break();
            }

            var backslash_files = new string[] {
                "Tools\\php\\pci",
                "Tools\\php\\pci.bat",
                "Tools\\php\\pciconf",
                "Tools\\php\\pciconf.bat",
                "Tools\\php\\pear.bat",
                "Tools\\php\\peardev.bat",
                "Tools\\php\\pecl.bat",
                "Tools\\php\\php.ini",
                "Tools\\php\\phpunit",
                "Tools\\php\\phpunit.bat",
                "Tools\\php\\pear\\peclcmd.php",
                "Tools\\php\\pear\\.registry\\archive_tar.reg",
                "Tools\\php\\pear\\.registry\\console_getopt.reg",
                "Tools\\php\\pear\\.registry\\pear.reg",
                "Tools\\php\\pear\\.registry\\structures_graph.reg",
                "Tools\\php\\pear\\.registry\\xml_util.reg",
                "Tools\\php\\pear\\PHPUnit\\Util\\PHP.php",
                "Tools\\php\\tests\\Structures_Graph\\tests\\helper.inc"

            };

            foreach (string file in backslash_files)
            {
                string contents = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + file);
                contents = contents.Replace(oldDirectoryBackslash, newDirectoryBackslash);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + file, contents);
            }
        }

        private void GetHtdocsDir()
        {
            string[] contents = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Tools\\apache\\conf\\httpd.conf");
            for(int i = 0; i < contents.Length; i++)
            {
                if (contents[i].StartsWith("DocumentRoot"))
                {
                    string line = contents[i];
                    string htdocs = line.Substring(line.IndexOf("\"") + 1);
                    htdocs = htdocs.Substring(0, htdocs.IndexOf("\""));
                    htdocs = htdocs.Replace("/", "\\");
                    textEdit.Text = htdocs;
                    return;
                }
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = textEdit.EditValue.ToString();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string htdocs = folderBrowserDialog.SelectedPath;
                SetHtdocsDir(htdocs);
            }
        }

        private void SetHtdocsDir(string htdocs)
        {
            textEdit.Text = htdocs;
            htdocs = htdocs.Replace("\\", "/");
            System.Diagnostics.Debugger.Break();
            string[] contents = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Tools\\apache\\conf\\httpd.conf");
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i].StartsWith("DocumentRoot"))
                {
                    contents[i] = "DocumentRoot \"" + htdocs + "\"";
                    contents[i + 1] = "<Directory \"" + htdocs + "\">";
                }
            }
            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "Tools\\apache\\conf\\httpd.conf", contents);
        }
    }
}
