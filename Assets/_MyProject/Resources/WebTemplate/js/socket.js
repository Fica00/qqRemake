let connection;
const createAndSetupConnection = async (token) => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("https://api.qoomonquest.com/hubs/game", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
            accessTokenFactory: () => token
        })
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on('UserDisconnectedAsync', (userName) => {
        console.log("User disconnected: " + userName);
        unity.SendMessage("SocketCommunication", "UserDisconnectedAsync", userName);
    });

    connection.on('UserLeftAsync', (userName) => {
        console.log("User left: " + userName);
        unity.SendMessage("SocketCommunication", "UserLeftAsync", userName);
    });

    connection.on('UserRejoinedAsync', (userName) => {
        console.log("User rejoined: " + userName);
        unity.SendMessage("SocketCommunication", "UserRejoinedAsync", userName);
    });

    connection.on('ReceiveOldMessagesAsync', (messages) => {
        console.log("ReceiveOldMessagesAsync: " + messages);
        unity.SendMessage("SocketCommunication", "ReceiveOldMessagesAsyncJson", JSON.stringify(messages));
    });

    connection.on('ReceiveMessageAsync', (message) => {
        console.log("ReceiveMessageAsync: " + message);
        unity.SendMessage("SocketCommunication", "ReceiveMessageAsync", message);
    });

    connection.on('MatchFoundAsync', (roomName, firstPlayer, secondPlayer) => {
        console.log("MatchFoundAsync: " + roomName + " " + firstPlayer + " " + secondPlayer);
        unity.SendMessage("SocketCommunication", "MatchFoundAsyncJson", JSON.stringify({ roomName, firstPlayer, secondPlayer }));
    });

    connection.on('MatchMakingStartedAsync', () => {
        console.log("MatchMakingStartedAsync");
        unity.SendMessage("SocketCommunication", "MatchMakingStartedAsync");
    });

    connection.on('MatchMakingCanceledAsync', () => {
        console.log("MatchMakingCanceledAsync");
        unity.SendMessage("SocketCommunication", "MatchMakingCanceledAsync");
    });

    connection.on('MatchLeftAsync', (success) => {
        console.log("MatchLeftAsync: " + success);

        const successInt = success ? 1 : 0;
        unity.SendMessage("SocketCommunication", "MatchLeftAsyncFromJs", successInt);
    });

    await start();
};

const start = async () => {
    try {
        await connection.start();
        unity.SendMessage("SocketCommunication", "ReceiveConnectionOutcome", 1);
        console.log("SignalR Connected.");
    } catch (err) {
        console.log("GRESKA" + err);
        unity.SendMessage("SocketCommunication", "ReceiveConnectionOutcome", 0);
        setTimeout(start, 5000);
    }
};

const sendMessage = async (roomName, message) => {

    console.log("sendMessage js: " + roomName + " " + message );

    await connection.send('SendMessageAsync', roomName, message)
}

const leaveMatch = async () => {

    console.log("leaveMatch js");

    await connection.send('LeaveMatchAsync')
}

const matchMakeAsync = async () => {

    console.log("matchMakeAsync js");
    await connection.send('MatchMakeAsync')
}

const cancelMatchMake = async () => {
    console.log("cancelMatchMake js");
    await connection.send('CancelMatchMakeAsync')
}