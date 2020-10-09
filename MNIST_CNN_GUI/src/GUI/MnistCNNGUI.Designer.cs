namespace Mnist_ANN_GUI
{
    partial class MnistCNNGUI
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
            this.MnistPictureBox = new System.Windows.Forms.PictureBox();
            this.MnistImageLabel = new System.Windows.Forms.Label();
            this.TrainButton = new System.Windows.Forms.Button();
            this.TestImageButton = new System.Windows.Forms.Button();
            this.ImageSelectionTextBox = new System.Windows.Forms.TextBox();
            this.ImageSelectionlabel = new System.Windows.Forms.Label();
            this.UseTrainingSetCheckBox = new System.Windows.Forms.CheckBox();
            this.InvalidImageLabel = new System.Windows.Forms.Label();
            this.PredictionLabel = new System.Windows.Forms.Label();
            this.TrainingProgressLabel = new System.Windows.Forms.Label();
            this.TrainingProgressBar = new System.Windows.Forms.ProgressBar();
            this.TrainingSensLabel = new System.Windows.Forms.Label();
            this.TestingSensLabel = new System.Windows.Forms.Label();
            this.TotalEpochLabel = new System.Windows.Forms.Label();
            this.NumEpochsComboBox = new System.Windows.Forms.ComboBox();
            this.NumEpochsLabel = new System.Windows.Forms.Label();
            this.CancelNetworkButton = new System.Windows.Forms.Button();
            this.ResetNetworkButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MnistPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MnistPictureBox
            // 
            this.MnistPictureBox.BackColor = System.Drawing.Color.Black;
            this.MnistPictureBox.Location = new System.Drawing.Point(252, 81);
            this.MnistPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MnistPictureBox.Name = "MnistPictureBox";
            this.MnistPictureBox.Size = new System.Drawing.Size(37, 34);
            this.MnistPictureBox.TabIndex = 0;
            this.MnistPictureBox.TabStop = false;
            // 
            // MnistImageLabel
            // 
            this.MnistImageLabel.AutoSize = true;
            this.MnistImageLabel.Location = new System.Drawing.Point(297, 100);
            this.MnistImageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MnistImageLabel.Name = "MnistImageLabel";
            this.MnistImageLabel.Size = new System.Drawing.Size(101, 17);
            this.MnistImageLabel.TabIndex = 1;
            this.MnistImageLabel.Text = "Image Label: 1";
            // 
            // TrainButton
            // 
            this.TrainButton.Location = new System.Drawing.Point(16, 196);
            this.TrainButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TrainButton.Name = "TrainButton";
            this.TrainButton.Size = new System.Drawing.Size(163, 55);
            this.TrainButton.TabIndex = 2;
            this.TrainButton.Text = "Train 1 Epoch";
            this.TrainButton.UseVisualStyleBackColor = true;
            this.TrainButton.Click += new System.EventHandler(this.TrainButton_Click);
            // 
            // TestImageButton
            // 
            this.TestImageButton.Location = new System.Drawing.Point(252, 139);
            this.TestImageButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TestImageButton.Name = "TestImageButton";
            this.TestImageButton.Size = new System.Drawing.Size(163, 28);
            this.TestImageButton.TabIndex = 3;
            this.TestImageButton.Text = "Test on Image";
            this.TestImageButton.UseVisualStyleBackColor = true;
            this.TestImageButton.Click += new System.EventHandler(this.TestImageButton_Click);
            // 
            // ImageSelectionTextBox
            // 
            this.ImageSelectionTextBox.Location = new System.Drawing.Point(252, 49);
            this.ImageSelectionTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ImageSelectionTextBox.Name = "ImageSelectionTextBox";
            this.ImageSelectionTextBox.Size = new System.Drawing.Size(132, 22);
            this.ImageSelectionTextBox.TabIndex = 4;
            this.ImageSelectionTextBox.TextChanged += new System.EventHandler(this.ImageSelectionTextBox_TextChanged);
            // 
            // ImageSelectionlabel
            // 
            this.ImageSelectionlabel.AutoSize = true;
            this.ImageSelectionlabel.Location = new System.Drawing.Point(248, 30);
            this.ImageSelectionlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ImageSelectionlabel.Name = "ImageSelectionlabel";
            this.ImageSelectionlabel.Size = new System.Drawing.Size(200, 17);
            this.ImageSelectionlabel.TabIndex = 5;
            this.ImageSelectionlabel.Text = "Choose image to test (0-9999)";
            // 
            // UseTrainingSetCheckBox
            // 
            this.UseTrainingSetCheckBox.AutoSize = true;
            this.UseTrainingSetCheckBox.Location = new System.Drawing.Point(252, 5);
            this.UseTrainingSetCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.UseTrainingSetCheckBox.Name = "UseTrainingSetCheckBox";
            this.UseTrainingSetCheckBox.Size = new System.Drawing.Size(218, 21);
            this.UseTrainingSetCheckBox.TabIndex = 6;
            this.UseTrainingSetCheckBox.Text = "Use images from training set?";
            this.UseTrainingSetCheckBox.UseVisualStyleBackColor = true;
            this.UseTrainingSetCheckBox.CheckedChanged += new System.EventHandler(this.UseTrainingSetCheckBox_CheckedChanged);
            // 
            // InvalidImageLabel
            // 
            this.InvalidImageLabel.AutoSize = true;
            this.InvalidImageLabel.ForeColor = System.Drawing.Color.Red;
            this.InvalidImageLabel.Location = new System.Drawing.Point(393, 53);
            this.InvalidImageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.InvalidImageLabel.Name = "InvalidImageLabel";
            this.InvalidImageLabel.Size = new System.Drawing.Size(48, 17);
            this.InvalidImageLabel.TabIndex = 7;
            this.InvalidImageLabel.Text = "Invalid";
            this.InvalidImageLabel.Visible = false;
            // 
            // PredictionLabel
            // 
            this.PredictionLabel.AutoSize = true;
            this.PredictionLabel.Location = new System.Drawing.Point(247, 119);
            this.PredictionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PredictionLabel.Name = "PredictionLabel";
            this.PredictionLabel.Size = new System.Drawing.Size(196, 17);
            this.PredictionLabel.TabIndex = 8;
            this.PredictionLabel.Text = "Network Label Prediction: N/A";
            // 
            // TrainingProgressLabel
            // 
            this.TrainingProgressLabel.AutoSize = true;
            this.TrainingProgressLabel.Location = new System.Drawing.Point(17, 59);
            this.TrainingProgressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TrainingProgressLabel.Name = "TrainingProgressLabel";
            this.TrainingProgressLabel.Size = new System.Drawing.Size(148, 17);
            this.TrainingProgressLabel.TabIndex = 9;
            this.TrainingProgressLabel.Text = "Network Progress: 0%";
            // 
            // TrainingProgressBar
            // 
            this.TrainingProgressBar.Location = new System.Drawing.Point(17, 79);
            this.TrainingProgressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TrainingProgressBar.Name = "TrainingProgressBar";
            this.TrainingProgressBar.Size = new System.Drawing.Size(163, 28);
            this.TrainingProgressBar.TabIndex = 10;
            // 
            // TrainingSensLabel
            // 
            this.TrainingSensLabel.AutoSize = true;
            this.TrainingSensLabel.Location = new System.Drawing.Point(17, 116);
            this.TrainingSensLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TrainingSensLabel.Name = "TrainingSensLabel";
            this.TrainingSensLabel.Size = new System.Drawing.Size(158, 17);
            this.TrainingSensLabel.TabIndex = 12;
            this.TrainingSensLabel.Text = "Training Sensitivity: N/A";
            // 
            // TestingSensLabel
            // 
            this.TestingSensLabel.AutoSize = true;
            this.TestingSensLabel.Location = new System.Drawing.Point(17, 149);
            this.TestingSensLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TestingSensLabel.Name = "TestingSensLabel";
            this.TestingSensLabel.Size = new System.Drawing.Size(153, 17);
            this.TestingSensLabel.TabIndex = 13;
            this.TestingSensLabel.Text = "Testing Sensitivity: N/A";
            // 
            // TotalEpochLabel
            // 
            this.TotalEpochLabel.AutoSize = true;
            this.TotalEpochLabel.Location = new System.Drawing.Point(19, 176);
            this.TotalEpochLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TotalEpochLabel.Name = "TotalEpochLabel";
            this.TotalEpochLabel.Size = new System.Drawing.Size(160, 17);
            this.TotalEpochLabel.TabIndex = 14;
            this.TotalEpochLabel.Text = "Total Trained Epochs: 0";
            // 
            // NumEpochsComboBox
            // 
            this.NumEpochsComboBox.FormattingEnabled = true;
            this.NumEpochsComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10"});
            this.NumEpochsComboBox.Location = new System.Drawing.Point(23, 30);
            this.NumEpochsComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NumEpochsComboBox.Name = "NumEpochsComboBox";
            this.NumEpochsComboBox.Size = new System.Drawing.Size(160, 24);
            this.NumEpochsComboBox.TabIndex = 15;
            this.NumEpochsComboBox.SelectedIndexChanged += new System.EventHandler(this.NumEpochsComboBox_SelectedIndexChanged);
            // 
            // NumEpochsLabel
            // 
            this.NumEpochsLabel.AutoSize = true;
            this.NumEpochsLabel.Location = new System.Drawing.Point(21, 6);
            this.NumEpochsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NumEpochsLabel.Name = "NumEpochsLabel";
            this.NumEpochsLabel.Size = new System.Drawing.Size(201, 17);
            this.NumEpochsLabel.TabIndex = 16;
            this.NumEpochsLabel.Text = "Number of Epochs per training";
            // 
            // CancelNetworkButton
            // 
            this.CancelNetworkButton.Location = new System.Drawing.Point(17, 260);
            this.CancelNetworkButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CancelNetworkButton.Name = "CancelNetworkButton";
            this.CancelNetworkButton.Size = new System.Drawing.Size(161, 28);
            this.CancelNetworkButton.TabIndex = 17;
            this.CancelNetworkButton.Text = "Cancel";
            this.CancelNetworkButton.UseVisualStyleBackColor = true;
            this.CancelNetworkButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ResetNetworkButton
            // 
            this.ResetNetworkButton.Location = new System.Drawing.Point(251, 260);
            this.ResetNetworkButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ResetNetworkButton.Name = "ResetNetworkButton";
            this.ResetNetworkButton.Size = new System.Drawing.Size(164, 28);
            this.ResetNetworkButton.TabIndex = 18;
            this.ResetNetworkButton.Text = "Reset Network";
            this.ResetNetworkButton.UseVisualStyleBackColor = true;
            this.ResetNetworkButton.Click += new System.EventHandler(this.ResetNetworkButton_Click);
            // 
            // MnistCNNGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 314);
            this.Controls.Add(this.ResetNetworkButton);
            this.Controls.Add(this.CancelNetworkButton);
            this.Controls.Add(this.NumEpochsLabel);
            this.Controls.Add(this.NumEpochsComboBox);
            this.Controls.Add(this.TotalEpochLabel);
            this.Controls.Add(this.TestingSensLabel);
            this.Controls.Add(this.TrainingSensLabel);
            this.Controls.Add(this.TrainingProgressBar);
            this.Controls.Add(this.TrainingProgressLabel);
            this.Controls.Add(this.PredictionLabel);
            this.Controls.Add(this.InvalidImageLabel);
            this.Controls.Add(this.UseTrainingSetCheckBox);
            this.Controls.Add(this.ImageSelectionlabel);
            this.Controls.Add(this.ImageSelectionTextBox);
            this.Controls.Add(this.TestImageButton);
            this.Controls.Add(this.TrainButton);
            this.Controls.Add(this.MnistImageLabel);
            this.Controls.Add(this.MnistPictureBox);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MnistCNNGUI";
            this.Text = "MNIST CNN";
            ((System.ComponentModel.ISupportInitialize)(this.MnistPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MnistPictureBox;
        private System.Windows.Forms.Label MnistImageLabel;
        private System.Windows.Forms.Button TrainButton;
        private System.Windows.Forms.Button TestImageButton;
        private System.Windows.Forms.TextBox ImageSelectionTextBox;
        private System.Windows.Forms.Label ImageSelectionlabel;
        private System.Windows.Forms.CheckBox UseTrainingSetCheckBox;
        private System.Windows.Forms.Label InvalidImageLabel;
        private System.Windows.Forms.Label PredictionLabel;
        private System.Windows.Forms.Label TrainingProgressLabel;
        private System.Windows.Forms.ProgressBar TrainingProgressBar;
        private System.Windows.Forms.Label TrainingSensLabel;
        private System.Windows.Forms.Label TestingSensLabel;
        private System.Windows.Forms.Label TotalEpochLabel;
        private System.Windows.Forms.ComboBox NumEpochsComboBox;
        private System.Windows.Forms.Label NumEpochsLabel;
        private System.Windows.Forms.Button CancelNetworkButton;
        private System.Windows.Forms.Button ResetNetworkButton;
    }
}

