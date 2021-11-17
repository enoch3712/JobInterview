import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";

export const connection = new HubConnectionBuilder()
    .withUrl("https://localhost:44332/hubs/assets", {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets})
    .withAutomaticReconnect()
    .build();

export async function startSignalR() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(startSignalR, 5000);
    }
};

connection.onclose(async () => {
    await startSignalR();
});

// Start the connection.
startSignalR();