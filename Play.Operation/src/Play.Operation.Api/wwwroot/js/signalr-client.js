const connection = new signalR.HubConnectionBuilder()
    .withUrl("/commandStatusHub") // Connect to the backend hub
    .build();

connection.start()
    .then(() => console.log("Connected to SignalR Hub"))
    .catch(err => console.error("Error connecting to SignalR Hub:", err));

// Listen for updates from the server
connection.on("CommandStatusUpdated", (correlationId, status, reason) => {
    console.log(`Command ${correlationId} status: ${status}`);
    document.getElementById("command-status").innerText = status;

    const reasonElement = document.getElementById("command-reason");
    if (reason) {
        reasonElement.innerText = `Reason: ${reason}`;
    } else {
        reasonElement.innerText = "";
    }
});