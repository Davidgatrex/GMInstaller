using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Net.Http;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Formats.Tar;

namespace GMInstaller
{
    public partial class Form1 : Form
    {

        public static string PDataPath = string.Empty;
        private List<MamboProgram> AppList = new();
        private bool UpdateWarn = false;
        private bool UpdateRequired = false;

        public static string lupPath = "";

        public Form1()
        {
            InitializeComponent();

            if (RegUtils.ValuePresent(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller", "ProgramFilesPath"))
            {
                PDataPath = RegUtils.ReadValueString(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller", "ProgramFilesPath") ?? "";
                ProgFilesTB.Text = PDataPath;
                folderBrowserDialog1.InitialDirectory = PDataPath;
            }

            if (RegUtils.ValuePresent(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller", "MamboFilesPath"))
            {
                lupPath = RegUtils.ReadValueString(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller", "MamboFilesPath") ?? "";
                MamboFilesTB.Text = lupPath;
                folderBrowserDialog2.InitialDirectory = lupPath;
            }
            RefreshLabels();
            ReloadListBtn_Click(this, new());
        }

        private void UpdateAppList()
        {
            AppList.Clear();
            string embedMambodata = System.Text.Encoding.UTF8.GetString(GMInstaller.Properties.Resources.GMInstaller_Mambo);
            MamboProgram embedMambo = new(embedMambodata, Application.StartupPath);
            if(embedMambo.isValid)
                AppList.Add(embedMambo);
            if (lupPath.Length == 0)
                return;
            string[] files = Directory.EnumerateFiles(lupPath).ToArray();
            foreach (string file in files)
            {
                string[] p = file.Split('\\').Last().Split('.');
                if (p.Length != 2)
                    continue;

                if (p[1].ToLower().Equals("mambo"))
                {
                    MamboProgram mamboProgram = new(File.ReadAllText(file), lupPath);
                    if (!mamboProgram.isValid)
                        continue;
                    AppList.Add(mamboProgram);
                }
            }

            GC.Collect();
        }

        private void ProgFilesTB_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(ProgFilesTB.Text) || ProgFilesTB.Text.Length == 0)
                PDataPath = ProgFilesTB.Text;
        }

        private void ProgFilesButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.InitialDirectory = PDataPath;
            folderBrowserDialog1.ShowDialog();

            if (folderBrowserDialog1.SelectedPath.Length > 0 && Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                PDataPath = folderBrowserDialog1.SelectedPath;
                ProgFilesTB.Text = PDataPath;
            }
        }

        private void RefreshLabels()
        {
            try
            {
                MamboProgram mamboProgram = AppList[ProgramsListBox.SelectedIndex];

                PName_Lab.Text = mamboProgram.ProgName;
                PVer_Lab.Text = mamboProgram.localVersion;
                if (mamboProgram.isInstalled)
                {
                    PVer_Lab.Text += $" (Installed: {mamboProgram.installedVersion})";
                    int CompVer = mamboProgram.CompareVersions();

                    PStat_Lab.Text = "Installed";
                    if (CompVer == 2)
                    {
                        PStat_Lab.Text += " (Potentially corrupted. Please, update)";
                    }
                    else if (CompVer == 1)
                    {
                        PStat_Lab.Text += " (Outdated)";
                    }
                    else
                    {
                        PStat_Lab.Text += " (Up to date)";
                    }
                    UpdateWarn = CompVer <= 0;
                    UpdateRequired = CompVer == 2;
                }
                else
                {
                    PStat_Lab.Text = "Not Installed";
                }


            }
            catch (ArgumentOutOfRangeException)
            {
                PName_Lab.Text = "";
                PVer_Lab.Text = "";
                PStat_Lab.Text = "";
            }

            RefreshInstalledProgs();
        }

        private void RefreshInstalledProgs()
        {
            string[]? names = RegUtils.ListValueNames(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller\InstalledPrograms");
            InstalledProgsLB.Items.Clear();
            if (names == null)
                return;
            foreach(string n in names)
            {
                InstalledProgsLB.Items.Add($"{n} ver. {RegUtils.ReadValueString(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller\InstalledPrograms", n) ?? "Unknown"}");
            }

            GC.Collect();
        }

        private void ProgramsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshLabels();
            RefreshButonsStatus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"This action will overwrite currently saved settings with \"{PDataPath}\". Continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                RegUtils.WriteValue(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller", "ProgramFilesPath", PDataPath);
            }
        }

