using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly Form settingsForm;
        
        public SettingsForm()
        {
            InitializeComponent();
            settingsForm = this;
            
            foreach(var ruleName in Properties.Settings.Default.PropertyValues)
            {
                var newElement = (SettingsPropertyValue)ruleName;
                var stringCheckList = checkedListBox1.Items.Add(newElement.Name);
                if ((bool)newElement.PropertyValue == true)
                {
                    checkedListBox1.SetItemChecked(stringCheckList, true);
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonAbortSettings = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
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
            // buttonAbortSettings
            // 
            this.buttonAbortSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbortSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonAbortSettings.Location = new System.Drawing.Point(197, 12);
            this.buttonAbortSettings.Name = "buttonAbortSettings";
            this.buttonAbortSettings.Size = new System.Drawing.Size(75, 26);
            this.buttonAbortSettings.TabIndex = 1;
            this.buttonAbortSettings.Text = "Отмена";
            this.buttonAbortSettings.UseVisualStyleBackColor = true;
            this.buttonAbortSettings.Click += new System.EventHandler(this.ButtonAbortSettings);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSaveSettings.Location = new System.Drawing.Point(12, 12);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 26);
            this.buttonSaveSettings.TabIndex = 0;
            this.buttonSaveSettings.Text = "Сохранить";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.ButtonSaveSettings);
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
            this.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "SettingsForm";
            this.Text = "Настройки правил";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ButtonSaveSettings(object sender, EventArgs e)
        {
            List<string> listNotSelected = new List<string>();
            foreach (var item in checkedListBox1.Items)
            {
                listNotSelected.Add(item.ToString());
            }
          
            foreach (var nameFromEnum in checkedListBox1.CheckedItems)
            {
                foreach (var rule in Properties.Settings.Default.PropertyValues)
                {
                    var currentRule = (SettingsPropertyValue)rule;
                    if (nameFromEnum.ToString() == currentRule.Name)
                    {
                        foreach (var item in Enum.GetValues(typeof(SettingsRules.NamesErrors)))
                        {
                            if (item.ToString() == currentRule.Name)
                            {
                                SettingsRules.SetDictionary((SettingsRules.NamesErrors)item, true);
                            }
                        }
                        currentRule.PropertyValue = true;
                        listNotSelected.Remove(currentRule.Name);
                        break;
                    }
                }
            }

            if (listNotSelected.Count != 0)
            {
                foreach (var nameFromEnum in listNotSelected)
                {
                    foreach (var rule in Properties.Settings.Default.PropertyValues)
                    {
                        var currentRule = (SettingsPropertyValue)rule;
                        if (nameFromEnum == currentRule.Name)
                        {
                            foreach (var item in Enum.GetValues(typeof(SettingsRules.NamesErrors)))
                            {
                                if (item.ToString() == currentRule.Name)
                                {
                                    SettingsRules.SetDictionary((SettingsRules.NamesErrors)item, false);
                                }
                            }
                            currentRule.PropertyValue = false;
                            break;
                        }
                    }
                }
            }
            
            Properties.Settings.Default.Save();
            CloseWindowSettigs();
            settingsForm.Close();
        }

        private void ButtonAbortSettings(object sender, EventArgs e)
        {
            CloseWindowSettigs();
            settingsForm.Close();
        }

        private void CloseWindowSettigs()
        {
            mainForm = InitializationForm.GetMainForm();
            mainForm.Enabled = true;
            mainForm.Show();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseWindowSettigs();
        }
    }
}