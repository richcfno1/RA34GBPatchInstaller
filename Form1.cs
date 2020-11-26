using AsmResolver.PE.File;
using AsmResolver.PE.File.Headers;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RA34GBPatchInstaller
{
    public partial class Form1 : Form
    {
        string path = "";
        string GameFileRelativePath = "\\Data\\ra3_1.12.game";
        public Form1()
        {
            InitializeComponent();
            UpdateText();
        }

        private void UpdateText()
        {
            path = Registry.GetRA3Path();
            label2.Text = path;
            label3.Text = System.IO.File.Exists(path + GameFileRelativePath) ? "检测到游戏，可以开始安装！" : "没有检测到游戏，请修复注册表！";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tempPath = path + GameFileRelativePath;
            try
            {
                var file = PEFile.FromFile(tempPath);
                if (!file.FileHeader.Characteristics.HasFlag(Characteristics.LargeAddressAware))
                {
                    file.Write(tempPath + ".backup");
                    file.FileHeader.Characteristics |= Characteristics.LargeAddressAware;
                    file.Write(tempPath);
                    MessageBox.Show("备份文件已储存，激活成功！");
                }
                else
                {
                    MessageBox.Show("已经激活，无需重复激活！");
                }
            }
            catch
            {
                MessageBox.Show("请先修复注册表！");
            }
            UpdateText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("请选择游戏安装目录下的RA3.exe文件以建立正确的注册表。");
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择游戏安装目录下的RA3.exe。";
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "RA3 Game |RA3.exe";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Registry.SetRA3Path(openFileDialog.FileName[0..^8]);
                        MessageBox.Show("注册表修复成功！");
                    }
                    catch
                    {
                        DialogResult result = MessageBox.Show("注册失败，需要以管理员权限！", "是否以管理员身份尝试？", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = Process.GetCurrentProcess().MainModule.FileName,
                                Verb = "runas",
                                UseShellExecute = true,
                                Arguments = $"\"{openFileDialog.FileName}\""
                            });
                            this.Close();
                        }
                    }
                }
            }
            UpdateText();
        }
    }
}
