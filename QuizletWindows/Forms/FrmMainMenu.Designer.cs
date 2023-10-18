namespace QuizletWindows
{
    partial class FrmMainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainMenu));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnBarStatistics = new DevExpress.XtraBars.BarButtonItem();
            this.btnBarLibrary = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnBarYourOwnClass = new DevExpress.XtraBars.BarButtonItem();
            this.btnBarJoiningClass = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(45, 44, 45, 44);
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.btnBarStatistics,
            this.btnBarLibrary,
            this.btnBarYourOwnClass,
            this.btnBarJoiningClass});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ribbonControl1.MaxItemId = 6;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsMenuMinWidth = 495;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1137, 231);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Main";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnBarStatistics);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnBarLibrary);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Your";
            // 
            // btnBarStatistics
            // 
            this.btnBarStatistics.Caption = "Statistics";
            this.btnBarStatistics.Id = 1;
            this.btnBarStatistics.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBarStatistics.ImageOptions.Image")));
            this.btnBarStatistics.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnBarStatistics.ImageOptions.LargeImage")));
            this.btnBarStatistics.Name = "btnBarStatistics";
            // 
            // btnBarLibrary
            // 
            this.btnBarLibrary.Caption = "Library";
            this.btnBarLibrary.Id = 2;
            this.btnBarLibrary.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBarLibrary.ImageOptions.Image")));
            this.btnBarLibrary.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnBarLibrary.ImageOptions.LargeImage")));
            this.btnBarLibrary.Name = "btnBarLibrary";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnBarYourOwnClass);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnBarJoiningClass);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Class";
            // 
            // btnBarYourOwnClass
            // 
            this.btnBarYourOwnClass.Caption = "Your own class";
            this.btnBarYourOwnClass.Id = 4;
            this.btnBarYourOwnClass.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBarYourOwnClass.ImageOptions.Image")));
            this.btnBarYourOwnClass.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnBarYourOwnClass.ImageOptions.LargeImage")));
            this.btnBarYourOwnClass.Name = "btnBarYourOwnClass";
            // 
            // btnBarJoiningClass
            // 
            this.btnBarJoiningClass.Caption = "Joining class";
            this.btnBarJoiningClass.Id = 5;
            this.btnBarJoiningClass.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnBarJoiningClass.ImageOptions.SvgImage")));
            this.btnBarJoiningClass.Name = "btnBarJoiningClass";
            // 
            // FrmMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 526);
            this.Controls.Add(this.ribbonControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmMainMenu";
            this.Ribbon = this.ribbonControl1;
            this.Text = "Main";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem btnBarStatistics;
        private DevExpress.XtraBars.BarButtonItem btnBarLibrary;
        private DevExpress.XtraBars.BarButtonItem btnBarYourOwnClass;
        private DevExpress.XtraBars.BarButtonItem btnBarJoiningClass;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
    }
}

