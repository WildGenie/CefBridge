﻿//2015-2016 MIT, WinterDev

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace LayoutFarm.CefBridge
{
    public enum CefV8PropertyAttribute
    {
        //from cef_types.h
        //cef_v8_propertyattribute_t
        V8_PROPERTY_ATTRIBUTE_NONE = 0, // Writeable, Enumerable,Configurable
        V8_PROPERTY_ATTRIBUTE_READONLY = 1 << 0, // Not writeable
        V8_PROPERTY_ATTRIBUTE_DONTENUM = 1 << 1,  // Not enumerable
        V8_PROPERTY_ATTRIBUTE_DONTDELETE = 1 << 2   // Not configurable
    }

    public abstract class Cef3RefCountingValue : IDisposable
    {
        readonly IntPtr _ptr;
        public Cef3RefCountingValue(IntPtr ptr)
        {
            _ptr = ptr;
        }
        public void Dispose()
        {
            //TODO: release pointer back
        }
        public IntPtr Ptr
        {
            get { return _ptr; }
        }
    }

    public class Cef3FuncHandler : Cef3RefCountingValue
    {
        private Cef3FuncHandler(IntPtr ptr) : base(ptr)
        {
        }

        public static Cef3FuncHandler CreateFuncHandler(MyCefCallback cefCallback)
        {
            //store in cbs, prevent collected by GC
            cbs.Add(cefCallback);
            return new Cef3FuncHandler(Cef3Binder.MyCefJs_New_V8Handler(cefCallback));
        }
        static List<MyCefCallback> cbs = new List<MyCefCallback>();
    }

    public class Cef3Func : Cef3RefCountingValue
    {
        public Cef3Func(IntPtr ptr) : base(ptr)
        {
        }
        public static Cef3Func CreateFunc(string name, Cef3FuncHandler funcHandler)
        {
            IntPtr func = Cef3Binder.MyCefJs_CreateFunction(name, funcHandler.Ptr);
            return new Cef3Func(func);
        }
        public CefV8Value ExecFunction(NativeJsContext context, string argAsJsonString)
        {
            unsafe
            {
                char[] chars = argAsJsonString.ToCharArray();
                fixed (char* first = &chars[0])
                {
                    CefV8Value value = new CefV8Value(Cef3Binder.MyCefJs_ExecJsFunctionWithContext(this.Ptr, context.Ptr, first));
                    return value;
                }
            }
        }
        public CefV8Value ExecFunction(NativeJsContext context, char[] argAsJsonChars)
        {
            unsafe
            {
                fixed (char* first = &argAsJsonChars[0])
                {
                    CefV8Value value = new CefV8Value(Cef3Binder.MyCefJs_ExecJsFunctionWithContext(this.Ptr, context.Ptr, first));
                    return value;
                }
            }
        }
    }

    public class NativeJsContext : Cef3RefCountingValue
    {
        public NativeJsContext(IntPtr ptr)
            : base(ptr)
        {
        }
        /// <summary>
        /// get global object
        /// </summary>
        /// <returns></returns>
        public CefV8Value GetGlobal()
        {
            return new CefV8Value(Cef3Binder.MyCefJsGetGlobal(this.Ptr));
        }
        public static NativeJsContext GetCurrentContext()
        {
            return new NativeJsContext(Cef3Binder.MyCefJsGetCurrentContext());
        }
        public static NativeJsContext GetEnteredContext()
        {
            return new NativeJsContext(Cef3Binder.MyCefJs_GetEnteredContext());
        }

        public void EnterContext()
        {
            Cef3Binder.MyCefJs_EnterContext(this.Ptr);
        }
        public void ExitContext()
        {
            Cef3Binder.MyCefJs_ExitContext(this.Ptr);
        }
    }

    public class CefV8Value : Cef3RefCountingValue
    {
        public CefV8Value(IntPtr ptr) : base(ptr)
        {
            if (ptr == IntPtr.Zero)
            {
            }
        }
        public void Set(string key, Cef3Func cef3Func)
        {
            Cef3Binder.MyCefJs_CefV8Value_SetValue_ByString(this.Ptr, key, cef3Func.Ptr, (int)CefV8PropertyAttribute.V8_PROPERTY_ATTRIBUTE_READONLY);
        }
        public bool IsFunc()
        {
            return Cef3Binder.MyCefJs_CefV8Value_IsFunc(this.Ptr);
        }
        public string ReadValueAsString()
        {
            const int BUFF_LEN = 512;
            char[] charBuff = new char[BUFF_LEN];
            unsafe
            {
                fixed (char* head = &charBuff[0])
                {
                    int actualLen = 0;
                    Cef3Binder.MyCefJs_CefV8Value_ReadAsString(this.Ptr, head, BUFF_LEN, ref actualLen);
                    if (actualLen > BUFF_LEN)
                    {
                        //read more
                    }
                    return new string(charBuff, 0, actualLen);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct JsValue
    {
        [FieldOffset(0)]
        public int I32;
        [FieldOffset(0)]
        public long I64;
        [FieldOffset(0)]
        public double Num;
        /// <summary>
        /// ptr from native side
        /// </summary>
        [FieldOffset(0)]
        public IntPtr Ptr;
        /// <summary>
        /// offset(8)See JsValueType, marshaled as integer. 
        /// </summary>
        [FieldOffset(8)]
        public JsValueType Type;
        /// <summary>
        /// offset(12) Length of array or string 
        /// </summary>
        [FieldOffset(12)]
        public int Length;
        /// <summary>
        /// offset(12) managed object keepalive index. 
        /// </summary>
        [FieldOffset(12)]
        public int Index;
        public static JsValue Null
        {
            get { return new JsValue() { Type = JsValueType.Null }; }
        }

        public static JsValue Empty
        {
            get { return new JsValue() { Type = JsValueType.Empty }; }
        }

        public static JsValue Error(int slot)
        {
            return new JsValue { Type = JsValueType.ManagedError, Index = slot };
        }

        public override string ToString()
        {
            return string.Format("[JsValue({0})]", Type);
        }
    }
    public enum JsValueType
    {
        UnknownError = -1,
        Empty = 0,
        Null = 1,
        Boolean = 2,
        Integer = 3,
        Number = 4,
        String = 5,
        Date = 6,
        Index = 7,
        Array = 10,
        StringError = 11,
        Managed = 12,
        ManagedError = 13,
        Wrapped = 14,
        Dictionary = 15,
        Error = 16,
        Function = 17,
        //---------------
        //my extension
        JsTypeWrap = 18
    }
    public class NativeBrowser : Cef3RefCountingValue
    {
        public NativeBrowser(IntPtr ptr) : base(ptr)
        {
        }
        public void ExecJavascript(string src, string url)
        {
            Cef3Binder.MyCefBwExecJavascript2(this.Ptr, src, url);
        }
    }
    public class NativeFrame : Cef3RefCountingValue
    {
        public NativeFrame(IntPtr ptr) : base(ptr)
        {
        }
        public NativeJsContext GetFrameContext()
        {
            return new NativeJsContext(Cef3Binder.MyCefJsFrameContext(this.Ptr));
        }
        public string GetUrl()
        {
            unsafe
            {
                char[] buffer = new char[255];
                int actualLength = 0;
                fixed (char* buffer_head = &buffer[0])
                {
                    Cef3Binder.MyCefFrame_GetUrl(Ptr, buffer_head, 255, ref actualLength);
                    return new string(buffer_head);
                }
            }
        }
    }
    public class NativeRendererApp : Cef3RefCountingValue
    {
        public NativeRendererApp(IntPtr ptr) : base(ptr)
        {
        }
    }
    public class NativeResourceMx : Cef3RefCountingValue
    {
        public NativeResourceMx(IntPtr ptr) : base(ptr)
        {
        }
        public void AddResourceProvider(ResourceProvider resProvider)
        {
        }
    }
    public class ResourceProvider
    {
    }
}