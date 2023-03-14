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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    public partial class Program : Form
    {
        private Button button1;
        private TextBox textBox1;

        public Program()
        { 
            InitializeComponent();
        }

        static void Main(string[] args)
        {
            Application.Run(new Program());
        }

        public string Start()
        {
            string solutionPath = @"C:\Works\ConsoleApp1\ConsoleApp1.sln";
            string logPath = "";

            StringBuilder warnings = new StringBuilder();

            const string warningMessageFormat =
              "'if' with equal 'then' and 'else' blocks is found in file {0} at line {1}";

            MSBuildLocator.RegisterDefaults();
            using (var workspace = MSBuildWorkspace.Create())
            {
                Project currProject = GetProjectFromSolution(solutionPath, workspace);
                foreach (var document in currProject.Documents)
                {
                    var tree = document.GetSyntaxTreeAsync().Result;
                    var ifStatementNodes = tree.GetRoot()
                                               .DescendantNodesAndSelf()
                                               .OfType<IfStatementSyntax>();
                    var correctA = tree.GetRoot()
                                       .DescendantNodesAndSelf()
                                       .OfType<VariableDeclarationSyntax>();


                    foreach (var ifStatement in ifStatementNodes)
                    {
                        if (ApplyRule(ifStatement))
                        {
                            int lineNumber = ifStatement.GetLocation()
                                                        .GetLineSpan()
                                                        .StartLinePosition.Line + 1;

                            warnings.AppendLine(String.Format(warningMessageFormat,
                                                              document.FilePath,
                                                              lineNumber));
                        }
                    }
                }

                if (warnings.Capacity != 0)
                {
                    return textBox1.Text = "1";
                }
                else
                {
                    return textBox1.Text = "2";
                }

            }
            
        }

        static Project GetProjectFromSolution(String solutionPath,
                                      MSBuildWorkspace workspace)
        {
            //MSBuildLocator.RegisterDefaults();
            Solution currSolution = workspace.OpenSolutionAsync(solutionPath)
                                             .Result;
            return currSolution.Projects.Single();
        }

        static bool ApplyRule(IfStatementSyntax ifStatement)
        {
            if (ifStatement?.Else == null)
                return false;

            StatementSyntax thenBody = ifStatement.Statement;
            StatementSyntax elseBody = ifStatement.Else.Statement;

            return SyntaxFactory.AreEquivalent(thenBody, elseBody);
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Program
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Program";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Start();

        }
    }
}
