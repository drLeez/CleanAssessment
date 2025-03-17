window.GetCookies = () => {
    return document.cookie;
}
window.SetCookie = (key, value) => {
    let age = 60 * 60 * 24 * 365;
    document.cookie = `${key}=${value};max-age=${age}`;
}
window.ClearCookie = (key) => {
    document.cookie = `${key}=garbage;max-age=${1}`;
}