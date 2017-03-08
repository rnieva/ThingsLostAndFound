//add the markers of the FO and LO objects
function showMarkersFOLO(objectListFO, objectListLO) {
    var bounds = new google.maps.LatLngBounds();
    var la = [];
    var lo = [];
    var info = [];
    var titles = [];
    var securityQuestion;
    var infohtml;
    var index;
    for (index = 0; index < objectListFO.length; ++index) {
        la.push(objectListFO[index].Latitude);
        lo.push(objectListFO[index].Longitude);
        titles.push(objectListFO[index].Title);
        securityQuestion = encodeURIComponent((objectListFO[index].SecurityQuestion).trim()); //to add %20 in spaces for a corrent info
        if (objectListFO[index].Img == true) {
            infohtml = "<div><b>Found Object</b><h2>" + objectListFO[index].Title + "</h2></div><p>" + objectListFO[index].Category + "<p> " + objectListFO[index].Address + "<p>" + objectListFO[index].Location + "<p> <p><img src=/File?id=" + objectListFO[index].FileId + " alt= height=42 width=42 /></p>" + "<p> <p><a href=/FoundObjects/Details/" + objectListFO[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserFoundObject/" + objectListFO[index].Id + "?title=" + objectListFO[index].Title + "&userName=" + objectListFO[index].UserNameReport + "&securityQuestion=" + securityQuestion + ">Contact</a></p>";
        } else {
            infohtml = "<div><b>Found Object</b><h2>" + objectListFO[index].Title + "</h2></div><p>" + objectListFO[index].Category + "<p> " + objectListFO[index].Address + "<p>" + objectListFO[index].Location + "<p> <p><a href=/FoundObjects/Details/" + objectListFO[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserFoundObject/" + objectListFO[index].Id + "?title=" + objectListFO[index].Title + "&userName=" + objectListFO[index].UserNameReport + "&securityQuestion=" + securityQuestion + ">Contact</a></p>"; // when I had this script in the same .cshtml with the View, I used Razor (<text>>razor code</text>) @ (+ `@Html.ActionLink("More Info", "Details", "FoundObjects", new { id = @marker.Id }, null)` + "<p>" + `@Html.ActionLink("Contact", "ContactUserFoundObject", "UsersContact", new { id = @marker.Id, title = @marker.Title, userName = @marker.UserNameReport, securityQuestion = @marker.SecurityQuestion  }, null)`;  )

        }
        info.push(infohtml);

    }
    var infoWindow = new google.maps.InfoWindow({ content: "Loading..." });
    var marker, i;
    for (i = 0; i < la.length; i++) // for example I choose as index the latitude
    {
        var point = new google.maps.LatLng(la[i], lo[i]);
        bounds.extend(point);
        marker = new google.maps.Marker({
            position: point,
            title: titles[i],
            icon: 'http://maps.google.com/mapfiles/ms/icons/green-dot.png',
            map: map
        });
        google.maps.event.addListener(marker, 'click', (function (marker, i) //Closures
        {
            return function () {
                infoWindow.setContent(info[i]);
                infoWindow.open(map, marker);
            }
        })(marker, i)); //Closures
    }

    var la2 = [];
    var lo2 = [];
    var info2 = [];
    var titles2 = [];
    var infohtml2;
    var index;
    for (index = 0; index < objectListLO.length; ++index) {
        la2.push(objectListLO[index].Latitude);
        lo2.push(objectListLO[index].Longitude);
        titles2.push(objectListLO[index].Title);
        if (objectListLO[index].Img == true) {
            infohtml2 = "<div><b>Lost Object</b><h2>" + objectListLO[index].Title + "</h2></div><p>" + objectListLO[index].Category + "<p> " + objectListLO[index].Address + "<p>" + objectListLO[index].Location + "<p> <p><img src=/File?id=" + objectListLO[index].FileId + " alt= height=42 width=42 /></p>" + "<p> <p><a href=/LostObjects/Details/" + objectListLO[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserLostObject/" + objectListLO[index].Id + "?title=" + objectListLO[index].Title + "&userName=" + objectListLO[index].UserNameReport + ">Contact</a></p>";
        } else {
            infohtml2 = "<div><b>Lost Object</b><h2>" + objectListLO[index].Title + "</h2></div><p>" + objectListLO[index].Category + "<p> " + objectListLO[index].Address + "<p>" + objectListLO[index].Location + "<p> <p><a href=/LostObjects/Details/" + objectListLO[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserLostObject/" + objectListLO[index].Id + "?title=" + objectListLO[index].Title + "&userName=" + objectListLO[index].UserNameReport + ">Contact</a></p>"; // when I had this script in the same .cshtml with the View, I used Razor (<text>>razor code</text>) @ (+ `@Html.ActionLink("More Info", "Details", "FoundObjects", new { id = @marker.Id }, null)` + "<p>" + `@Html.ActionLink("Contact", "ContactUserFoundObject", "UsersContact", new { id = @marker.Id, title = @marker.Title, userName = @marker.UserNameReport, securityQuestion = @marker.SecurityQuestion  }, null)`;  )

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