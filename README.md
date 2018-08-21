# Getting Started

## Installation 

* Download and install the latest scanx app from here [here](https://github.com/balbarak/scanx/releases/download/v1.0.1/Scanx.v1.0.1.msi). 

    * After installed go to http://localhost:61234 to make sure the protocol is running.
    * This app is must be installed in clients pc in order to control their devices and printers.

### To use it in your project 

Include:

``` javascript

<script src="/path/to/jquery.js"></script> 

<script src="/path/to/signalr.min.js"></script> 

<script src="/path/to/scanx.js"></script>
```
files:

* [jquery](https://code.jquery.com/jquery-3.3.1.min.js)
* [scanx.js](https://github.com/balbarak/scanx/blob/master/src/ScanX.Protocol/wwwroot/js/scanx.js)
* [signalr.min.js](https://github.com/balbarak/scanx/blob/master/src/ScanX.Protocol/wwwroot/js/signalr.min.js)

## Usage

### html

``` html
<img id="image" alt="scanned image" />

<button type="button" onclick="startScan()">
    Scan
</button>

```

### javascript
``` javascript
<script type="text/javascript">

// declar scanx class
var scan = new ScanX();


//setup on image scan events
scan.connection.on("OnImageScanned", (data) => { 
    console.log('image event');
     var image = document.getElementById('image'); 
     image.src = "data:image/jpeg;base64," + data.imageBytes; 
});

//get installed scanners and select the first id
var firstScannerId = scan.getScanners()[0].deviceId;

//define scan settings
var settings = {
    color: 1, //color mode 1 color, 2 Grayscale, 4 Black and white
    dpi: 200 //dpi
}

//Scan first imaga
function startScan(){
    scan.scanSingle(firstScannerId,settings);
}

//Connect to the protocol
scan.connect();


</script>

```
