using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    public partial class SettingsForm : Form
    {
        private SplitContainer splitContainer1;
        private Button buttonAbortSettings;
        private Button buttonSaveSettings;
        private CheckedListBox checkedListBox1;
        private Form mainForm;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonAbortSettings = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonAbortSettings);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSaveSettings);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkedListBox1);
            this.splitContainer1.Size = new System.Drawing.Size(284, 261);
            this.splitContainer1.SplitterDistance = 54;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(12, 12);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSettings.TabIndex = 0;
            this.buttonSaveSettings.Text = "Сохранить";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.ButtonSaveSettings);
            // 
            // buttonAbortSettings
            // 
            this.buttonAbortSettings.Location = new System.Drawing.Point(197, 12);
            this.buttonAbortSettings.Name = "buttonAbortSettings";
            this.buttonAbortSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonAbortSettings.TabIndex = 1;
            this.buttonAbortSettings.Text = "Отмена";
            this.buttonAbortSettings.UseVisualStyleBackColor = true;
            this.buttonAbortSettings.Click += new System.EventHandler(this.ButtonAbortSettings);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(284, 203);
            this.checkedListBox1.TabIndex = 0;
            // 
            // SettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SettingsForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ButtonSaveSettings(object sender, EventArgs e)
        {

        }

        private void ButtonAbortSettings(object sender, EventArgs e)
        {
            mainForm = InitializationForm.GetMainForm();   
            //this.Close();
            mainForm.Enabled = true;
            mainForm.Show();
            
        }


    }
}
