using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace aetherion_server
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        Process apacheProcess;
        Process mysqlProcess;

        Thread processMonitorThread;

        public MainForm()
        {
            InitializeComponent();

            SetLabels();
            PositionWindowAtBottomRight();

            UpdateApacheStatus();
            UpdateMySqlStatus();
            StartMonitoringProcesses();
            GetHtdocsDir();
            RefreshWebsitesList();
        }

        /// <summary>
        /// Refresh the websites list.
        /// </summary>
        private void RefreshWebsitesList()
        {
            websitesTree.BeginUnboundLoad();
            websitesTree.Nodes.Clear();

            string htdocs = textEdit.Text;
            if (Directory.Exists(htdocs))
            {
               foreach(string directory in Directory.GetDirectories(htdocs))
                {
                    string name = directory.Substring(directory.LastIndexOf("\\") + 1);
                    TreeListNode node = websitesTree.AppendNode(new object[] { name }, null);
                }
            }

            websitesTree.EndUnboundLoad();
        }

        #region Start/Stop Processes

            /// <summary>
            /// Start the Apache process.
            /// </summary>
        private void StartApache()
        {
            // Create the apache process.
            apacheProcess = new Process();
            apacheProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Tools\\apache\\bin\\httpd.exe";
            apacheProcess.StartInfo.Arguments = "";
            apacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            apacheProcess.Start();

            UpdateApacheStatus();
        }

        /// <summary>
        /// Stop the Apache process.
        /// </summary>
        private void StopApache()
        {
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
        }

        /// <summary>
        /// Start the MySQL process.
        /// </summary>
        private void StartMySql()
        {
            // Create the apache process.
            Process mysqlProcess = new Process();
            mysqlProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Tools\\mysql\\mysql_start.bat";
            mysqlProcess.StartInfo.Arguments = "";
            mysqlProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            mysqlProcess.Start();

            UpdateMySqlStatus();
        }

        /// <summary>
        /// Stop the MySQL process.
        /// </summary>
        private void StopMySql()
        {
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
        }
        #endregion

        #region Monitoring Processes

        /// <summary>
        /// Start monitoring the processes.
        /// </summary>
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

                    if (startButton.Enabled != (apacheProcess == null || mysqlProcess == null))
                    {
                        Invoke(new Action(() =>
                        {
                            startButton.Enabled = apacheProcess == null || mysqlProcess == null;
                        }));
                    }

                    if (stopButton.Enabled != (apacheProcess != null || mysqlProcess != null))
                    {
                        Invoke(new Action(() =>
                        {
                            stopButton.Enabled = apacheProcess != null || mysqlProcess != null;
                        }));
                    }

                    if (pmaButton.Enabled != (apacheProcess != null && mysqlProcess != null))
                    {
                        Invoke(new Action(() =>
                        {
                            pmaButton.Enabled = (apacheProcess != null && mysqlProcess != null);
                        }));
                    }

                    Thread.Sleep(100);
                }
            });
            processMonitorThread.Start();
        }

        /// <summary>
        /// Stop monitoring the processes.
        /// </summary>
        private void StopMonitoringProcesses()
        {
            processMonitorThread.Abort();
            processMonitorThread = null;
        }

        /// <summary>
        /// Check if the Apache process is running.
        /// </summary>
        private void UpdateApacheStatus()
        {
            if (apacheProcess != null)
            {
                apacheStatusItem.State = DevExpress.XtraEditors.StepProgressBarItemState.Active;
                textEdit.Properties.Buttons[0].Enabled = false;
            }
            else
            {
                apacheStatusItem.State = DevExpress.XtraEditors.StepProgressBarItemState.Inactive;
                textEdit.Properties.Buttons[0].Enabled = true;
            }
        }

        /// <summary>
        /// Check if the MySQL process is running.
        /// </summary>
        private void UpdateMySqlStatus()
        {
            if (mysqlProcess != null)
            {
                mysqlStatusItem.State = DevExpress.XtraEditors.StepProgressBarItemState.Active;
            }
            else
            {
                mysqlStatusItem.State = DevExpress.XtraEditors.StepProgressBarItemState.Inactive;
            }
        }
        #endregion

        #region Actions

        private void startButton_Click(object sender, EventArgs e)
        {
            string htdocs = textEdit.Text;
            while (!Directory.Exists(htdocs))
            {
                XtraMessageBox.Show("The websites root directory does not exist. Select an existing one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                folderBrowserDialog.SelectedPath = textEdit.EditValue.ToString();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    htdocs = folderBrowserDialog.SelectedPath;
                    SetHtdocsDir(htdocs);
                    RefreshWebsitesList();
                    Thread.Sleep(100);
                    Application.DoEvents();
                    ShowForm();
                } else
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                    ShowForm();
                    return;
                }
                
            }

            StopMonitoringProcesses();

            FixDirectories();
            if (apacheProcess == null)
            {
                StartApache();
            }

            if (mysqlProcess == null)
            {
                StartMySql();
            }

            StartMonitoringProcesses();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopMonitoringProcesses();

            if (apacheProcess != null)
            {
                StopApache();
            }

            if (mysqlProcess != null)
            {
                StopMySql();
            }

            StartMonitoringProcesses();
        }

        
        #endregion

        #region Open / Close Tool

        /// <summary>
        /// Called when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMonitoringProcesses();

            if (apacheProcess != null)
            {
                StopApache();
            }

            if (mysqlProcess != null)
            {
                StopMySql();
            }
        }

        /// <summary>
        /// Called when the form loses focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            HideForm();
        }

        /// <summary>
        /// Called when the form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Ensure the tray icon is removed when the form is closed
            trayIcon.Visible = false;
            trayIcon.Dispose();
            base.OnFormClosed(e);
        }

        /// <summary>
        /// Called when the tray icon is double clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trayIcon_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        /// <summary>
        /// Called when the exit button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            // Confirm exit
            if (XtraMessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                ShowForm();
            }
        }

        /// <summary>
        /// Set the labels in the form.
        /// </summary>
        private void SetLabels()
        {
            titleLabel.Text = Application.ProductName;
            versionLabel.Text = "v" + Application.ProductVersion;
            trayIcon.Text = Application.ProductName;

            // Get apache version
            string phpExePath = AppDomain.CurrentDomain.BaseDirectory + "Tools\\php\\php.exe";
            FileInfo fileInfo = new FileInfo(phpExePath);
            string version = FileVersionInfo.GetVersionInfo(fileInfo.FullName).ProductVersion;
            apacheStatusItem.ContentBlock2.Description = "PHP v" + version;

            // Get mysql version
            string mysqlExePath = AppDomain.CurrentDomain.BaseDirectory + "Tools\\mysql\\bin\\mysql.exe";
            fileInfo = new FileInfo(mysqlExePath);
            version = FileVersionInfo.GetVersionInfo(fileInfo.FullName).ProductVersion;
            mysqlStatusItem.ContentBlock2.Description = "MariaDB v" + version;

        }

        /// <summary>
        /// Position the window at the bottom right of the screen.
        /// </summary>
        private void PositionWindowAtBottomRight()
        {
            int horizontal_padding = 8; // px;
            int vertical_padding = 8; // px;
            Screen screen = Screen.FromControl(this);
            this.Left = screen.WorkingArea.Right - this.Width - horizontal_padding;
            this.Top = screen.WorkingArea.Bottom - this.Height - vertical_padding;
        }

        /// <summary>
        /// Show the form.
        /// </summary>
        private void ShowForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        /// <summary>
        /// Hide the form.
        /// </summary>
        private void HideForm()
        {
            this.Hide();
        }
        #endregion

        #region Websites Root Folder
        /// <summary>
        /// Get the htdocs directory from the configuration file.
        /// </summary>
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

        /// <summary>
        /// Set the htdocs directory in the configuration file.
        /// </summary>
        /// <param name="htdocs"> The htdocs directory. </param>
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

        private void textEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Tag.ToString() == "edit")
            {
                folderBrowserDialog.SelectedPath = textEdit.EditValue.ToString();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string htdocs = folderBrowserDialog.SelectedPath;
                    SetHtdocsDir(htdocs);
                }
                ShowForm();
            }
            else if (e.Button.Tag.ToString() == "browse")
            {
                // Show in Windows Explorer
                string htdocs = textEdit.EditValue.ToString();
                if (Directory.Exists(htdocs))
                {
                    Process.Start("explorer.exe", htdocs);
                }
            }
        }
        #endregion

        /// <summary>
        /// Fix the directories in the configuration files.
        /// </summary>
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

            foreach (string file in files)
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + file))
                    continue;

                string contents = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + file);
                contents = contents.Replace(oldDirectory, newDirectory);
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + file);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + file, contents);
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

        private void websitesTree_SelectionChanged(object sender, EventArgs e)
        {
            if (websitesTree.Selection.Count > 0)
            {
                viewInNavigatorButton.Enabled = true;
                editInVsCodeButton.Enabled = true;
                viewWebsiteInWindowsExplorerButton.Enabled = true;
            }
            else
            {
                viewInNavigatorButton.Enabled = false;
                editInVsCodeButton.Enabled = false;
                viewWebsiteInWindowsExplorerButton.Enabled = false;
            }
        }

        /// <summary>
        /// View the website in the browser.
        /// </summary>
        /// <param name="website"></param>
        private void ViewWebsite(string website)
        {
            string path = "http://localhost/" + "\\" + website;
            Process.Start(path);
        }

        /// <summary>
        /// Edit the website in Visual Studio Code.
        /// </summary>
        /// <param name="website"></param>
        private void EditWebsite(string website)
        {
            string path = textEdit.Text + "\\" + website;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c code \"{path}\"", // Use /c to run and close cmd.exe
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(startInfo);

        }

        /// <summary>
        /// Called when the view in navigator button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewInNavigatorButton_Click(object sender, EventArgs e)
        {
            if (websitesTree.Selection.Count > 0)
            {
                string website = websitesTree.Selection[0].GetDisplayText(0);
                ViewWebsite(website);
            }
        }

        /// <summary>
        /// Called when the edit in Visual Studio Code button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editInVsCodeButton_Click(object sender, EventArgs e)
        {
            if (websitesTree.Selection.Count > 0)
            {
                string website = websitesTree.Selection[0].GetDisplayText(0);
                EditWebsite(website);
            }
        }

        /// <summary>
        /// Called when the websites tree is double clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void websitesTree_DoubleClick(object sender, EventArgs e)
        {
            TreeListHitInfo hi = websitesTree.CalcHitInfo(websitesTree.PointToClient(Control.MousePosition));
            if (hi.Node != null)
            {
                string website = hi.Node.GetDisplayText(0);
                ViewWebsite(website);
            }
        }

        private void viewWebsiteInWindowsExplorerButton_Click(object sender, EventArgs e)
        {
            if (websitesTree.Selection.Count > 0)
            {
                string website = websitesTree.Selection[0].GetDisplayText(0);
                string path = textEdit.Text + "\\" + website;
                Process.Start("explorer.exe", path);
            }
        }

        /// <summary>
        /// Called when the phpMyAdmin button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pmaButton_Click(object sender, EventArgs e)
        {
            Process.Start("http://localhost/phpmyadmin");
        }
    }
}
