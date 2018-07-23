
$(function () {
    
})


class ScanX {

    constructor() {

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:61234/scanx")
            .configureLogging(signalR.LogLevel.Information)
            .build();
    }

    connect() {

        this.connection.start().catch(err => console.error(err.toString()));
    }

    scanSingle(deviceId,settings) {

        this.connection.invoke("ScanSingle",deviceId,settings).catch(err => console.error(err.toString()));
    }

    scanMultiple(deviceId,settings) {

        this.connection.invoke("ScanMultiple", deviceId, settings).catch(err => console.error(err.toString()));
    }

    scanTest() {
        this.connection.invoke("ScanTest").catch(err => console.error(err.toString()));
    }
}