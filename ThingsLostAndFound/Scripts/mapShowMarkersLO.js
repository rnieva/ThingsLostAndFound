// show the markers with popup with Details and Contact links
//var map;
//var marker;
function showMarkersLO(objectList) {
    var bounds = new google.maps.LatLngBounds();
    var la2 = [];
    var lo2 = [];
    var info2 = [];
    var titles2 = [];
    var infohtml2;
    var index;
    for (index = 0; index < objectList.length; ++index) {
        la2.push(objectList[index].Latitude);
        lo2.push(objectList[index].Longitude);
        titles2.push(objectList[index].Title);
        if (objectList[index].Img == true) {
            infohtml2 = "<div><b>Lost Object</b><h2>" + objectList[index].Title + "</h2></div><p>" + objectList[index].Category + "<p> " + objectList[index].Address + "<p>" + objectList[index].Location + "<p> <p><img src=/File?id=" + objectList[index].FileId + " alt= height=42 width=42 /></p>" + "<p> <p><a href=/LostObjects/Details/" + objectList[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserLostObject/" + objectList[index].Id + "?title=" + objectList[index].Title + "&userName=" + objectList[index].UserNameReport + ">Contact</a></p>";
        } else {
            infohtml2 = "<div><b>Lost Object</b><h2>" + objectList[index].Title + "</h2></div><p>" + objectList[index].Category + "<p> " + objectList[index].Address + "<p>" + objectList[index].Location + "<p> <p><a href=/LostObjects/Details/" + objectList[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserLostObject/" + objectList[index].Id + "?title=" + objectList[index].Title + "&userName=" + objectList[index].UserNameReport + ">Contact</a></p>"; // when I had this script in the same .cshtml with the View, I used Razor (<text>>razor code</text>) @ (+ `@Html.ActionLink("More Info", "Details", "FoundObjects", new { id = @marker.Id }, null)` + "<p>" + `@Html.ActionLink("Contact", "ContactUserFoundObject", "UsersContact", new { id = @marker.Id, title = @marker.Title, userName = @marker.UserNameReport, securityQuestion = @marker.SecurityQuestion  }, null)`;  )

        }
        info2.push(infohtml2);

    }
    var infoWindow = new google.maps.InfoWindow({ content: "Loading..." });
    var marker, i;
    for (i = 0; i < la2.length; i++) // for example I choose as index the latitude
    {
        var point2 = new google.maps.LatLng(la2[i], lo2[i]);
        bounds.extend(point2);
        marker = new google.maps.Marker({
            position: point2,
            title: titles2[i],
            icon: 'http://maps.google.com/mapfiles/ms/icons/red-dot.png',
            map: map
        });
        google.maps.event.addListener(marker, 'click', (function (marker, i) //Closures
        {
            return function () {
                infoWindow.setContent(info2[i]);
                infoWindow.open(map, marker);
            }
        })(marker, i)); //Closures
    }
}