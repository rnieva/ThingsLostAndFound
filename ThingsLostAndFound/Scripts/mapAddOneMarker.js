//Add a marker to the map. It show on Object Details
function addMarker(mapLocation,title) {
    var latlngStr = (mapLocation).split(',', 2);
    var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: title
    });
    map.setCenter(latlng);
    map.setZoom(8);
}
