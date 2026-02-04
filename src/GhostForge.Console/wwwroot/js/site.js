// GhostForge JavaScript 辅助函数

/**
 * 下载文件到本地
 * @param {string} filename - 文件名
 * @param {string} base64Content - Base64编码的文件内容
 */
window.downloadFile = function (filename, base64Content) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = 'data:text/plain;base64,' + base64Content;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

/**
 * 复制文本到剪贴板（备用方法，如果 navigator.clipboard 不可用）
 * @param {string} text - 要复制的文本
 */
window.copyToClipboard = function (text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
        return navigator.clipboard.writeText(text);
    } else {
        // 备用方法
        const textArea = document.createElement('textarea');
        textArea.value = text;
        textArea.style.position = 'fixed';
        textArea.style.left = '-9999px';
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();

        try {
            document.execCommand('copy');
            document.body.removeChild(textArea);
            return Promise.resolve();
        } catch (err) {
            document.body.removeChild(textArea);
            return Promise.reject(err);
        }
    }
};

console.log('🔧 GhostForge JavaScript loaded');