        private void ReloadListBtn_Click(object sender, EventArgs e)
        {
            UpdateAppList();
            ProgramsListBox.Items.Clear();
            foreach (MamboProgram p in AppList)
                ProgramsListBox.Items.Add(p.ProgName);

            RefreshButonsStatus();

            GC.Collect();
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            try
            {
                MamboProgram mamboProgram = AppList[ProgramsListBox.SelectedIndex];

                if (mamboProgram.Install())
                {
                    MessageBox.Show("Program installed successfully");
                    if(mamboProgram.LaunchOnInstall)
                    {
                        mamboProgram.Launch();
                        if (mamboProgram.CloseInstallerOnLaunch)
                        {
                            Application.Exit();
                        }
                    }
                }
                else
                    MessageBox.Show("Error installing program");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Selection out of range");
            }

            RefreshLabels();
            RefreshButonsStatus();
        }

        private void UninstallButton_Click(object sender, EventArgs e)
        {
            try
            {
                MamboProgram mamboProgram = AppList[ProgramsListBox.SelectedIndex];

                if (mamboProgram.Uninstall())
                    MessageBox.Show("Program uninstalled successfully");
                else
                    MessageBox.Show("Error uninstalling program");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Selection out of range");
            }

            RefreshLabels();
            RefreshButonsStatus();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateWarn)
                {
                    if (MessageBox.Show("You are trying to update, but the currently installed version is equal or greater than the one you're trying to install. Continue (Strongly discouraged)?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        return;
                }
                MamboProgram mamboProgram = AppList[ProgramsListBox.SelectedIndex];

                if (mamboProgram.Update())
                {
                    MessageBox.Show("Program updated successfully");
                    if (mamboProgram.LaunchOnInstall)
                    {
                        mamboProgram.Launch();
                        if (mamboProgram.CloseInstallerOnLaunch)
                        {
                            Application.Exit();
                        }
                    }
                }
                else
                    MessageBox.Show("Error updating program");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Selection out of range");
            }

            RefreshLabels();
            RefreshButonsStatus();
        }

        private void RefreshButonsStatus()
        {
            try
            {
                MamboProgram mamboProgram = AppList[ProgramsListBox.SelectedIndex];

                int CompVer = mamboProgram.CompareVersions();
                InstallButton.Enabled = !mamboProgram.isInstalled;
                UninstallButton.Enabled = mamboProgram.isInstalled;
                UpdateButton.Enabled = (CompVer > -2) && mamboProgram.isInstalled;
                UpdateWarn = CompVer <= 0;
                UpdateRequired = CompVer == 2;


            }
            catch (ArgumentOutOfRangeException)
            {
                InstallButton.Enabled = false;
                UninstallButton.Enabled = false;
                UpdateButton.Enabled = false;
                UpdateWarn = false;
                UpdateRequired = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MamboFilesTB_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(MamboFilesTB.Text) || MamboFilesTB.Text.Length == 0)
            {
                lupPath = MamboFilesTB.Text;
                ReloadListBtn_Click(this, new());
            }

        }

        private void MamboFilesBTN_Click(object sender, EventArgs e)
        {
            folderBrowserDialog2.InitialDirectory = lupPath;
            folderBrowserDialog2.ShowDialog();

            if (folderBrowserDialog2.SelectedPath.Length > 0 && Directory.Exists(folderBrowserDialog2.SelectedPath))
            {
                lupPath = folderBrowserDialog2.SelectedPath;
                MamboFilesTB.Text = lupPath;
                ReloadListBtn_Click(this, new());
            }
        }

