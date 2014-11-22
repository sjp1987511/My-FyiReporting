﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mono.Data.Sqlite;
using System.Drawing.Printing;

namespace SampleApp2_SetData
{
    public class Form1 : Form
    {
        private fyiReporting.RdlViewer.RdlViewer rdlViewer1;
        private ToolStrip bar;
        private ToolStripTextBox currentPage = new ToolStripTextBox("Current Page");
        private ToolStripLabel pageCount = new ToolStripLabel("Page Count");

        public Form1()
        {
            InitializeComponent();

            // TODO: You must change this connection string to match where your database is
            string connectionString = @"Data Source=/home/peter/Projects/My-FyiReporting/Examples/northwindEF.db;Version=3;Pooling=True;Max Pool Size=100;";
            var cn = new SqliteConnection(connectionString);
            var cmd = new SqliteCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CategoryID, CategoryName, Description FROM Categories;";
            cmd.Connection = cn;
      
 
        }

        private void OpenClicked(object sender, System.EventArgs e)
        {
            var dlg = new OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            rdlViewer1.SourceFile = new Uri(dlg.FileName);
            rdlViewer1.Rebuild();

            currentPage.Text = rdlViewer1.PageCurrent.ToString();
            pageCount.Text = rdlViewer1.PageCount.ToString();
        }

        private void PrintClicked(object sender, System.EventArgs e)
        {

            PrintDocument pd = new PrintDocument();
            pd.DocumentName = rdlViewer1.SourceFile.LocalPath;
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = rdlViewer1.PageCount;
            pd.PrinterSettings.MaximumPage = rdlViewer1.PageCount;
            pd.PrinterSettings.MinimumPage = 1;
            pd.DefaultPageSettings.Landscape = rdlViewer1.PageWidth > rdlViewer1.PageHeight ? true : false;
            using (PrintDialog dlg = new PrintDialog())
            {
                dlg.Document = pd;
                dlg.AllowSelection = true;
                dlg.AllowSomePages = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    rdlViewer1.Print(pd);
                }
            }
     
        }

        private void SaveAsClicked(object sender, System.EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "PDF files|*.pdf|XML files|*.xml|HTML files|*.html|CSV files|*.csv|RTF files|*.rtf|TIF files|*.tif|Excel files|*.xlsx|MHT files|*.mht";
            dlg.FileName = ".pdf";
            var result = dlg.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }
            // save the report in a rendered format
            string ext = null;
            int i = dlg.FileName.LastIndexOf('.');
            if (i < 1)
            {
                ext = "";
            }
            else
            {
                ext = dlg.FileName.Substring(i + 1).ToLower();
            }
            fyiReporting.RDL.OutputPresentationType type = fyiReporting.RDL.OutputPresentationType.Internal;
            switch (ext)
            {
                case "pdf":
                    type = fyiReporting.RDL.OutputPresentationType.PDF;
                    break;
                case "xml":
                    type = fyiReporting.RDL.OutputPresentationType.XML;
                    break;
                case "html":
                    type = fyiReporting.RDL.OutputPresentationType.HTML;
                    break;
                case "htm":
                    type = fyiReporting.RDL.OutputPresentationType.HTML;
                    break;
                case "csv":
                    type = fyiReporting.RDL.OutputPresentationType.CSV;
                    break;
                case "rtf":
                    type = fyiReporting.RDL.OutputPresentationType.RTF;
                    break;
                case "mht":
                    type = fyiReporting.RDL.OutputPresentationType.MHTML;
                    break;
                case "mhtml":
                    type = fyiReporting.RDL.OutputPresentationType.MHTML;
                    break;
                case "xlsx":
                    type = fyiReporting.RDL.OutputPresentationType.Excel;
                    break;
                case "tif":
                    type = fyiReporting.RDL.OutputPresentationType.TIF;
                    break;
                case "tiff":
                    type = fyiReporting.RDL.OutputPresentationType.TIF;
                    break;
                default:
                    MessageBox.Show(String.Format("{0} is not a valid file type. File extension must be PDF, XML, HTML, CSV, MHT, RTF, TIF, XLSX.", dlg.FileName),
                        "Invalid File Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            rdlViewer1.SaveAs(dlg.FileName, type);
        }

        private void FirstPageClicked(object sender, System.EventArgs e)
        {

        }

        private void PreviousPageClicked(object sender, System.EventArgs e)
        {
        }

        private void NextPageClicked(object sender, System.EventArgs e)
        {
        }

        private void LastPageClicked(object sender, System.EventArgs e)
        {
        }

        private void ZoomInClicked(object sender, System.EventArgs e)
        {
            rdlViewer1.Zoom += 0.5f;
        }

        private void ZoomOutClicked(object sender, System.EventArgs e)
        {
            rdlViewer1.Zoom -= 0.5f;
        }

        private void InitializeToolBar()
        {
            this.bar = new ToolStrip();
            this.Controls.Add(bar);

            bar.Items.Add(new ToolStripButton("Open", null, OpenClicked));
            bar.Items.Add(new ToolStripButton("Save As", null, SaveAsClicked));
            bar.Items.Add(new ToolStripButton("Print", null, PrintClicked));
            //bar.Items.Add(new ToolStripButton("First Page", null, FirstPageClicked));
            //bar.Items.Add(new ToolStripButton("Previous Page", null, PreviousPageClicked));
            //bar.Items.Add(new ToolStripButton("Next Page", null, NextPageClicked));
            //bar.Items.Add(new ToolStripButton("Last Page", null, LastPageClicked));
            bar.Items.Add(this.currentPage);
            bar.Items.Add(this.pageCount);
            bar.Items.Add(new ToolStripButton("Zoom In", null, ZoomInClicked));
            bar.Items.Add(new ToolStripButton("Zoom Out", null, ZoomOutClicked));
        }

        private void InitializeViewer()
        {
            this.rdlViewer1 = new fyiReporting.RdlViewer.RdlViewer();
            this.rdlViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));

            this.rdlViewer1.Location = new System.Drawing.Point(40, 69);
            this.rdlViewer1.Name = "rdlViewer1";

            this.rdlViewer1.Size = new System.Drawing.Size(731, 381);

            this.rdlViewer1.PageNavigation += HandlePageNavigation;

        }

        void HandlePageNavigation(object sender, fyiReporting.RdlViewer.PageNavigationEventArgs e)
        {
            currentPage.Text = e.NewPage.ToString();
        }


        private void InitializeComponent()
        {
            InitializeToolBar();
            InitializeViewer();

         
            this.SuspendLayout();
 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 462);
            this.Controls.Add(this.rdlViewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }



    }
}