// initiate the map. Add a marker to get the Location Info using Lat-Long, or Add a marker if the user write a location
initMap()
var map;
var marker;

function initMap() {
    var centerPosition = new google.maps.LatLng(55.942071, -3.200165);
    var options = {
        zoom: 11,
        center: centerPosition,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        styles: [   //remove POI from the Google Maps API
                {
                    "featureType": "poi",
                    "stylers": [
                      { "visibility": "off" }
                    ]
                }
                    ]
    };
    map = new google.maps.Map($('#map_canvas')[0], options);
    google.maps.event.addListener(map, 'click', function (event) {
        addMarker(event.latLng);
    });
    if (navigator.geolocation) //center the map in user location
    {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            map.setCenter(pos);
        }, function () {
            handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, infoWindow, map.getCenter());
    }
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
                          'Error: The Geolocation service failed.' :
                          'Error: Your browser doesn\'t support geolocation.');
}

function addMarker(location) {
    if (marker) {
        //if marker already was created change positon
    } else {
        //create a marker
        marker = new google.maps.Marker({
            position: location,
            map: map,
            draggable: true
        });
    }
    marker.setPosition(location);
    var lat = marker.getPosition().lat().toFixed(6);
    var lng = marker.getPosition().lng().toFixed(6);
    var mapLocationForm = document.getElementById("MapLocation");
    mapLocationForm.value = lat + "," + lng;
    var geocoder = new google.maps.Geocoder;
    var latlngStr = (mapLocationForm.value).split(',', 2);
    geocodeLatLng(latlngStr, geocoder);
}

function geocodeLatLng(latlngStr, geocoder) { //get info from lat and lng
    var AddressForm = document.getElementById('Address');
    var LocationForm = document.getElementById('Location');
    var ZipCodeForm = document.getElementById('ZipCode');
    var CountryForm = document.getElementById('Country');
    var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };
    geocoder.geocode({ 'location': latlng }, function (results, status) {
        var resultTemp = results;
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                AddressForm.value = results[0].formatted_address;  //TODO: add if para controlar los resultados
                for (var i = 0; i < results[0].address_components.length; i++) {
                    for (var j = 0; j < results[0].address_components[i].types.length; j++) {
                        if (results[0].address_components[i].types[j] == "country") {
                            CountryForm.value = results[0].address_components[i].long_name;
                        }
                        if (results[0].address_components[i].types[j] == "postal_code") {
                            ZipCodeForm.value = results[0].address_components[i].long_name;
                        }
                        if (results[0].address_components[i].types[j] == "locality") {
                            LocationForm.value = results[0].address_components[i].long_name;
                        }
                    }
                }
            } else {
                window.alert('No results found');
                AddressForm.value = "";
                LocationForm.value = "";
                ZipCodeForm.value = "";
                CountryForm.value = "";
            }
        } else {
            window.alert('Geocoder failed due to: ' + status);
            AddressForm.value = "";
            LocationForm.value = "";
            ZipCodeForm.value = "";
            CountryForm.value = "";
        }
    });

}

//This functions add a marker if the user type the ZipCode or the Address

function addMarkerFromZipCode() {
    var geocoder = new google.maps.Geocoder;
    var ZipCode = document.getElementById('ZipCode').value;
    geocoder.geocode({ 'address': ZipCode }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            addMarker(results[0].geometry.location)
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}

function addMarkerFromAddress() {
    var geocoder = new google.maps.Geocoder;
    var address = document.getElementById('Address').value;
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            addMarker(results[0].geometry.location)
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}