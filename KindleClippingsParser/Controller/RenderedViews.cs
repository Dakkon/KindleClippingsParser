using System;

namespace KindleClippingsParser.Controller
{
    [Flags]
    public enum RenderedViews
    {
        EditView = 1,
        TextPageView = 2,
        MCFileView = 4
    }
}
