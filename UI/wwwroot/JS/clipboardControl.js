export async function GetClipboardContents() {
    return await navigator.clipboard.readText();
}