using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using System.Reflection;

namespace BistroQ.Presentation.Helpers;

public static class CursorUtilities
{
    public static void ChangeCursor(this UIElement element, CursorType cursorType)
    {
        ArgumentNullException.ThrowIfNull(element);

        InputCursor? cursor = null;
        switch (cursorType)
        {
            case CursorType.Arrow:
                cursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
                break;

            case CursorType.Hand:
                cursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
                break;

            default:
                cursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
                break;
        }

        typeof(UIElement).InvokeMember("ProtectedCursor", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, element, new[] { cursor });
    }
}

public enum CursorType
{
    Hand,
    Arrow,
}