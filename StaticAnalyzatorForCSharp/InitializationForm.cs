using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    public partial class InitializationForm : Form
    {
        private Button buttonStart;
        private Button buttonSearchPath;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private ListBox listBox1;
        private Button buttonSettings;
        private TextBox textBoxPath;
        private Form formSettings;
        private static Form mainForm;

        public InitializationForm()
        {
            InitializeComponent();
            mainForm = this;
            Properties.Settings.Default.isLowerSymbolInVariableMessage = Properties.Settings.Default.isLowerSymbolInVariableMessage; //Костыль. Без него не выводится список настроек
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializationForm));
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonSearchPath = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(3, 3);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(645, 20);
            this.textBoxPath.TabIndex = 0;
            this.textBoxPath.Text = "C:\\Users\\user\\source\\repos\\ConsoleApp1\\ConsoleApp1.sln";
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(3, 41);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(123, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Старт";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStartAnalysis);
            // 
            // buttonSearchPath
            // 
            this.buttonSearchPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearchPath.Location = new System.Drawing.Point(3, 3);
            this.buttonSearchPath.Name = "buttonSearchPath";
            this.buttonSearchPath.Size = new System.Drawing.Size(123, 21);
            this.buttonSearchPath.TabIndex = 2;
            this.buttonSearchPath.Text = "Обзор";
            this.buttonSearchPath.UseVisualStyleBackColor = true;
            this.buttonSearchPath.Click += new System.EventHandler(this.ButtonSearchPath);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer1.Size = new System.Drawing.Size(784, 261);
            this.splitContainer1.SplitterDistance = 651;
            this.splitContainer1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxPath, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.51693F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.48306F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(651, 261);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(3, 30);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(645, 225);
            this.listBox1.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.buttonSettings, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.buttonStart, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonSearchPath, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.70588F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.29412F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(129, 261);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.Location = new System.Drawing.Point(3, 88);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(123, 23);
            this.buttonSettings.TabIndex = 3;
            this.buttonSettings.Text = "Настройки";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.ButtonOpenSettingsWindow);
            // 
            // InitializationForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 261);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 300);
            this.Name = "InitializationForm";
            this.Text = "Статистический анализатор языка C#";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ButtonStartAnalysis(object sender, EventArgs e)
        {
            if (textBoxPath.Text == "")
            {
                MessageBox.Show("Введите путь до проекта!", "Ошибка!", MessageBoxButtons.OK);
            }
            else
            {
                if (File.Exists(textBoxPath.Text))
                {
                    listBox1.Items.Clear();
                    new Thread(() =>
                        TestingStaticAnalyzator.Start(textBoxPath.Text, listBox1)).Start();
                    
                }
                else
                {
                    MessageBox.Show("Данного пути не существует!", "Ошибка!", MessageBoxButtons.OK);
                }
            }            
        }

        private void ButtonSearchPath(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SLN-файлы(*.sln)|*.sln";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = openFileDialog.FileName;
            }
        }

        private void ButtonOpenSettingsWindow(object sender, EventArgs e)
        {
            formSettings = new SettingsForm();
            this.Enabled = false;
            formSettings.Show();
        }

        static internal Form GetMainForm()
        {
            return mainForm;
        }
    }
}
