﻿#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   2019/12/13 16:37
// Modified On:  2020/01/13 12:45
// Modified By:  Alexis

#endregion




using SuperMemoAssistant.SuperMemo;
using System;

namespace SuperMemoAssistant.SMA
{
  public static class CoreEx
  {
    #region Methods

    public static int ExecuteOnMainThread(
      this   NativeMethod method,
      params dynamic[]    parameters)
    {
      dynamic _unused = null;
      return Core.Hook.ExecuteOnMainThread(method, parameters, out _unused);
    }

    public static int ExecuteOnMainThreadWithOutParameter(
      this   NativeMethod method,
      out    dynamic      outParameter,
      params dynamic[]    parameters)
    {
      return Core.Hook.ExecuteOnMainThread(method, parameters, out outParameter);
    }

    #endregion
  }
}
