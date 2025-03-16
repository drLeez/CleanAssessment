window.InitKeyboardListeners = (dotNetHelper) => {
    if (!window.keyboardListeners) {
        window.keyboardListeners = {};
    }
    var downHandler = function (event) {
        dotNetHelper.invokeMethodAsync('KeyDown', event.key);
    }
    document.addEventListener('keydown', downHandler);
    var upHandler = function (event) {
        dotNetHelper.invokeMethodAsync('KeyUp', event.key);
    }
    document.addEventListener('keyup', upHandler);
    console.log(dotNetHelper);
    window.keyboardListeners[dotNetHelper] = [downHandler, upHandler];
}

window.RemoveKeyboardListeners = (dotNetHelper) => {
    if (!window.keyboardListeners) {
        return;
    }

    var handlers = window.keyboardListeners[dotNetHelper];
    if (handlers) {
        document.removeEventListener('keydown', handlers[0]);
        document.removeEventListener('keyup', handlers[1]);
    }
}