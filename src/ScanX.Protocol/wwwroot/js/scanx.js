const url = "";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:61234/scanx")
    .configureLogging(signalR.LogLevel.Information)
    .build();


$(function () {
    connection.start().catch(err => console.error(err.toString()));
})