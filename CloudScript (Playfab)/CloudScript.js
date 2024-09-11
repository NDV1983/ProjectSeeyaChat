
handlers.GetChatHistory = function (args, context) {
    // Fetch chat history logic
    try {
        var chatHistory = server.GetTitleData({ "Keys": ["ChatHistory"] });

        return {
            success: true,
            chatHistory: chatHistory.Data.ChatHistory
        };
    } catch (e) {
        return {
            success: false,
            error: e
        };
    }
};


handlers.SendMessage = function (args, context) {
    var message = args.message;
    var playerId = currentPlayerId;
    var timestamp = new Date().toISOString();

    var chatMessage = {
        playerId: playerId,
        message: message,
        timestamp: timestamp
    };

    var chatHistory = server.GetTitleData({ Keys: ["ChatHistory"] }).Data["ChatHistory"];
    chatHistory = chatHistory ? JSON.parse(chatHistory) : [];

    chatHistory.push(chatMessage);

    server.SetTitleData({
        Key: "ChatHistory",
        Value: JSON.stringify(chatHistory)
    });

    return { result: "Message sent successfully", chatMessage: chatMessage };
};


handlers.addPlayerId = function (args, context) {
    var playerId = args.playerId;
    var titleData = server.GetTitleData({
        Keys: ["PlayerList"]
    });

    var playerList;
    if (titleData.Data.PlayerList) {
        playerList = JSON.parse(titleData.Data.PlayerList);
    } else {
        playerList = { players: [] };
    }

    if (playerList.players.indexOf(playerId) === -1) {
        playerList.players.push(playerId);
    }

    server.SetTitleData({
        Key: "PlayerList",
        Value: JSON.stringify(playerList)
    });

    return { success: true };
};