//Initiate the map and add a marker to the map. It show on Object Details

var map;

function initMapAndaddMarker(mapLocation, title) {
    var latlngStr = (mapLocation).split(',', 2);
    var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };
    var options = {
        zoom: 8,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map($('#map_canvas')[0], options);
    
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: title
    });
}


    


