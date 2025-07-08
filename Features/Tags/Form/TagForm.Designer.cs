namespace T24AddIn.Features.Tags.Form
{
    partial class TagForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TagDoorBtn = new Button();
            AddPropertiesBtn = new Button();
            TagWindowBtn = new Button();
            GenerateScheduleBtn = new Button();
            TagWallBtn = new Button();
            ExitBtn = new Button();
            K24Tool = new Label();
            button1 = new Button();
            colorDialog1 = new ColorDialog();
            PropSelect = new ComboBox();
            TagType = new ComboBox();
            SelectColorBtn = new Button();
            panel1 = new Panel();
            label7 = new Label();
            ScheduleTagTypeComboBox = new ComboBox();
            label6 = new Label();
            label4 = new Label();
            ViewComboBox = new ComboBox();
            label3 = new Label();
            ScheduleGroupComboBox = new ComboBox();
            panel2 = new Panel();
            label5 = new Label();
            label2 = new Label();
            label1 = new Label();
            TagCurtainWallBtn = new Button();
            GrossWallBtn = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // TagDoorBtn
            // 
            TagDoorBtn.Location = new Point(44, 81);
            TagDoorBtn.Margin = new Padding(4, 3, 4, 3);
            TagDoorBtn.Name = "TagDoorBtn";
            TagDoorBtn.Size = new Size(153, 43);
            TagDoorBtn.TabIndex = 0;
            TagDoorBtn.Text = "Tag External Doors";
            TagDoorBtn.UseVisualStyleBackColor = true;
            TagDoorBtn.Click += TagDoorBtn_Click;
            // 
            // AddPropertiesBtn
            // 
            AddPropertiesBtn.Location = new Point(260, 81);
            AddPropertiesBtn.Margin = new Padding(4, 3, 4, 3);
            AddPropertiesBtn.Name = "AddPropertiesBtn";
            AddPropertiesBtn.Size = new Size(153, 43);
            AddPropertiesBtn.TabIndex = 1;
            AddPropertiesBtn.Text = "Add Properties";
            AddPropertiesBtn.UseVisualStyleBackColor = true;
            AddPropertiesBtn.Click += AddPropertiesBtn_Click;
            // 
            // TagWindowBtn
            // 
            TagWindowBtn.Location = new Point(260, 147);
            TagWindowBtn.Margin = new Padding(4, 3, 4, 3);
            TagWindowBtn.Name = "TagWindowBtn";
            TagWindowBtn.Size = new Size(153, 43);
            TagWindowBtn.TabIndex = 2;
            TagWindowBtn.Text = "Tag External Windows";
            TagWindowBtn.UseVisualStyleBackColor = true;
            TagWindowBtn.Click += TagWindowBtn_Click;
            // 
            // GenerateScheduleBtn
            // 
            GenerateScheduleBtn.Location = new Point(99, 252);
            GenerateScheduleBtn.Margin = new Padding(4, 3, 4, 3);
            GenerateScheduleBtn.Name = "GenerateScheduleBtn";
            GenerateScheduleBtn.Size = new Size(136, 43);
            GenerateScheduleBtn.TabIndex = 3;
            GenerateScheduleBtn.Text = "Generate Schedules";
            GenerateScheduleBtn.UseVisualStyleBackColor = true;
            GenerateScheduleBtn.Click += GenerateScheduleBtn_Click;
            // 
            // TagWallBtn
            // 
            TagWallBtn.Location = new Point(44, 147);
            TagWallBtn.Margin = new Padding(4, 3, 4, 3);
            TagWallBtn.Name = "TagWallBtn";
            TagWallBtn.Size = new Size(153, 43);
            TagWallBtn.TabIndex = 4;
            TagWallBtn.Text = "Tag External Walls";
            TagWallBtn.UseVisualStyleBackColor = true;
            TagWallBtn.Click += TagWallBtn_Click;
            // 
            // ExitBtn
            // 
            ExitBtn.BackColor = Color.Transparent;
            ExitBtn.Location = new Point(465, 115);
            ExitBtn.Margin = new Padding(4, 3, 4, 3);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new Size(316, 43);
            ExitBtn.TabIndex = 5;
            ExitBtn.Text = "Import Tags";
            ExitBtn.UseVisualStyleBackColor = false;
            ExitBtn.Click += ExitBtn_Click;
            // 
            // K24Tool
            // 
            K24Tool.AutoSize = true;
            K24Tool.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            K24Tool.Location = new Point(244, 22);
            K24Tool.Margin = new Padding(4, 0, 4, 0);
            K24Tool.Name = "K24Tool";
            K24Tool.Size = new Size(211, 26);
            K24Tool.TabIndex = 6;
            K24Tool.Text = "K2D T24 Revit Tools";
            // 
            // button1
            // 
            button1.Location = new Point(29, 252);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(136, 43);
            button1.TabIndex = 7;
            button1.Text = "Update Color";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // PropSelect
            // 
            PropSelect.FormattingEnabled = true;
            PropSelect.Location = new Point(134, 80);
            PropSelect.Margin = new Padding(4, 3, 4, 3);
            PropSelect.Name = "PropSelect";
            PropSelect.Size = new Size(207, 23);
            PropSelect.TabIndex = 8;
            PropSelect.SelectedIndexChanged += PropSelect_SelectedIndexChanged;
            // 
            // TagType
            // 
            TagType.FormattingEnabled = true;
            TagType.Location = new Point(134, 140);
            TagType.Margin = new Padding(4, 3, 4, 3);
            TagType.Name = "TagType";
            TagType.Size = new Size(207, 23);
            TagType.TabIndex = 9;
            TagType.SelectedIndexChanged += TagType_SelectedIndexChanged;
            // 
            // SelectColorBtn
            // 
            SelectColorBtn.Location = new Point(205, 252);
            SelectColorBtn.Margin = new Padding(4, 3, 4, 3);
            SelectColorBtn.Name = "SelectColorBtn";
            SelectColorBtn.Size = new Size(136, 43);
            SelectColorBtn.TabIndex = 10;
            SelectColorBtn.Text = "Select Coolor";
            SelectColorBtn.UseVisualStyleBackColor = true;
            SelectColorBtn.Click += button2_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label7);
            panel1.Controls.Add(ScheduleTagTypeComboBox);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(ViewComboBox);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(ScheduleGroupComboBox);
            panel1.Controls.Add(GenerateScheduleBtn);
            panel1.Location = new Point(465, 290);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(316, 324);
            panel1.TabIndex = 11;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(20, 143);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(52, 15);
            label7.TabIndex = 16;
            label7.Text = "Tag Type";
            // 
            // ScheduleTagTypeComboBox
            // 
            ScheduleTagTypeComboBox.FormattingEnabled = true;
            ScheduleTagTypeComboBox.Location = new Point(86, 140);
            ScheduleTagTypeComboBox.Margin = new Padding(4, 3, 4, 3);
            ScheduleTagTypeComboBox.Name = "ScheduleTagTypeComboBox";
            ScheduleTagTypeComboBox.Size = new Size(201, 23);
            ScheduleTagTypeComboBox.TabIndex = 15;
            ScheduleTagTypeComboBox.SelectedIndexChanged += ScheduleTagTypeComboBox_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(104, 27);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(103, 26);
            label6.TabIndex = 14;
            label6.Text = "Schedule";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 190);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(32, 15);
            label4.TabIndex = 14;
            label4.Text = "View";
            // 
            // ViewComboBox
            // 
            ViewComboBox.FormattingEnabled = true;
            ViewComboBox.Location = new Point(86, 187);
            ViewComboBox.Margin = new Padding(4, 3, 4, 3);
            ViewComboBox.Name = "ViewComboBox";
            ViewComboBox.Size = new Size(201, 23);
            ViewComboBox.TabIndex = 13;
            ViewComboBox.SelectedIndexChanged += ViewComboBox_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 83);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 12;
            label3.Text = "Group";
            // 
            // ScheduleGroupComboBox
            // 
            ScheduleGroupComboBox.FormattingEnabled = true;
            ScheduleGroupComboBox.Location = new Point(86, 83);
            ScheduleGroupComboBox.Margin = new Padding(4, 3, 4, 3);
            ScheduleGroupComboBox.Name = "ScheduleGroupComboBox";
            ScheduleGroupComboBox.Size = new Size(201, 23);
            ScheduleGroupComboBox.TabIndex = 9;
            ScheduleGroupComboBox.SelectedIndexChanged += ScheduleGroupComboBox_SelectedIndexChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(PropSelect);
            panel2.Controls.Add(TagType);
            panel2.Controls.Add(SelectColorBtn);
            panel2.Controls.Add(button1);
            panel2.Location = new Point(44, 290);
            panel2.Margin = new Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(369, 324);
            panel2.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(150, 27);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(64, 26);
            label5.TabIndex = 13;
            label5.Text = "Color";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 143);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 12;
            label2.Text = "Tag Type";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 83);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(40, 15);
            label1.TabIndex = 11;
            label1.Text = "Group";
            // 
            // TagCurtainWallBtn
            // 
            TagCurtainWallBtn.Location = new Point(44, 213);
            TagCurtainWallBtn.Margin = new Padding(4, 3, 4, 3);
            TagCurtainWallBtn.Name = "TagCurtainWallBtn";
            TagCurtainWallBtn.Size = new Size(153, 43);
            TagCurtainWallBtn.TabIndex = 13;
            TagCurtainWallBtn.Text = "Tag Curtain Walls";
            TagCurtainWallBtn.UseVisualStyleBackColor = true;
            TagCurtainWallBtn.Click += TagCurtainWallBtn_Click;
            // 
            // GrossWallBtn
            // 
            GrossWallBtn.Location = new Point(260, 213);
            GrossWallBtn.Margin = new Padding(4, 3, 4, 3);
            GrossWallBtn.Name = "GrossWallBtn";
            GrossWallBtn.Size = new Size(153, 43);
            GrossWallBtn.TabIndex = 14;
            GrossWallBtn.Text = "Calculate Gross Wall Area";
            GrossWallBtn.UseVisualStyleBackColor = true;
            GrossWallBtn.Click += GrossWallBtn_Click;
            // 
            // TagForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(824, 646);
            Controls.Add(GrossWallBtn);
            Controls.Add(TagCurtainWallBtn);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(K24Tool);
            Controls.Add(ExitBtn);
            Controls.Add(TagWallBtn);
            Controls.Add(TagWindowBtn);
            Controls.Add(AddPropertiesBtn);
            Controls.Add(TagDoorBtn);
            Margin = new Padding(4, 3, 4, 3);
            Name = "TagForm";
            Text = "T24 Tevit Tool";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TagDoorBtn;
        private System.Windows.Forms.Button AddPropertiesBtn;
        private System.Windows.Forms.Button TagWindowBtn;
        private System.Windows.Forms.Button GenerateScheduleBtn;
        private System.Windows.Forms.Button TagWallBtn;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Label K24Tool;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox PropSelect;
        private System.Windows.Forms.ComboBox TagType;
        private System.Windows.Forms.Button SelectColorBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox ScheduleGroupComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ViewComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ScheduleTagTypeComboBox;
        private Button TagCurtainWallBtn;
        private Button GrossWallBtn;
    }
}