// show the markers with popup with Details and Contact links
//var map;
//var marker;

function showMarkersFO(objectList) {
    var bounds = new google.maps.LatLngBounds();
    var la = [];
    var lo = [];
    var info = [];
    var titles = [];
    var securityQuestion;
    var infohtml;
        var index;
        for (index = 0; index < objectList.length; ++index)
        {
            la.push(objectList[index].Latitude);
            lo.push(objectList[index].Longitude);
            titles.push(objectList[index].Title);
            securityQuestion = encodeURIComponent((objectList[index].SecurityQuestion).trim()); //to add %20 in spaces for a corrent info
            if (objectList[index].Img == true)
        { 
                infohtml = "<div><b>Found Object</b><h2>" + objectList[index].Title + "</h2></div><p>" + objectList[index].Category + "<p> " + objectList[index].Address + "<p>" + objectList[index].Location + "<p> <p><img src=/File?id=" + objectList[index].FileId + " alt= height=42 width=42 /></p>" + "<p> <p><a href=/FoundObjects/Details/" + objectList[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserFoundObject/" + objectList[index].Id + "?title=" + objectList[index].Title + "&userName=" + objectList[index].UserNameReport + "&securityQuestion="+securityQuestion+">Contact</a></p>";
        }else
            {
                infohtml = "<div><b>Found Object</b><h2>" + objectList[index].Title + "</h2></div><p>" + objectList[index].Category + "<p> " + objectList[index].Address + "<p>" + objectList[index].Location + "<p> <p><a href=/FoundObjects/Details/" + objectList[index].Id + ">More Info</a></p>" + "<p> <p><a href=/UsersContact/ContactUserFoundObject/" + objectList[index].Id + "?title=" + objectList[index].Title + "&userName=" + objectList[index].UserNameReport + "&securityQuestion=" + securityQuestion + ">Contact</a></p>"; // when I had this script in the same .cshtml with the View, I used Razor (<text>>razor code</text>) @ (+ `@Html.ActionLink("More Info", "Details", "FoundObjects", new { id = @marker.Id }, null)` + "<p>" + `@Html.ActionLink("Contact", "ContactUserFoundObject", "UsersContact", new { id = @marker.Id, title = @marker.Title, userName = @marker.UserNameReport, securityQuestion = @marker.SecurityQuestion  }, null)`;  )
         
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
            icon:'http://maps.google.com/mapfiles/ms/icons/green-dot.png',
            map: map
        });
        google.maps.event.addListener(marker, 'click', (function(marker, i) //Closures
        {
            return function()
            {
                infoWindow.setContent(info[i]);
                infoWindow.open(map, marker);
            }
        })(marker, i)); //Closures
    }
}