import * as signalR from "@microsoft/signalr";

let connection = null;

export async function startSignalR(token) {

    if (connection)
        return connection;

    connection =
        new signalR.HubConnectionBuilder()

        .withUrl(
            "https://localhost:7129/notificationHub",
            {
                accessTokenFactory: () => token
            })

        .withAutomaticReconnect()

        .build();

    await connection.start();

    console.log("SignalR Connected");

    return connection;
}

export function getConnection() {
    return connection;
}