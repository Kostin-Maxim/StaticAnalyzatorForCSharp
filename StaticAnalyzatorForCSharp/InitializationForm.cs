using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    public partial class InitializationForm : Form
    {
        private Button buttonStart;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonSearchPath;
        private TextBox textBoxPath;

        public InitializationForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonSearchPath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(13, 24);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(533, 20);
            this.textBoxPath.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(668, 70);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "button1";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStartAnalysis);
            // 
            // buttonSearchPath
            // 
            this.buttonSearchPath.Location = new System.Drawing.Point(668, 24);
            this.buttonSearchPath.Name = "buttonSearchPath";
            this.buttonSearchPath.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchPath.TabIndex = 2;
            this.buttonSearchPath.Text = "button2";
            this.buttonSearchPath.UseVisualStyleBackColor = true;
            this.buttonSearchPath.Click += new System.EventHandler(this.ButtonSearchPath);
            // 
            // InitializationForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.buttonSearchPath);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxPath);
            this.Name = "InitializationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ButtonStartAnalysis(object sender, EventArgs e)
        {
            textBoxPath.Text = TestingStaticAnalyzator.Start(textBoxPath.Text);
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
    }
}
