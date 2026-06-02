#nullable enable
#if ANDROID
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Droid.Input;

internal class NativeTextInputServiceDroid(Activity activity) : INativeTextInputService
{
    private NativeInputView? _inputView;
    private NativeTextInputDroid? _current;
    private INativeTextInputConsumer? _currentConsumer;

    private RectangleF? _newBounds;
    
    public INativeTextInput AllocateTextInput(INativeTextInputConsumer consumer, InputType inputType, RectangleF bounds, int cursorPosition = 0)
    {
        _current?.Dispose();
        _currentConsumer = consumer;
        var input = new NativeTextInputDroid(this);
        _current = input;

        activity.RunOnUiThread(() =>
        {
            if (_inputView == null)
            {
                _inputView = new NativeInputView(activity, this);
                activity.AddContentView(_inputView, new ViewGroup.MarginLayoutParams(1, 1));
            }

            SetProperPosition(bounds);
            
            activity.Window?.SetSoftInputMode(SoftInput.AdjustPan);
            _inputView.RequestFocus();
                        
            var imm = (InputMethodManager)activity.GetSystemService(Context.InputMethodService)!;
            imm.ShowSoftInput(_inputView, ShowFlags.Forced);
        });
        return input;
    }

    internal void OnDisposed(NativeTextInputDroid input)
    {
        if (_current != input) return;
        _current = null;
        var consumer = _currentConsumer;
        _currentConsumer = null;

        activity.RunOnUiThread(() =>
        {
            if (_inputView != null)
            {
                var imm = (InputMethodManager)activity.GetSystemService(Context.InputMethodService)!;
                imm.HideSoftInputFromWindow(_inputView.WindowToken, 0);
            }
            activity.Window?.SetSoftInputMode(SoftInput.AdjustUnspecified);
        });

        consumer?.OnTextInputClosed();
    }

    internal bool HandleKeyEvent(KeyEvent e)
    {
        if (_currentConsumer == null || e.Action != KeyEventActions.Down) return false;

        switch (e.KeyCode)
        {
            case Keycode.Del:
                _currentConsumer.OnKey(Key.Backspace);
                return true;

            case Keycode.Enter:
            case Keycode.NumpadEnter:
                _currentConsumer.OnKey(Key.Enter);
                return true;

            default:
                var unicode = e.GetUnicodeChar(e.MetaState);
                if (unicode != 0)
                    _currentConsumer.OnTextInput(((char)unicode).ToString());
                return true;
        }
    }

    private void SetProperPosition(RectangleF bounds)
    {
        if (_inputView?.LayoutParameters is not ViewGroup.MarginLayoutParams lp) return;

        lp.SetMargins((int)bounds.X, (int)(bounds.Y + bounds.Height), 0, 0);
        lp.Width = Math.Max(1, (int)bounds.Width);
        lp.Height = 1;
        _inputView.LayoutParameters = lp;
        _inputView.RequestLayout();
    }

    internal void UpdatePosition(RectangleF bounds, int _)
    {
        activity.RunOnUiThread(() =>
        {
            SetProperPosition(bounds);
            ApplyRequestRectangleOnScreen();
        });
    }

    private void ApplyRequestRectangleOnScreen()
    {
        if (_inputView?.LayoutParameters is not ViewGroup.MarginLayoutParams lp) return;
        _inputView.RequestRectangleOnScreen(new Android.Graphics.Rect(0, 0, lp.Width, 1), true);
    }

    internal void OnText(string text) => _currentConsumer?.OnTextInput(text);
    internal void OnBackspace() => _currentConsumer?.OnKey(Key.Backspace);
    internal void OnEnter() => _currentConsumer?.OnKey(Key.Enter);

    private class NativeInputView : View, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly NativeTextInputServiceDroid _service;
        private int _lastVisibleHeight;

        public NativeInputView(Context context, NativeTextInputServiceDroid service) : base(context)
        {
            _service = service;
            Focusable = true;
            FocusableInTouchMode = true;
            ViewTreeObserver?.AddOnGlobalLayoutListener(this);
        }

        public void OnGlobalLayout()
        {
            var r = new Android.Graphics.Rect();
            GetWindowVisibleDisplayFrame(r);
            var visibleHeight = r.Height();

            if (visibleHeight != _lastVisibleHeight)
            {
                _lastVisibleHeight = visibleHeight;
                _service.ApplyRequestRectangleOnScreen();
            }
        }

        public override IInputConnection? OnCreateInputConnection(EditorInfo? outAttrs)
        {
            if (outAttrs != null)
            {
                outAttrs.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextFlagNoSuggestions;
            }
            return new NativeInputConnection(this, _service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ViewTreeObserver?.RemoveOnGlobalLayoutListener(this);
            base.Dispose(disposing);
        }
    }

    private class NativeInputConnection : BaseInputConnection
    {
        private readonly NativeTextInputServiceDroid _service;

        public NativeInputConnection(View targetView, NativeTextInputServiceDroid service) : base(targetView, false)
        {
            _service = service;
        }

        public override bool CommitText(Java.Lang.ICharSequence? text, int newCursorPosition)
        {
            var str = text?.ToString();
            if (!string.IsNullOrEmpty(str))
                _service.OnText(str);
            return true;
        }

        public override bool DeleteSurroundingText(int beforeLength, int afterLength)
        {
            for (var i = 0; i < beforeLength; i++)
                _service.OnBackspace();
            return true;
        }

        public override bool SendKeyEvent(KeyEvent? e)
        {
            if (e?.Action == KeyEventActions.Down)
            {
                switch (e.KeyCode)
                {
                    case Keycode.Del:
                        _service.OnBackspace();
                        return true;
                    case Keycode.Enter:
                    case Keycode.NumpadEnter:
                        _service.OnEnter();
                        return true;
                }
            }
            return base.SendKeyEvent(e);
        }

        public override bool PerformEditorAction(ImeAction actionCode)
        {
            _service.OnEnter();
            return true;
        }
    }
}
#endif
