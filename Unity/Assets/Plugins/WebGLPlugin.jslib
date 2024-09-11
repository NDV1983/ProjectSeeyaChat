mergeInto(LibraryManager.library, {
    JS_SendChatHistory: function(jsonChatHistory) {
        var jsonStr = UTF8ToString(jsonChatHistory);
        if (typeof window.ReceiveChatHistory === 'function') {
            window.ReceiveChatHistory(jsonStr);
        } else {
            console.error('ReceiveChatHistory function is not defined in the window object.');
        }
    }
});
