namespace USB_Comm_Tool
{
    partial class MainForm
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
            this.DiscoverDevicesButton = new System.Windows.Forms.Button();
            this.DiscoveredDevicesListBox = new System.Windows.Forms.ListBox();
            this.ConnectToDeviceButton = new System.Windows.Forms.Button();
            this.GetDescriptorsButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.DescriptorsTabPage = new System.Windows.Forms.TabPage();
            this.DescriptorsPanel = new System.Windows.Forms.Panel();
            this.CustomDescriptorsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.EndpointDescriptorsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.InterfaceDescriptorsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ConfigurationDescriptorsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.DeviceDescriptorsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SendReceiveTabPage = new System.Windows.Forms.TabPage();
            this.RequestResponseButton = new System.Windows.Forms.Button();
            this.QueryButton = new System.Windows.Forms.Button();
            this.ReceiveButton = new System.Windows.Forms.Button();
            this.ReceiveRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.SendRichTextBox = new System.Windows.Forms.RichTextBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.DescriptorsTabPage.SuspendLayout();
            this.DescriptorsPanel.SuspendLayout();
            this.SendReceiveTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // DiscoverDevicesButton
            // 
            this.DiscoverDevicesButton.Location = new System.Drawing.Point(12, 12);
            this.DiscoverDevicesButton.Name = "DiscoverDevicesButton";
            this.DiscoverDevicesButton.Size = new System.Drawing.Size(99, 23);
            this.DiscoverDevicesButton.TabIndex = 0;
            this.DiscoverDevicesButton.Text = "Discover Devices";
            this.DiscoverDevicesButton.UseVisualStyleBackColor = true;
            this.DiscoverDevicesButton.Click += new System.EventHandler(this.DiscoverDevicesButton_Click);
            // 
            // DiscoveredDevicesListBox
            // 
            this.DiscoveredDevicesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DiscoveredDevicesListBox.FormattingEnabled = true;
            this.DiscoveredDevicesListBox.Location = new System.Drawing.Point(12, 41);
            this.DiscoveredDevicesListBox.Name = "DiscoveredDevicesListBox";
            this.DiscoveredDevicesListBox.Size = new System.Drawing.Size(302, 433);
            this.DiscoveredDevicesListBox.TabIndex = 1;
            // 
            // ConnectToDeviceButton
            // 
            this.ConnectToDeviceButton.Location = new System.Drawing.Point(117, 12);
            this.ConnectToDeviceButton.Name = "ConnectToDeviceButton";
            this.ConnectToDeviceButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectToDeviceButton.TabIndex = 2;
            this.ConnectToDeviceButton.Text = "Connect";
            this.ConnectToDeviceButton.UseVisualStyleBackColor = true;
            this.ConnectToDeviceButton.Click += new System.EventHandler(this.ConnectToDeviceButton_Click);
            // 
            // GetDescriptorsButton
            // 
            this.GetDescriptorsButton.Location = new System.Drawing.Point(6, 16);
            this.GetDescriptorsButton.Name = "GetDescriptorsButton";
            this.GetDescriptorsButton.Size = new System.Drawing.Size(90, 23);
            this.GetDescriptorsButton.TabIndex = 3;
            this.GetDescriptorsButton.Text = "Get Descriptors";
            this.GetDescriptorsButton.UseVisualStyleBackColor = true;
            this.GetDescriptorsButton.Click += new System.EventHandler(this.GetDeviceDescriptorsButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.DescriptorsTabPage);
            this.tabControl1.Controls.Add(this.SendReceiveTabPage);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(318, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(823, 462);
            this.tabControl1.TabIndex = 7;
            // 
            // DescriptorsTabPage
            // 
            this.DescriptorsTabPage.Controls.Add(this.DescriptorsPanel);
            this.DescriptorsTabPage.Location = new System.Drawing.Point(4, 22);
            this.DescriptorsTabPage.Name = "DescriptorsTabPage";
            this.DescriptorsTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.DescriptorsTabPage.Size = new System.Drawing.Size(815, 436);
            this.DescriptorsTabPage.TabIndex = 0;
            this.DescriptorsTabPage.Text = "Descriptors";
            this.DescriptorsTabPage.UseVisualStyleBackColor = true;
            // 
            // DescriptorsPanel
            // 
            this.DescriptorsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptorsPanel.AutoScroll = true;
            this.DescriptorsPanel.Controls.Add(this.GetDescriptorsButton);
            this.DescriptorsPanel.Controls.Add(this.CustomDescriptorsRichTextBox);
            this.DescriptorsPanel.Controls.Add(this.EndpointDescriptorsRichTextBox);
            this.DescriptorsPanel.Controls.Add(this.InterfaceDescriptorsRichTextBox);
            this.DescriptorsPanel.Controls.Add(this.ConfigurationDescriptorsRichTextBox);
            this.DescriptorsPanel.Controls.Add(this.DeviceDescriptorsRichTextBox);
            this.DescriptorsPanel.Location = new System.Drawing.Point(6, 6);
            this.DescriptorsPanel.Name = "DescriptorsPanel";
            this.DescriptorsPanel.Size = new System.Drawing.Size(803, 424);
            this.DescriptorsPanel.TabIndex = 7;
            // 
            // CustomDescriptorsRichTextBox
            // 
            this.CustomDescriptorsRichTextBox.Location = new System.Drawing.Point(405, 174);
            this.CustomDescriptorsRichTextBox.Name = "CustomDescriptorsRichTextBox";
            this.CustomDescriptorsRichTextBox.Size = new System.Drawing.Size(390, 245);
            this.CustomDescriptorsRichTextBox.TabIndex = 12;
            this.CustomDescriptorsRichTextBox.Text = "";
            // 
            // EndpointDescriptorsRichTextBox
            // 
            this.EndpointDescriptorsRichTextBox.Location = new System.Drawing.Point(405, 45);
            this.EndpointDescriptorsRichTextBox.Name = "EndpointDescriptorsRichTextBox";
            this.EndpointDescriptorsRichTextBox.Size = new System.Drawing.Size(390, 123);
            this.EndpointDescriptorsRichTextBox.TabIndex = 10;
            this.EndpointDescriptorsRichTextBox.Text = "";
            // 
            // InterfaceDescriptorsRichTextBox
            // 
            this.InterfaceDescriptorsRichTextBox.Location = new System.Drawing.Point(6, 267);
            this.InterfaceDescriptorsRichTextBox.Name = "InterfaceDescriptorsRichTextBox";
            this.InterfaceDescriptorsRichTextBox.Size = new System.Drawing.Size(390, 152);
            this.InterfaceDescriptorsRichTextBox.TabIndex = 8;
            this.InterfaceDescriptorsRichTextBox.Text = "";
            // 
            // ConfigurationDescriptorsRichTextBox
            // 
            this.ConfigurationDescriptorsRichTextBox.Location = new System.Drawing.Point(6, 164);
            this.ConfigurationDescriptorsRichTextBox.Name = "ConfigurationDescriptorsRichTextBox";
            this.ConfigurationDescriptorsRichTextBox.Size = new System.Drawing.Size(390, 97);
            this.ConfigurationDescriptorsRichTextBox.TabIndex = 6;
            this.ConfigurationDescriptorsRichTextBox.Text = "";
            // 
            // DeviceDescriptorsRichTextBox
            // 
            this.DeviceDescriptorsRichTextBox.Location = new System.Drawing.Point(6, 45);
            this.DeviceDescriptorsRichTextBox.Name = "DeviceDescriptorsRichTextBox";
            this.DeviceDescriptorsRichTextBox.Size = new System.Drawing.Size(390, 113);
            this.DeviceDescriptorsRichTextBox.TabIndex = 4;
            this.DeviceDescriptorsRichTextBox.Text = "";
            // 
            // SendReceiveTabPage
            // 
            this.SendReceiveTabPage.Controls.Add(this.RequestResponseButton);
            this.SendReceiveTabPage.Controls.Add(this.QueryButton);
            this.SendReceiveTabPage.Controls.Add(this.ReceiveButton);
            this.SendReceiveTabPage.Controls.Add(this.ReceiveRichTextBox);
            this.SendReceiveTabPage.Controls.Add(this.SendButton);
            this.SendReceiveTabPage.Controls.Add(this.SendRichTextBox);
            this.SendReceiveTabPage.Location = new System.Drawing.Point(4, 22);
            this.SendReceiveTabPage.Name = "SendReceiveTabPage";
            this.SendReceiveTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.SendReceiveTabPage.Size = new System.Drawing.Size(815, 436);
            this.SendReceiveTabPage.TabIndex = 1;
            this.SendReceiveTabPage.Text = "Send / Receive";
            this.SendReceiveTabPage.UseVisualStyleBackColor = true;
            // 
            // RequestResponseButton
            // 
            this.RequestResponseButton.Location = new System.Drawing.Point(87, 6);
            this.RequestResponseButton.Name = "RequestResponseButton";
            this.RequestResponseButton.Size = new System.Drawing.Size(108, 23);
            this.RequestResponseButton.TabIndex = 5;
            this.RequestResponseButton.Text = "Request Response";
            this.RequestResponseButton.UseVisualStyleBackColor = true;
            this.RequestResponseButton.Click += new System.EventHandler(this.RequestResponseButton_Click);
            // 
            // QueryButton
            // 
            this.QueryButton.Location = new System.Drawing.Point(201, 6);
            this.QueryButton.Name = "QueryButton";
            this.QueryButton.Size = new System.Drawing.Size(75, 23);
            this.QueryButton.TabIndex = 4;
            this.QueryButton.Text = "Query";
            this.QueryButton.UseVisualStyleBackColor = true;
            this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
            // 
            // ReceiveButton
            // 
            this.ReceiveButton.Location = new System.Drawing.Point(6, 248);
            this.ReceiveButton.Name = "ReceiveButton";
            this.ReceiveButton.Size = new System.Drawing.Size(75, 23);
            this.ReceiveButton.TabIndex = 3;
            this.ReceiveButton.Text = "Receive";
            this.ReceiveButton.UseVisualStyleBackColor = true;
            this.ReceiveButton.Click += new System.EventHandler(this.ReceiveButton_Click);
            // 
            // ReceiveRichTextBox
            // 
            this.ReceiveRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReceiveRichTextBox.Location = new System.Drawing.Point(6, 277);
            this.ReceiveRichTextBox.Name = "ReceiveRichTextBox";
            this.ReceiveRichTextBox.Size = new System.Drawing.Size(812, 219);
            this.ReceiveRichTextBox.TabIndex = 2;
            this.ReceiveRichTextBox.Text = "";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(6, 6);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 1;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // SendRichTextBox
            // 
            this.SendRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendRichTextBox.Location = new System.Drawing.Point(6, 35);
            this.SendRichTextBox.Name = "SendRichTextBox";
            this.SendRichTextBox.Size = new System.Drawing.Size(812, 207);
            this.SendRichTextBox.TabIndex = 0;
            this.SendRichTextBox.Text = "*IDN?";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(1053, 6);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(48, 13);
            this.VersionLabel.TabIndex = 8;
            this.VersionLabel.Text = "Version: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 486);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ConnectToDeviceButton);
            this.Controls.Add(this.DiscoveredDevicesListBox);
            this.Controls.Add(this.DiscoverDevicesButton);
            this.Name = "MainForm";
            this.Text = "USB Comm Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.DescriptorsTabPage.ResumeLayout(false);
            this.DescriptorsPanel.ResumeLayout(false);
            this.SendReceiveTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DiscoverDevicesButton;
        private System.Windows.Forms.ListBox DiscoveredDevicesListBox;
        private System.Windows.Forms.Button ConnectToDeviceButton;
        private System.Windows.Forms.Button GetDescriptorsButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage DescriptorsTabPage;
        private System.Windows.Forms.Panel DescriptorsPanel;
        private System.Windows.Forms.RichTextBox CustomDescriptorsRichTextBox;
        private System.Windows.Forms.RichTextBox EndpointDescriptorsRichTextBox;
        private System.Windows.Forms.RichTextBox InterfaceDescriptorsRichTextBox;
        private System.Windows.Forms.RichTextBox ConfigurationDescriptorsRichTextBox;
        private System.Windows.Forms.RichTextBox DeviceDescriptorsRichTextBox;
        private System.Windows.Forms.TabPage SendReceiveTabPage;
        private System.Windows.Forms.Button ReceiveButton;
        private System.Windows.Forms.RichTextBox ReceiveRichTextBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.RichTextBox SendRichTextBox;
        private System.Windows.Forms.Button QueryButton;
        private System.Windows.Forms.Button RequestResponseButton;
        private System.Windows.Forms.Label VersionLabel;
    }
}

