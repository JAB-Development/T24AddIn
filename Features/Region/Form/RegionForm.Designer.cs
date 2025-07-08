namespace T24AddIn.Features.Region.Form
{
    partial class RegionForm
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
            SelectBtn = new Button();
            label4 = new Label();
            label5 = new Label();
            button1 = new Button();
            ColorRegionOverride = new ColorDialog();
            ColorPickerPannel = new Panel();
            SuspendLayout();
            // 
            // SelectBtn
            // 
            SelectBtn.Location = new Point(63, 254);
            SelectBtn.Name = "SelectBtn";
            SelectBtn.Size = new Size(182, 45);
            SelectBtn.TabIndex = 0;
            SelectBtn.Text = "Select Wall Corner";
            SelectBtn.UseVisualStyleBackColor = true;
            SelectBtn.Click += SelectBtn_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(80, 90);
            label4.Name = "label4";
            label4.Size = new Size(142, 15);
            label4.TabIndex = 7;
            label4.Text = "Pick Each Corner in Order";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(114, 32);
            label5.Name = "label5";
            label5.Size = new Size(70, 25);
            label5.TabIndex = 8;
            label5.Text = "Region";
            // 
            // button1
            // 
            button1.Location = new Point(63, 184);
            button1.Name = "button1";
            button1.Size = new Size(182, 45);
            button1.TabIndex = 9;
            button1.Text = "Select Color";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ColorPickerPannel
            // 
            ColorPickerPannel.BorderStyle = BorderStyle.FixedSingle;
            ColorPickerPannel.Enabled = false;
            ColorPickerPannel.Location = new Point(122, 132);
            ColorPickerPannel.Name = "ColorPickerPannel";
            ColorPickerPannel.Size = new Size(47, 28);
            ColorPickerPannel.TabIndex = 10;
            // 
            // RegionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 343);
            Controls.Add(ColorPickerPannel);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(SelectBtn);
            Name = "RegionForm";
            Text = "RegionForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SelectBtn;
        private Label label4;
        private Label label5;
        private Button button1;
        private ColorDialog ColorRegionOverride;
        private Panel ColorPickerPannel;
    }
}