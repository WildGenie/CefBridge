﻿//2015-2016 MIT, WinterDev

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace LayoutFarm.CefBridge
{
    public partial class Form1 : Form
    {
        LayoutFarm.CefBridge.IWindowForm nativeWindow;
        public Form1()
        {
            InitializeComponent();
            nativeWindow = LayoutFarm.CefBridge.MyWindowForm.TryGetWindowFormOrRegisterIfNotExists(this);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.cefWebBrowser1.Agent.Listener = new MyCefUIProcessListener();
            this.splitContainer1.SplitterMoved += SplitContainer1_SplitterMoved;
        }
        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            cefWebBrowser1.Agent.SetSize(splitContainer1.Panel2.Width, splitContainer1.Panel2.Height);
        }
        public void Navigate(string url)
        {
            this.cefWebBrowser1.NavigateTo(url);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.cefWebBrowser1.Focus();
            this.cefWebBrowser1.NavigateTo("https://html5test.com");
            //this.cefWebBrowser1.NavigateTo("https://localhost:8000");
            //this.cefWebBrowser1.NavigateTo("http://localhost/LiborMasekThesis.pdf");
            //this.cefWebBrowser1.NavigateTo("http://localhost/pdfjs/web/viewer.html");
            //this.cefWebBrowser1.NavigateTo("http://localhost/pdfjs/web/compressed.tracemonkey-pldi-09.pdf");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            cefWebBrowser1.Agent.ExecJavascript(
                 "window.open('https://html5test.com');", "about:blank");
            //cefWebBrowser1.Agent.ExecJavascript(
            //    "alert('test!');", "about:blank");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string data = "arg1=val1&arg2=val2";
            byte[] dataBuffer = Encoding.UTF8.GetBytes(data);
            cefWebBrowser1.Agent.PostData(
                "http://tests/request",
                dataBuffer,
                dataBuffer.Length);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cefWebBrowser1.Agent.GetText(
                str =>
                {
                    Console.WriteLine(str);
                });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cefWebBrowser1.Agent.GetSource(
                str =>
                {
                    Console.WriteLine(str);
                });
        }



        private void button8_Click(object sender, EventArgs e)
        {
            this.cefWebBrowser1.Agent.ShowDevTools();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 anotherForm1 = new Form1();
            anotherForm1.Show();
            anotherForm1.Navigate("http://localhost");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //back
            this.cefWebBrowser1.Agent.GoBack();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //foward
            this.cefWebBrowser1.Agent.GoForward();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //reload
            this.cefWebBrowser1.Agent.Reload();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //stop
            this.cefWebBrowser1.Agent.Stop();
        }

        private void cmdReloadIgnoreCache_Click(object sender, EventArgs e)
        {
            this.cefWebBrowser1.Agent.ReloadIgnoreCache();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.cefWebBrowser1.Focus();
            this.cefWebBrowser1.NavigateTo("http://www.youtube.com");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.cefWebBrowser1.Focus();
            this.cefWebBrowser1.NavigateTo("http://localhost/index2.html");

            //this.cefWebBrowser2.Focus();
            //this.cefWebBrowser2.NavigateTo("https://www.google.com");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            {
                var p = cefWebBrowser1.Parent;
                p.Controls.Remove(cefWebBrowser1);
                cefWebBrowser1.Dispose();
            }
            //{
            //    var p = cefWebBrowserControl1.Parent;
            //    p.Controls.Remove(p);
            //    p.Dispose();
            //}
        }
    }
}
