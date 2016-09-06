﻿
using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace System.Windows.Forms
{
    public static class Application
    {
        public static void EnableVisualStyles() { }
        public static void SetCompatibleTextRenderingDefault(bool value) { }
        public static event EventHandler Idle;
        public static void Run(Form form) { }
        public static void Run(ApplicationContext appContext) { }
    }



    public delegate void SimpleAction();

    public class Timer
    {
        public void Dispose() { }
        public bool Enabled { get; set; }
        public int Interval { get; set; }
        public event EventHandler Tick;
    }
    public class FormClosedEventArgs : EventArgs { }
    public class PreviewKeyEventArgs : EventArgs { }
    public class ApplicationContext
    {
        Form mainForm;
        public ApplicationContext() { }
        public ApplicationContext(Form mainForm)
        {
            this.mainForm = mainForm;
        }

    }
    public class Form : Control
    {
        public Form()
        {
            CreateNativeCefWindowHandle();
        }
        void CreateNativeCefWindowHandle()
        {

        }
        public void Invoke(Delegate ac) { }
        public void Close() { }
        public event EventHandler<FormClosingEventArgs> FormClosing;     
        public event EventHandler<FormClosedEventArgs> FormClosed;

        public static Form CreateFromNativeWindowHwnd(IntPtr hwnd)
        {
            Form newControl = new Form();
            newControl.Handle = hwnd;
            newControl.TopLevelControl = newControl;
            return newControl;
        }
    }

    public class ControlCollection
    {
        Control owner;
        internal ControlCollection(Control owner)
        {
            this.owner = owner;
        }
        public void Add(Control c)
        {
        }
        public void Remove(Control c)
        {

        }
        public void Clear()
        {
        }
    }

    public class Control : IDisposable
    {
        protected bool DesignMode { get; set; }
        protected virtual void OnHandleCreated(EventArgs e)
        {
        }
        protected virtual void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
        }
        protected virtual void OnSizeChanged(EventArgs e)
        {
        }
        public ControlCollection Controls
        {
            get;
            set;
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsHandleCreated { get; set; }
        public IntPtr Handle { get; set; }
        public void Dispose() { }
        public void SetSize(int w, int h)
        {
        }
        public bool Visible { get; set; }
        public virtual string Text { get; set; }
        public virtual void Show() { }
        public virtual Control TopLevelControl
        {
            get;
            set;
        }
        public Control Parent { get; set; }
        public static Control CreateFromNativeWindowHwnd(IntPtr hwnd)
        {
            Control newControl = new Control();
            newControl.Handle = hwnd;
            return newControl;
        }
    }
    public class FormClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }

    public class PreviewKeyDownEventArgs : EventArgs { }
}