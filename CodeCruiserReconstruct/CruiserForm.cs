using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Luschny.Utils.CodeCruiser
{
    public class CruiserForm : Form
    {
        private RadioButton asyCode;
        private GroupBox CodeGroup;
        private IContainer components;
        private Button ConvertButton;
        private Button Copyright;
        private RadioButton csCode;
        private Button DestButton;
        private GroupBox DestinationGroup;
        private Label DestinationLabel;
        private string destName;
        private string dirName;
        private Label dstHelp;
        private CheckBox EmbededCss;
        private OpenFileDialog fileDialog;
        private string fileName;
        private FolderBrowserDialog folderDialog;
        private RadioButton grCode;
        private RadioButton javaCode;
        private RadioButton jsCode;
        private CheckBox LineNumber;
        private CheckBox LinePrinterPaper;
        private RadioButton SaveToPoolDir;
        private RadioButton SaveToSourceDir;
        private RadioButton SelectDirectory;
        private RadioButton SelectDirTree;
        private RadioButton SelectFile;
        private Button SelectSource;
        private GroupBox SourceGroup;
        private Label SourceLabel;
        private Label srcHelp;
        private bool srcTypeIsDir;
        private Label styHelp;
        private GroupBox StyleOptions;
        private ToolTip toolTip;
        private RadioButton vbCode;

        internal CruiserForm()
        {
            this.InitializeComponent();
            this.srcTypeIsDir = true;
            this.toolTip.AutoPopDelay = 0x1f40;
            this.toolTip.SetToolTip(this.vbCode, "Visual Basic");
            this.toolTip.SetToolTip(this.csCode, "C# CSharp");
            this.toolTip.SetToolTip(this.jsCode, "JavaScript");
            this.toolTip.SetToolTip(this.asyCode, "Asymptotic");
            this.toolTip.SetToolTip(this.grCode, "Groovy");
            this.toolTip.SetToolTip(this.javaCode, "Java");
            this.toolTip.SetToolTip(this.srcHelp, Application.srcOptions);
            this.toolTip.SetToolTip(this.dstHelp, Application.dstOptions);
            this.toolTip.SetToolTip(this.styHelp, Application.formOptions);
            base.StartPosition = FormStartPosition.CenterScreen;
            this.folderDialog = new FolderBrowserDialog();
            this.fileDialog = new OpenFileDialog();
            this.fileDialog.DefaultExt = "*.java";
            this.fileDialog.Filter = "Java code (*.java)|*.java|CSharp code (*.cs)|*.cs|VB code (*.vb)|*.vb|JavaScript (*.js)|*.js|Groovy (*.groovy)|*.groovy|Asymptotic code (*.asy)|*.asy|PowerShell (*.ps1)|*.ps1|All files (*.*)|*.*";
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            if (!this.Panic())
            {
                CruiserState krass = new CruiserState
                {
                    sourcePath = this.SourceLabel.Text,
                    destinationPath = this.DestinationLabel.Text
                };
                if (this.SelectDirTree.Checked)
                {
                    krass.src = CruiserState.SRC.tree;
                }
                else if (this.SelectDirectory.Checked)
                {
                    krass.src = CruiserState.SRC.dir;
                }
                else
                {
                    krass.src = CruiserState.SRC.file;
                }
                if (this.SaveToSourceDir.Checked)
                {
                    krass.dst = CruiserState.DST.same;
                }
                else
                {
                    krass.dst = CruiserState.DST.pool;
                }
                if (this.csCode.Checked)
                {
                    krass.cod = CruiserState.COD.cs;
                }
                else if (this.vbCode.Checked)
                {
                    krass.cod = CruiserState.COD.vb;
                }
                else if (this.jsCode.Checked)
                {
                    krass.cod = CruiserState.COD.js;
                }
                else if (this.grCode.Checked)
                {
                    krass.cod = CruiserState.COD.groovy;
                }
                else if (this.asyCode.Checked)
                {
                    krass.cod = CruiserState.COD.asy;
                }
                else
                {
                    krass.cod = CruiserState.COD.java;
                }
                krass.sty = 0;
                if (this.LineNumber.Checked)
                {
                    krass.sty |= CruiserState.STY.line;
                }
                if (this.LinePrinterPaper.Checked)
                {
                    krass.sty |= CruiserState.STY.paper;
                }
                if (this.EmbededCss.Checked)
                {
                    krass.sty |= CruiserState.STY.embed;
                }
                this.ConvertButton.Enabled = false;
                this.SelectSource.Enabled = false;
                this.DestButton.Enabled = false;
                this.Copyright.Enabled = false;
                this.KrassWork(krass);
                this.ConvertButton.Enabled = true;
                this.SelectSource.Enabled = true;
                this.DestButton.Enabled = true;
                this.Copyright.Enabled = true;
            }
        }

        private void Copyright_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Application.CopyrightMsg, "C O P Y R I G H T", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void DestButton_Click(object sender, EventArgs e)
        {
            if (this.folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.destName = this.folderDialog.SelectedPath;
                this.DestinationLabel.Text = this.destName;
                this.SaveToPoolDir.Checked = true;
                base.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static bool FileHandler(TextWriter sw, out int count, string src, string dst, string searchPattern, bool dir, bool tree, bool sameDir)
        {
            StringBuilder builder = new StringBuilder(600);
            count = 0;
            try
            {
                if (dir)
                {
                    SearchOption searchOption = tree ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                    DirectoryInfo info = new DirectoryInfo(src);
                    foreach (FileInfo info2 in info.GetFiles(searchPattern, searchOption))
                    {
                        builder.Append("<file>\n<src>");
                        builder.Append(info2.FullName);
                        builder.Append("</src>\n<dst>");
                        if (sameDir)
                        {
                            builder.Append(info2.FullName);
                        }
                        else
                        {
                            builder.Append(Path.Combine(dst, info2.Name));
                            builder.Append("." + ((int)count).ToString(CultureInfo.InvariantCulture));
                        }
                        builder.Append(".html</dst>\n</file>\n");
                        count++;
                    }
                }
                else
                {
                    string fileName = Path.GetFileName(src);
                    builder.Append("<file>\n<src>");
                    builder.Append(src);
                    builder.Append("</src>\n<dst>");
                    builder.Append(Path.Combine(dst, fileName));
                    builder.Append(".html</dst>\n</file>\n");
                    count = 1;
                }
                builder.Append("</CodeCruiser>\n");
                sw.Write(builder.ToString());
            }
            catch (SecurityException exception)
            {
                MessageBox.Show(exception.Message, "Please exclude the offendig path from the source path!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return true;
            }
            catch (UnauthorizedAccessException exception2)
            {
                MessageBox.Show(exception2.Message, "Please exclude the offendig path from the source path!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return true;
            }
            catch (IOException exception3)
            {
                MessageBox.Show(exception3.Message, "P A N I C !", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return true;
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.SourceGroup = new GroupBox();
            this.srcHelp = new Label();
            this.SelectSource = new Button();
            this.SelectDirTree = new RadioButton();
            this.SelectDirectory = new RadioButton();
            this.SelectFile = new RadioButton();
            this.SourceLabel = new Label();
            this.DestinationGroup = new GroupBox();
            this.dstHelp = new Label();
            this.SaveToPoolDir = new RadioButton();
            this.SaveToSourceDir = new RadioButton();
            this.DestButton = new Button();
            this.ConvertButton = new Button();
            this.StyleOptions = new GroupBox();
            this.styHelp = new Label();
            this.EmbededCss = new CheckBox();
            this.LinePrinterPaper = new CheckBox();
            this.LineNumber = new CheckBox();
            this.DestinationLabel = new Label();
            this.CodeGroup = new GroupBox();
            this.asyCode = new RadioButton();
            this.jsCode = new RadioButton();
            this.grCode = new RadioButton();
            this.javaCode = new RadioButton();
            this.csCode = new RadioButton();
            this.vbCode = new RadioButton();
            this.Copyright = new Button();
            this.toolTip = new ToolTip(this.components);
            this.SourceGroup.SuspendLayout();
            this.DestinationGroup.SuspendLayout();
            this.StyleOptions.SuspendLayout();
            this.CodeGroup.SuspendLayout();
            base.SuspendLayout();
            this.SourceGroup.Controls.Add(this.srcHelp);
            this.SourceGroup.Controls.Add(this.SelectSource);
            this.SourceGroup.Controls.Add(this.SelectDirTree);
            this.SourceGroup.Controls.Add(this.SelectDirectory);
            this.SourceGroup.Controls.Add(this.SelectFile);
            this.SourceGroup.Location = new Point(0x1c, 0x19);
            this.SourceGroup.Name = "SourceGroup";
            this.SourceGroup.Size = new Size(0xbc, 0x94);
            this.SourceGroup.TabIndex = 0;
            this.SourceGroup.TabStop = false;
            this.SourceGroup.Text = "Source";
            this.srcHelp.AutoSize = true;
            this.srcHelp.Font = new Font("Microsoft Sans Serif", 10.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.srcHelp.ForeColor = SystemColors.HotTrack;
            this.srcHelp.Location = new Point(0x98, -5);
            this.srcHelp.Name = "srcHelp";
            this.srcHelp.Size = new Size(0x15, 0x18);
            this.srcHelp.TabIndex = 5;
            this.srcHelp.Text = "?";
            this.SelectSource.Location = new Point(0x13, 0x67);
            this.SelectSource.Name = "SelectSource";
            this.SelectSource.Size = new Size(0x86, 0x1c);
            this.SelectSource.TabIndex = 3;
            this.SelectSource.Text = "Select Source";
            this.SelectSource.UseVisualStyleBackColor = true;
            this.SelectSource.Click += new EventHandler(this.SrcButton_Click);
            this.SelectDirTree.AutoSize = true;
            this.SelectDirTree.Location = new Point(0x13, 0x4c);
            this.SelectDirTree.Name = "SelectDirTree";
            this.SelectDirTree.Size = new Size(0x66, 0x15);
            this.SelectDirTree.TabIndex = 2;
            this.SelectDirTree.Text = "Select Tree";
            this.SelectDirTree.UseVisualStyleBackColor = true;
            this.SelectDirTree.CheckedChanged += new EventHandler(this.SelectDirTree_CheckedChanged);
            this.SelectDirectory.AutoSize = true;
            this.SelectDirectory.Checked = true;
            this.SelectDirectory.Location = new Point(0x13, 0x31);
            this.SelectDirectory.Name = "SelectDirectory";
            this.SelectDirectory.Size = new Size(0x70, 0x15);
            this.SelectDirectory.TabIndex = 1;
            this.SelectDirectory.TabStop = true;
            this.SelectDirectory.Text = "Select Folder";
            this.SelectDirectory.UseVisualStyleBackColor = true;
            this.SelectDirectory.CheckedChanged += new EventHandler(this.SelectDirectory_CheckedChanged);
            this.SelectFile.AutoSize = true;
            this.SelectFile.Location = new Point(0x13, 0x16);
            this.SelectFile.Name = "SelectFile";
            this.SelectFile.Size = new Size(0x5e, 0x15);
            this.SelectFile.TabIndex = 0;
            this.SelectFile.Text = "Select File";
            this.SelectFile.UseVisualStyleBackColor = true;
            this.SelectFile.CheckedChanged += new EventHandler(this.SelectFile_CheckedChanged);
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Location = new Point(0xd5, 0xbf);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Size = new Size(0, 0x11);
            this.SourceLabel.TabIndex = 4;
            this.DestinationGroup.Controls.Add(this.dstHelp);
            this.DestinationGroup.Controls.Add(this.SaveToPoolDir);
            this.DestinationGroup.Controls.Add(this.SaveToSourceDir);
            this.DestinationGroup.Controls.Add(this.DestButton);
            this.DestinationGroup.Location = new Point(0xf4, 0x19);
            this.DestinationGroup.Name = "DestinationGroup";
            this.DestinationGroup.Size = new Size(0xbc, 0x94);
            this.DestinationGroup.TabIndex = 1;
            this.DestinationGroup.TabStop = false;
            this.DestinationGroup.Text = "Destination";
            this.dstHelp.AutoSize = true;
            this.dstHelp.Font = new Font("Microsoft Sans Serif", 10.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.dstHelp.ForeColor = SystemColors.HotTrack;
            this.dstHelp.Location = new Point(0x98, -5);
            this.dstHelp.Name = "dstHelp";
            this.dstHelp.Size = new Size(0x15, 0x18);
            this.dstHelp.TabIndex = 4;
            this.dstHelp.Text = "?";
            this.SaveToPoolDir.AutoSize = true;
            this.SaveToPoolDir.Location = new Point(0x13, 0x43);
            this.SaveToPoolDir.Name = "SaveToPoolDir";
            this.SaveToPoolDir.Size = new Size(150, 0x15);
            this.SaveToPoolDir.TabIndex = 3;
            this.SaveToPoolDir.Text = "Save to DestFolder";
            this.SaveToPoolDir.UseVisualStyleBackColor = true;
            this.SaveToPoolDir.CheckedChanged += new EventHandler(this.SaveToPoolDir_CheckedChanged);
            this.SaveToSourceDir.AutoSize = true;
            this.SaveToSourceDir.Checked = true;
            this.SaveToSourceDir.Location = new Point(0x13, 0x1f);
            this.SaveToSourceDir.Name = "SaveToSourceDir";
            this.SaveToSourceDir.Size = new Size(0xa6, 0x15);
            this.SaveToSourceDir.TabIndex = 2;
            this.SaveToSourceDir.TabStop = true;
            this.SaveToSourceDir.Text = "Save to SourceFolder";
            this.SaveToSourceDir.UseVisualStyleBackColor = true;
            this.SaveToSourceDir.CheckedChanged += new EventHandler(this.SaveToSourceDir_CheckedChanged);
            this.DestButton.Location = new Point(0x13, 0x67);
            this.DestButton.Name = "DestButton";
            this.DestButton.Size = new Size(0x86, 0x1c);
            this.DestButton.TabIndex = 1;
            this.DestButton.Text = "Select Destination";
            this.DestButton.UseVisualStyleBackColor = true;
            this.DestButton.Click += new EventHandler(this.DestButton_Click);
            this.ConvertButton.Location = new Point(0x29, 0xbf);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new Size(140, 0x2d);
            this.ConvertButton.TabIndex = 2;
            this.ConvertButton.Text = "Convert";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new EventHandler(this.ConvertButton_Click);
            this.StyleOptions.Controls.Add(this.styHelp);
            this.StyleOptions.Controls.Add(this.EmbededCss);
            this.StyleOptions.Controls.Add(this.LinePrinterPaper);
            this.StyleOptions.Controls.Add(this.LineNumber);
            this.StyleOptions.Location = new Point(0x1c9, 0x19);
            this.StyleOptions.Name = "StyleOptions";
            this.StyleOptions.Size = new Size(0xbc, 0x94);
            this.StyleOptions.TabIndex = 5;
            this.StyleOptions.TabStop = false;
            this.StyleOptions.Text = "Style";
            this.styHelp.AutoSize = true;
            this.styHelp.Font = new Font("Microsoft Sans Serif", 10.8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.styHelp.ForeColor = SystemColors.HotTrack;
            this.styHelp.Location = new Point(0x98, -5);
            this.styHelp.Name = "styHelp";
            this.styHelp.Size = new Size(0x15, 0x18);
            this.styHelp.TabIndex = 3;
            this.styHelp.Text = "?";
            this.EmbededCss.AutoSize = true;
            this.EmbededCss.Location = new Point(0x18, 0x67);
            this.EmbededCss.Name = "EmbededCss";
            this.EmbededCss.Size = new Size(0x9e, 0x15);
            this.EmbededCss.TabIndex = 2;
            this.EmbededCss.Text = "EmbededStyleSheet";
            this.EmbededCss.UseVisualStyleBackColor = true;
            this.LinePrinterPaper.AutoSize = true;
            this.LinePrinterPaper.Checked = true;
            this.LinePrinterPaper.CheckState = CheckState.Checked;
            this.LinePrinterPaper.Location = new Point(0x18, 0x43);
            this.LinePrinterPaper.Name = "LinePrinterPaper";
            this.LinePrinterPaper.Size = new Size(0x89, 0x15);
            this.LinePrinterPaper.TabIndex = 1;
            this.LinePrinterPaper.Text = "LinePrinterPaper";
            this.LinePrinterPaper.UseVisualStyleBackColor = true;
            this.LineNumber.AutoSize = true;
            this.LineNumber.Checked = true;
            this.LineNumber.CheckState = CheckState.Checked;
            this.LineNumber.Location = new Point(0x18, 0x1f);
            this.LineNumber.Name = "LineNumber";
            this.LineNumber.Size = new Size(0x6b, 0x15);
            this.LineNumber.TabIndex = 0;
            this.LineNumber.Text = "LineNumber";
            this.LineNumber.UseVisualStyleBackColor = true;
            this.DestinationLabel.AutoSize = true;
            this.DestinationLabel.Location = new Point(0xd5, 0xdb);
            this.DestinationLabel.Name = "DestinationLabel";
            this.DestinationLabel.Size = new Size(0, 0x11);
            this.DestinationLabel.TabIndex = 6;
            this.CodeGroup.Controls.Add(this.asyCode);
            this.CodeGroup.Controls.Add(this.jsCode);
            this.CodeGroup.Controls.Add(this.grCode);
            this.CodeGroup.Controls.Add(this.javaCode);
            this.CodeGroup.Controls.Add(this.csCode);
            this.CodeGroup.Controls.Add(this.vbCode);
            this.CodeGroup.Location = new Point(0x29c, 0x19);
            this.CodeGroup.Name = "CodeGroup";
            this.CodeGroup.Size = new Size(0xbc, 0x94);
            this.CodeGroup.TabIndex = 7;
            this.CodeGroup.TabStop = false;
            this.CodeGroup.Text = "Code";
            this.asyCode.AutoSize = true;
            this.asyCode.Location = new Point(0x6d, 0x66);
            this.asyCode.Name = "asyCode";
            this.asyCode.Size = new Size(60, 0x15);
            this.asyCode.TabIndex = 5;
            this.asyCode.TabStop = true;
            this.asyCode.Text = "ASY";
            this.asyCode.UseVisualStyleBackColor = true;
            this.jsCode.AutoSize = true;
            this.jsCode.Location = new Point(0x6d, 0x43);
            this.jsCode.Name = "jsCode";
            this.jsCode.Size = new Size(0x2d, 0x15);
            this.jsCode.TabIndex = 4;
            this.jsCode.TabStop = true;
            this.jsCode.Text = "JS";
            this.jsCode.UseVisualStyleBackColor = true;
            this.grCode.AutoSize = true;
            this.grCode.Location = new Point(0x6d, 0x1f);
            this.grCode.Name = "grCode";
            this.grCode.Size = new Size(50, 0x15);
            this.grCode.TabIndex = 3;
            this.grCode.TabStop = true;
            this.grCode.Text = "GR";
            this.grCode.UseVisualStyleBackColor = true;
            this.javaCode.AutoSize = true;
            this.javaCode.Checked = true;
            this.javaCode.Location = new Point(0x11, 0x66);
            this.javaCode.Name = "javaCode";
            this.javaCode.Size = new Size(0x3f, 0x15);
            this.javaCode.TabIndex = 2;
            this.javaCode.TabStop = true;
            this.javaCode.Text = "JAVA";
            this.javaCode.UseVisualStyleBackColor = true;
            this.csCode.AutoSize = true;
            this.csCode.Location = new Point(0x11, 0x42);
            this.csCode.Name = "csCode";
            this.csCode.Size = new Size(0x2f, 0x15);
            this.csCode.TabIndex = 1;
            this.csCode.Text = "CS";
            this.csCode.UseVisualStyleBackColor = true;
            this.vbCode.AutoSize = true;
            this.vbCode.Location = new Point(0x11, 0x1f);
            this.vbCode.Name = "vbCode";
            this.vbCode.Size = new Size(0x2f, 0x15);
            this.vbCode.TabIndex = 0;
            this.vbCode.Text = "VB";
            this.vbCode.UseVisualStyleBackColor = true;
            this.Copyright.BackColor = SystemColors.HotTrack;
            this.Copyright.Location = new Point(12, 8);
            this.Copyright.Name = "Copyright";
            this.Copyright.Size = new Size(0x10, 0x10);
            this.Copyright.TabIndex = 8;
            this.Copyright.TextAlign = ContentAlignment.TopCenter;
            this.Copyright.UseVisualStyleBackColor = false;
            this.Copyright.Click += new EventHandler(this.Copyright_Click);
            base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x376, 0x110);
            base.Controls.Add(this.Copyright);
            base.Controls.Add(this.CodeGroup);
            base.Controls.Add(this.DestinationLabel);
            base.Controls.Add(this.StyleOptions);
            base.Controls.Add(this.SourceLabel);
            base.Controls.Add(this.ConvertButton);
            base.Controls.Add(this.DestinationGroup);
            base.Controls.Add(this.SourceGroup);
            base.Name = "CruiserForm";
            this.Text = "CC Code Cruiser";
            this.SourceGroup.ResumeLayout(false);
            this.SourceGroup.PerformLayout();
            this.DestinationGroup.ResumeLayout(false);
            this.DestinationGroup.PerformLayout();
            this.StyleOptions.ResumeLayout(false);
            this.StyleOptions.PerformLayout();
            this.CodeGroup.ResumeLayout(false);
            this.CodeGroup.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void KrassWork(CruiserState krass)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = folderPath + @"\CoolCodeList.xml";
            string str3 = (krass.dst == CruiserState.DST.same) ? folderPath : krass.destinationPath;
            StringBuilder builder = new StringBuilder(300);
            bool flag = false;
            int count = 0;
            Color backColor = this.ConvertButton.BackColor;
            this.ConvertButton.BackColor = Color.FromArgb(0x99, 0xcc, 0xff);
            this.ConvertButton.Text = string.Format(CultureInfo.InvariantCulture, "Scanning ...", new object[0]);
            this.ConvertButton.Refresh();
            using (StreamWriter writer = new StreamWriter(path))
            {
                builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n");
                builder.Append("<CodeCruiser>\n");
                writer.Write(builder.ToString());
                if (Directory.Exists(krass.sourcePath))
                {
                    flag = FileHandler(writer, out count, krass.sourcePath, krass.destinationPath, "*." + krass.cod.ToString(), true, krass.src == CruiserState.SRC.tree, krass.dst == CruiserState.DST.same);
                }
                else
                {
                    flag = FileHandler(writer, out count, krass.sourcePath, krass.destinationPath, "", false, false, false);
                }
            }
            if (!flag)
            {
                if (count > 0)
                {
                    this.ConvertButton.Text = string.Format(CultureInfo.InvariantCulture, "C o n v e r t i n g \n {0} f i l e (s)", new object[] { count });
                    this.ConvertButton.Refresh();
                    IndexFile index = new IndexFile();
                    try
                    {
                        new XHtmlWriter(krass.cod.ToString(), (krass.sty & CruiserState.STY.paper) != 0, (krass.sty & CruiserState.STY.line) != 0, (krass.sty & CruiserState.STY.embed) != 0).Transform(path, index, new CallBack(this.ConvertButton, count));
                    }
                    catch (Exception exception)
                    {
                        index.Save(str3 + @"\__index.html");
                        MessageBox.Show(exception.Message, "P A N I C !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if (!flag)
                    {
                        string destDir = str3 + @"\__index.html";
                        string src = index.Save(destDir);
                        destDir = str3 + @"\_index.html";
                        CodeFrame.WriteFrame(destDir, src);
                        if (MessageBox.Show(count + " file(s) processed!\nDo you want to preview the files?\n", "S U C C E S S !", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            new BrowserForm(destDir).Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No files of given type found!", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            this.ConvertButton.BackColor = backColor;
            this.ConvertButton.Text = "Convert";
            this.ConvertButton.Refresh();
        }

        private bool Panic()
        {
            if ((this.SourceLabel.Text == null) || (!File.Exists(this.SourceLabel.Text) && !Directory.Exists(this.SourceLabel.Text)))
            {
                MessageBox.Show("Source file/directory is not valid!", "P A N I C !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            if (File.Exists(this.SourceLabel.Text))
            {
                this.setCodeRadioButton(this.SourceLabel.Text);
            }
            if ((this.DestinationLabel.Text != null) && Directory.Exists(this.DestinationLabel.Text))
            {
                return false;
            }
            MessageBox.Show("Destination directory is not valid!", "P A N I C !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return true;
        }

        private void SaveToPoolDir_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked && (this.fileName != null))
            {
                this.DestinationLabel.Text = this.destName;
                this.SaveToPoolDir.Checked = true;
            }
        }

        private void SaveToSourceDir_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                if (this.SelectFile.Checked)
                {
                    if (this.fileName != null)
                    {
                        this.dirName = Path.GetDirectoryName(this.fileName);
                        this.destName = this.dirName;
                        this.DestinationLabel.Text = this.dirName;
                    }
                }
                else if (this.dirName != null)
                {
                    this.destName = this.dirName;
                    this.DestinationLabel.Text = this.dirName;
                }
            }
        }

        private void SelectDir(RadioButton b)
        {
            if (b.Checked && !this.srcTypeIsDir)
            {
                if (this.fileName != null)
                {
                    this.dirName = Path.GetDirectoryName(this.fileName);
                    this.SourceLabel.Text = this.dirName;
                }
                else
                {
                    this.SourceLabel.Text = "";
                }
                this.srcTypeIsDir = true;
            }
        }

        private void SelectDirectory_CheckedChanged(object sender, EventArgs e)
        {
            this.SelectDir(sender as RadioButton);
        }

        private void SelectDirTree_CheckedChanged(object sender, EventArgs e)
        {
            this.SelectDir(sender as RadioButton);
        }

        private void SelectFile_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                this.srcTypeIsDir = false;
                this.fileName = null;
                this.SourceLabel.Text = "";
            }
        }

        private void SelectFileDialog(object sender, EventArgs e)
        {
            this.setFileDefaultExt();
            if (this.fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.fileName = this.fileDialog.FileName;
            }
        }

        private void SelectFolderDialog(object sender, EventArgs e)
        {
            if (this.folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.dirName = this.folderDialog.SelectedPath;
            }
        }

        private void setCodeRadioButton(string fileName)
        {
            if (fileName != null)
            {
                string str = Path.GetExtension(fileName).TrimStart(new char[] { '.' });
                if (str == CruiserState.COD.java.ToString())
                {
                    this.javaCode.Checked = true;
                }
                else if (str == CruiserState.COD.cs.ToString())
                {
                    this.csCode.Checked = true;
                }
                else if (str == CruiserState.COD.vb.ToString())
                {
                    this.vbCode.Checked = true;
                }
                else if (str == CruiserState.COD.js.ToString())
                {
                    this.jsCode.Checked = true;
                }
                else if (str == CruiserState.COD.asy.ToString())
                {
                    this.asyCode.Checked = true;
                }
                else if (str == CruiserState.COD.groovy.ToString())
                {
                    this.grCode.Checked = true;
                }
            }
        }

        private void setFileDefaultExt()
        {
            if (this.javaCode.Checked)
            {
                this.fileDialog.DefaultExt = "*.java";
            }
            else if (this.csCode.Checked)
            {
                this.fileDialog.DefaultExt = "*.cs";
            }
            else if (this.vbCode.Checked)
            {
                this.fileDialog.DefaultExt = "*.vb";
            }
            else if (this.jsCode.Checked)
            {
                this.fileDialog.DefaultExt = "*.js";
            }
            else if (this.asyCode.Checked)
            {
                this.fileDialog.DefaultExt = "*.asy";
            }
            else if (this.grCode.Checked)
            {
                this.fileDialog.DefaultExt = "*.groovy";
            }
        }

        private void SrcButton_Click(object sender, EventArgs e)
        {
            if (this.SelectFile.Checked)
            {
                this.SelectFileDialog(this, e);
                if (this.SaveToSourceDir.Checked)
                {
                    this.dirName = Path.GetDirectoryName(this.fileName);
                    this.DestinationLabel.Text = this.dirName;
                }
                this.SourceLabel.Text = this.fileName;
                this.setCodeRadioButton(this.fileName);
            }
            else
            {
                this.SelectFolderDialog(sender, e);
                this.fileName = this.dirName;
                this.SourceLabel.Text = this.fileName;
                if (this.SaveToSourceDir.Checked)
                {
                    this.destName = this.dirName;
                    this.DestinationLabel.Text = this.dirName;
                }
            }
        }

        public class CallBack
        {
            private Button button;
            private int count;

            public CallBack(Button button, int count)
            {
                this.button = button;
                this.count = count;
            }

            public void Call()
            {
                this.count--;
                if ((this.count % 7) == 0)
                {
                    this.button.Text = string.Format(CultureInfo.InvariantCulture, "C o n v e r t i n g \n {0} f i l e (s)", new object[] { this.count });
                    this.button.Refresh();
                }
            }
        }
    }
} 
