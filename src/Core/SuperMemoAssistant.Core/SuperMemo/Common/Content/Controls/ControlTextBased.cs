#region License & Metadata

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

#endregion




namespace SuperMemoAssistant.SuperMemo.Common.Content.Controls
{
  using System;
  using Interop.SuperMemo.Content.Controls;
  using Interop.SuperMemo.Content.Models;
  using Interop.SuperMemo.Registry.Members;
  using SMA;

  public abstract class ControlTextBased : ComponentControlBase, IControlText
  {
    #region Constructors

    /// <inheritdoc />
    protected ControlTextBased(int           id,
                               ComponentType type,
                               ControlGroup  group)
      : base(id, type, group) { }

    #endregion




    #region Properties Impl - Public

    public virtual string Text
    {
      get => Group.GetText(this);
      set => Group.SetText(this, value);
    }

    public virtual IText TextMember
    {
      get => Core.SM.Registry.Text[TextMemberId];
      set => TextMemberId = value?.Id ?? throw new ArgumentNullException(nameof(value));
    }

    public virtual int TextMemberId
    {
      get => Group.GetTextRegMember(this);
      set => Group.SetTextRegMember(this, value);
    }

    #endregion
  }
}