        private void MamboSavePath_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"This action will overwrite currently saved settings with \"{lupPath}\". Continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                RegUtils.WriteValue(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller", "MamboFilesPath", lupPath);
            }
        }
    }

    public static class RegUtils
    {
        public static object? ReadValue(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
                return (subKey == null) ? null : subKey.GetValue(valueName);
        }

        public static string? ReadValueString(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
            {
                if (subKey == null)
                    return null;
                object? val = subKey.GetValue(valueName);
                if (val == null)
                    return null;
                if (subKey.GetValueKind(valueName) == RegistryValueKind.String)
                {
                    return (string)val;
                }
                else
                    throw new NotMatchingRegValueTypeException(RegistryValueKind.String, subKey.GetValueKind(valueName), hive, key, valueName, val);
            }
        }

        public static string[]? ListValueNames(RegistryHive hive, string key)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
            {
                if (subKey == null)
                    return null;
                return subKey.GetValueNames();

            }
        }

        public static RegistryValueKind? GetValueKind(RegistryHive hive, string key, string valueName)
        {
            if(!ValuePresent(hive, key, valueName))
            {
                return null;
            }

            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey subKey = baseKey.OpenSubKey(key, writable: true) ?? baseKey.CreateSubKey(key, writable: true))
                return subKey.GetValueKind(valueName);
        }

        public static void WriteValue(RegistryHive hive, string key, string valueName, object value)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            {
                using (RegistryKey subKey = baseKey.OpenSubKey(key, writable: true) ?? baseKey.CreateSubKey(key, writable: true))
                {
                    subKey.SetValue(valueName, value);
                }
            }
        }

        public static void DeleteKeyTree(RegistryHive hive, string key)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                baseKey.DeleteSubKeyTree(key, false);
        }

        public static void DeleteValue(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            {
                using (RegistryKey? subKey = baseKey.OpenSubKey(key, writable: true))
                {
                    if (subKey == null)
                        return;
                    if (valueName.Equals(""))
                        subKey.SetValue("", "");
                    else
                        subKey.DeleteValue(valueName, false);
                }
            }
        }

        public static bool ValuePresent(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
                return (subKey == null) ? false : subKey.GetValue(valueName) != null;
        }

        public static RegistryHive? GetHive(string path)
        {
            if (!path.Contains("\\"))
                return null;
            string HiveRaw = path.Split("\\")[0];
            switch(HiveRaw)
            {
                case "HKEY_CLASSES_ROOT":
                    return RegistryHive.ClassesRoot;
                case "HKEY_CURRENT_USER":
                    return RegistryHive.CurrentUser;
                case "HKEY_LOCAL_MACHINE":
                    return RegistryHive.LocalMachine;
                case "HKEY_USERS":
                    return RegistryHive.Users;
                case "HKEY_CURRENT_CONFIG":
                    return RegistryHive.CurrentConfig;
            }

            return null;
        }

        public static string? GetKeyPath(string path)
        {
            if (!path.Contains("\\") || path.IndexOf('\\') + 1 == path.Length)
                return null;

            return path.Substring(path.IndexOf('\\') + 1);
        }
    }

    public delegate bool CompMethod(string fullpath, string dir);

    public class MamboProgram
    {
        public Dictionary<string, string> MamboParams = new();
        public Dictionary<string, string> MamboRegs = new();
        public List<string> MamboRegsUninstall = new();

        public int CompareVersions()
        {
            if (!isInstalled) // No se debe permitir actualizar porque no está instalado
                return -3;

            bool parse_c = Version.TryParse(localVersion, out Version? ver_c);
            bool parse_i = Version.TryParse(installedVersion, out Version? ver_i);

            if (parse_c && !parse_i) // Se debe incentivar a actualizar porque la versión instalada está potencialmente corrupta
                return 2;

            if (!parse_c) // No se debe permitir actualizar porque la versión local está potencialmente corrupta
                return -2;

            if (ver_c > ver_i) return 1; // Se debe permitir actualizar porque la versión local es mayor que la instalada
            if (ver_c == ver_i) return 0; // Se debe permitir actualizar (Con aviso) porque la versión local es igual que la instalada

            return -1; // Se debe permitir actualizar (Con aviso) porque la versión local es menor que la instalada
        }

        private enum MamboFSM
        {
            Params,
            Regs,
            Regs_Uninst
        }

        public static readonly Dictionary<string, CompMethod> CompMethods = new()
        {
            {"None", 
                ((string fullpath, string dir) => {
                    return true; 
                })
            },
            {"Zip",
                ((string fullpath, string dir) => 
                {
                    ZipArchive zipArchive = new(File.OpenRead(fullpath));
                    zipArchive.ExtractToDirectory(dir);
                    zipArchive.Dispose();
                    File.Delete(fullpath);
                    return true;
                })
            },
            {"Tar",
                ((string fullpath, string dir) =>
                {
                    TarFile.ExtractToDirectory(File.OpenRead(fullpath), dir, true);
                    File.Delete(fullpath);
                    return true;
                })
            }
        };

        public bool isValid = false;

        public bool isInstalled = false;
        public bool LaunchOnInstall = false;
        public bool CloseInstallerOnLaunch = false;
        public string installedVersion = "";

        public CompMethod compMethod = CompMethods["None"];
        public string DiskFile = "";
        public string InstallDir = "";
        public string ExecName = "";
        public string ProgName = "";
        public string localVersion = "";
        public string MamboPath = "";

        private void MamboFSM_Params(string line)
        {
            if (!line.Contains('='))
                return;
            string[] parts = line.Split("=");
            if (parts.Length < 2)
                return;
            if (parts.Length > 2)
            {
                string? partb = null;
                foreach (string part in parts)
                {
                    if (partb == null)
                    {
                        partb = "";
                        continue;
                    }
                    if (partb.Equals(""))
                    {
                        partb = part;
                        continue;
                    }

                    partb = $"={part}";
                }

                parts = new string[] { parts[0], partb ?? "" };
            }
            MamboParams.Add(parts[0], parts[1]);
        }

        private void MamboFSM_Regs(string line)
        {
            if (!line.Contains('=') || !line.Contains("\\"))
                return;
            string[] parts = line.Split("=");
            if (parts.Length < 2)
                return;
            if (parts.Length > 2)
            {
                string? partb = null;
                foreach (string part in parts)
                {
                    if (partb == null)
                    {
                        partb = "";
                        continue;
                    }
                    if (partb.Equals(""))
                    {
                        partb = part;
                        continue;
                    }

                    partb = $"={part}";
                }

                parts = new string[] { parts[0], partb ?? "" };
            }
            MamboRegs.Add(parts[0], parts[1]);
        }

        private void MamboFSM_RegsUninstall(string line)
        {
            if(line.Contains("\\"))
                MamboRegsUninstall.Add(line);
        }

        public MamboProgram(string mambo, string mamboPath)
        {
            MamboPath = mamboPath;
            string[] lines = mambo.Split('\n');
            MamboFSM FSM = MamboFSM.Params;

            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Trim();

            MamboParams.Add("ProgFilesDir", Form1.PDataPath);

            foreach (string line in lines)
            {
                if (line.Equals("$reg"))
                {
                    FSM = MamboFSM.Regs;
                    continue;
                }

                if (line.Equals("$reg_uninst"))
                {
                    FSM = MamboFSM.Regs_Uninst;
                    continue;
                }

                switch (FSM)
                {
                    case MamboFSM.Params:
                        MamboFSM_Params(line); break;

                    case MamboFSM.Regs:
                        MamboFSM_Regs(line); break;

                    case MamboFSM.Regs_Uninst:
                        MamboFSM_RegsUninstall(line); break;
                }
            }

            ResolveParams();
            ExtractParams();

            if (RegUtils.ValuePresent(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller\InstalledPrograms", ProgName))
            {
                installedVersion = RegUtils.ReadValueString(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller\InstalledPrograms", ProgName) ?? "";
                isInstalled = true;
            }
        }

        private void ExtractParams()
        {
            if (!MamboParams.ContainsKey("Comp") || !MamboParams.ContainsKey("DiskFile") || !MamboParams.ContainsKey("InstallDir") ||
                !MamboParams.ContainsKey("ExecName") || !MamboParams.ContainsKey("Ver") || !MamboParams.ContainsKey("Name"))
                return;

            ExecName = MamboParams["ExecName"];
            localVersion = MamboParams["Ver"];

            Version v;
            if (!Version.TryParse(localVersion, out v!))
                isValid = false;

            if (MamboParams.ContainsKey("LaunchOnInstall"))
                LaunchOnInstall = MamboParams["LaunchOnInstall"].Equals("true");

            if (MamboParams.ContainsKey("CloseInstallerOnLaunch"))
                CloseInstallerOnLaunch = MamboParams["CloseInstallerOnLaunch"].Equals("true");

            DiskFile = MamboParams["DiskFile"];
            InstallDir = MamboParams["InstallDir"];
            ProgName = MamboParams["Name"];

            // Compression Method. Slightly bigger logic

            string compMet = MamboParams["Comp"];

            if (CompMethods.ContainsKey(compMet))
                compMethod = CompMethods[compMet];

            isValid = true;
        }

        private void ResolveParams()
        {
            Dictionary<string, string> regs_tmp = new();
            MatchEvaluator evaluator = m =>
            {
                string san = m.Groups[1].Value;
                if (MamboParams.ContainsKey(san))
                    return MamboParams[san];
                else
                    return m.Value;
            };
            for (int i = 0; i < MamboParams.Count; i++)
                MamboParams[MamboParams.Keys.ElementAt(i)] = Regex.Replace(MamboParams.Values.ElementAt(i), "%\\[(.*?)\\]", evaluator);

            for (int i = 0; i < MamboRegs.Count; i++)
            {
                string k = Regex.Replace(MamboRegs.Keys.ElementAt(i), "%\\[(.*?)\\]", evaluator);

                string v = Regex.Replace(MamboRegs.Values.ElementAt(i), "%\\[(.*?)\\]", evaluator);

                regs_tmp.Add(k, v);
            }

            for (int i = 0; i < MamboRegsUninstall.Count; i++)
            {

                MamboRegsUninstall[i] = Regex.Replace(MamboRegsUninstall[i], "%\\[(.*?)\\]", evaluator);
            }

            MamboRegs = regs_tmp;
        }

        private void CreateShortcut()
        {
            // 1. Validar si el script solicita el acceso directo
            if (!MamboParams.ContainsKey("CreateStartMenuShortcut") ||
                !MamboParams["CreateStartMenuShortcut"].ToLower().Equals("true"))
                return;

            try
            {
                // 2. Obtener la ruta del Menú de Inicio del usuario actual
                // Si quisieras para todos los usuarios sería: CommonStartMenu
                string startMenuPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                    "Programs"
                );

                // Si quieres crear una carpeta propia para tu suite dentro del menú:
                string mamboFolder = Path.Combine(startMenuPath, "GenerallyMambo");
                if (!Directory.Exists(mamboFolder)) Directory.CreateDirectory(mamboFolder);

                string shortcutName = MamboParams.ContainsKey("StartMenuShortcutName") ? MamboParams["StartMenuShortcutName"] : ProgName;
                string shortcutPath = Path.Combine(mamboFolder, $"{shortcutName}.lnk");
                string targetExePath = Path.Combine(InstallDir, ExecName);

                // 3. Crear el acceso directo usando Windows Script Host de forma dinámica (vía COM)
                Type? shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType == null) return;

                dynamic shell = Activator.CreateInstance(shellType)!;
                dynamic shortcut = shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = targetExePath;
                shortcut.WorkingDirectory = InstallDir;
                shortcut.Description = MamboParams.ContainsKey("StartMenuShortcutDescription") ? MamboParams["StartMenuShortcutDescription"] : "";

                shortcut.Save();
            }
            catch (Exception ex)
            {
                // Si falla la creación del acceso directo, el instalador no debería colapsar
                System.Diagnostics.Debug.WriteLine($"Error al crear acceso directo: {ex.Message}");
            }
        }

        private void DeleteShortcut()
        {
            // 1. Validar si el script solicita el acceso directo
            if (!MamboParams.ContainsKey("CreateStartMenuShortcut") ||
                !MamboParams["CreateStartMenuShortcut"].ToLower().Equals("true"))
                return;

            try
            {
                // 2. Obtener la ruta del Menú de Inicio del usuario actual
                // Si quisieras para todos los usuarios sería: CommonStartMenu
                string startMenuPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                    "Programs"
                );

                // Si quieres crear una carpeta propia para tu suite dentro del menú:
                string mamboFolder = Path.Combine(startMenuPath, "GenerallyMambo");
                if (!Directory.Exists(mamboFolder)) Directory.CreateDirectory(mamboFolder);

                string shortcutName = MamboParams.ContainsKey("StartMenuShortcutName") ? MamboParams["StartMenuShortcutName"] : ProgName;
                string shortcutPath = Path.Combine(mamboFolder, $"{shortcutName}.lnk");

                if (File.Exists(shortcutPath))
                    File.Delete(shortcutPath);

            }
            catch (Exception ex)
            {
                // Si falla la creación del acceso directo, el instalador no debería colapsar
                System.Diagnostics.Debug.WriteLine($"Error al eliminar acceso directo: {ex.Message}");
            }
        }

        public bool Install()
        {
            if(!(MamboParams.ContainsKey("NoCopy") && MamboParams["NoCopy"].Equals("true")))
                if(!CopyFiles(false)) return false;
            if(!SetupRegistry()) return false;
            RegUtils.WriteValue(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller\InstalledPrograms", ProgName, localVersion);
            installedVersion = localVersion;
            isInstalled = true;
            CreateShortcut();
            return true;
        }

        public bool Update()
        {
            if (!(MamboParams.ContainsKey("NoCopy") && MamboParams["NoCopy"].Equals("true")))
                if (!CopyFiles(true)) return false;
            if (!SetupRegistry()) return false;
            RegUtils.WriteValue(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller\InstalledPrograms", ProgName, localVersion);
            installedVersion = localVersion;
            CreateShortcut();
            return true;
        }

        public bool Uninstall()
        {
            if(!DeleteFiles()) return false;
            if (!ClearRegistry()) return false;
            RegUtils.DeleteValue(RegistryHive.LocalMachine, @"SOFTWARE\GenerallyMambo\GMInstaller\InstalledPrograms", ProgName);
            installedVersion = "";
            isInstalled = false;
            return true;
        }

        private bool DeleteFiles()
        {
            if(Directory.Exists(InstallDir))
                Directory.Delete(InstallDir, true);

            DeleteShortcut();
            return true;
        }

        private bool CopyFiles(bool Update)
        {
            string dFilePath_O = MamboPath + "\\" + DiskFile;
            string dFilePath_T = InstallDir + "\\" + DiskFile;
            bool gotFromInet = false;

            if (!File.Exists(dFilePath_O))
            {
                string? url = GetParam("URL");
                if (url == null || url.Length == 0)
                    return false;
                try
                {
                    MessageBox.Show("File not found in disk. Downloading it from " + url);
                    using HttpClient httpClient = new();

                    byte[] fileBytes = httpClient.GetByteArrayAsync(url).GetAwaiter().GetResult();

                    if (!Directory.Exists(MamboPath))
                        Directory.CreateDirectory(MamboPath);

                    File.WriteAllBytes(dFilePath_O, fileBytes);
                    gotFromInet = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }

            if (!Directory.Exists(InstallDir))
                Directory.CreateDirectory(InstallDir);

            if (File.Exists(dFilePath_T))
            {
                if (Update)
                    File.Delete(dFilePath_T);
                else
                    return false;
            }

            if( Update)
            {
                foreach (string s in Directory.GetFiles(InstallDir))
                    File.Delete(s);
            }

            File.Copy(dFilePath_O, dFilePath_T);
            if (gotFromInet)
                File.Delete(dFilePath_O);

            // Ejecuta tu método de descompresión (el de la tabla de bytes)
            return compMethod(dFilePath_T, InstallDir);
        }

        public bool SetupRegistry()
        {
            // Testing
            foreach(KeyValuePair<string, string> pair in MamboRegs)
            {
                if (pair.Key.Contains('#') && pair.Key.Split('#').Length < 2)
                    return false;
                string rKey = pair.Key.Contains('#') ? pair.Key.Split('#')[0] : pair.Key; 
                string rValName = pair.Key.Contains('#') ? pair.Key.Split('#')[1] : "";
                RegistryHive? hive = RegUtils.GetHive(rKey);
                string? rKPath = RegUtils.GetKeyPath(rKey);

                if (hive == null || rKPath == null)
                    return false;
            }

            // Writing
            foreach (KeyValuePair<string, string> pair in MamboRegs)
            {
                string rKey = pair.Key.Contains('#') ? pair.Key.Split('#')[0] : pair.Key;
                string rValName = pair.Key.Contains('#') ? pair.Key.Split('#')[1] : "";
                RegistryHive hive = RegUtils.GetHive(rKey) ?? RegistryHive.CurrentConfig;
                string rKPath = RegUtils.GetKeyPath(rKey) !;

                RegUtils.WriteValue(hive, rKPath, rValName.Equals("@") ? "" : rValName, pair.Value);
            }
            return true;
        }

        public bool ClearRegistry()
        {
            // Testing
            foreach (string s in MamboRegsUninstall)
            {
                if (s.Contains('#') && s.Split('#').Length < 2)
                    return false;
                string rKey = s.Contains('#') ? s.Split('#')[0] : s;
                string rValName = s.Contains('#') ? s.Split('#')[1] : "";
                RegistryHive? hive = RegUtils.GetHive(rKey);
                string? rKPath = RegUtils.GetKeyPath(rKey);

                if (hive == null || rKPath == null)
                    return false;
            }

            // Writing
            foreach (string s in MamboRegsUninstall)
            {
                string rKey = s.Contains('#') ? s.Split('#')[0] : s;
                string rValName = s.Contains('#') ? s.Split('#')[1] : "";
                RegistryHive hive = RegUtils.GetHive(rKey) ?? RegistryHive.CurrentConfig;
                string rKPath = RegUtils.GetKeyPath(rKey) !;

                if (rValName.Length == 0)
                    RegUtils.DeleteKeyTree(hive, rKPath);
                else
                    RegUtils.DeleteValue(hive, rKPath, rValName.Equals("@") ? "" : rValName);
            }
            return true;
        }

        public string? GetParam(string name)
        {
            if (!MamboParams.ContainsKey(name))
                return null;

            return MamboParams[name];
        }

        public void Launch()
        {

            string fullPath = Path.Combine(InstallDir, ExecName);
            MessageBox.Show($"Launching program {fullPath}");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = fullPath,
                WorkingDirectory = InstallDir,
                UseShellExecute = false
            };

            startInfo.EnvironmentVariables.Clear();

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error launching process: {ex.Message}");
            }
        }
    }

    // Exceptions

    public class NotMatchingRegValueTypeException : Exception
    {
        RegistryHive hive;
        RegistryValueKind requestedType;
        RegistryValueKind gotType;
        string key;
        string valueName;
        object? value;
        public NotMatchingRegValueTypeException(RegistryValueKind _req, RegistryValueKind _got, RegistryHive _hive, string _key, string _valueName, object? _value)
        {
            value = _value;
            hive = _hive;
            requestedType = _req;
            gotType = _got;
            key = _key;
            valueName = _valueName;
        }
    }
}
