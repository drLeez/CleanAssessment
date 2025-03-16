using Microsoft.JSInterop;

namespace CleanAssessment.Helpers
{
    public class KeyboardHelper : IKeyboardHelper
    {
        private readonly DotNetObjectReference<KeyboardHelper>? _dotNetObjectReference;
        private readonly IJSRuntime _jsRuntime;
        public KeyboardHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _dotNetObjectReference = DotNetObjectReference.Create(this);

            _jsRuntime.InvokeVoidAsync("InitKeyboardListeners", _dotNetObjectReference);
        }

        private readonly Dictionary<string, bool> _heldMap = [];
        private readonly Dictionary<string, bool> _pressMap = [];

        public Func<string, Task> OnKeyDown { get; set; }
        public Func<string, Task> OnKeyUp { get; set; }

        [JSInvokable]
        public async void KeyDown(string keyCode)
        {
            _heldMap[keyCode] = true;
            _pressMap[keyCode] = true;
            if (OnKeyDown != null) await OnKeyDown(keyCode);
            ResetPressMap(keyCode); // don't await this!
        }
        [JSInvokable]
        public async void KeyUp(string keyCode)
        {
            _heldMap[keyCode] = false;
            _pressMap[keyCode] = false;
            if (OnKeyUp != null) await OnKeyUp(keyCode);
        }

        private async Task ResetPressMap(string keyCode)
        {
            await Task.Delay(10);
            _pressMap[keyCode] = false;
        }

        public bool IsKeyHeld(string keyCode) => _heldMap.TryGetValue(keyCode, out var held) && held;
        public bool IsKeyPressed(string keyCode) => _pressMap.TryGetValue(keyCode, out var press) && press;

        //public async void Dispose()
        //{
        //    await _jsRuntime.InvokeVoidAsync("RemoveKeyboardListeners", _dotNetObjectReference);
        //    _dotNetObjectReference?.Dispose();
        //}
    }
}
