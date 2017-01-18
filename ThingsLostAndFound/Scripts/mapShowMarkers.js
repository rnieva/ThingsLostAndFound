//
var map;
var marker;

function showMarkersFO(fol) {
    var bounds = new google.maps.LatLngBounds();
    var la = [];
    var lo = [];
    var info = [];
    var titles = [];
    var infohtml;
        var index;
        for (index = 0; index < fol.length; ++index)
        {
            la.push(fol[index].Latitude);
            lo.push(fol[index].Longitude);
            titles.push(fol[index].Title);
            if (fol[index].Img == true)
        { 
                infohtml = "<div><b>Found Object</b><h2>" + fol[index].Title + "</h2></div><p>" + fol[index].Category + "<p> " + fol[index].Address + "<p>" + fol[index].Location + "<p> <p><img src=~/File?id="+fol[index].FileId+" alt= height=42 width=42 /></p>" + Html.ActionLink("More Info", "Details", "FoundObjects", new { id = " + fol[index].Id + " }, null)` + "<p>" + `@Html.ActionLink("Contact", "ContactUserFoundObject", "UsersContact", new { id = @marker.Id, title = @marker.Title, userName = @marker.UserNameReport, securityQuestion = @marker.SecurityQuestion  }, null)`;
        }else
        { 
                infohtml = "<div><b>Found Object</b><h2>" + fol[index].Titl + "</h2></div><p> @marker.Category<p> @marker.Address<p>@marker.Location<p>" + `@Html.ActionLink("More Info", "Details", "FoundObjects", new { id = @marker.Id }, null)` + "<p>" + `@Html.ActionLink("Contact", "ContactUserFoundObject", "UsersContact", new { id = @marker.Id, title = @marker.Title, userName = @marker.UserNameReport, securityQuestion = @marker.SecurityQuestion  }, null)`;  
         
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
            icon:'http://maps.google.com/mapfiles/ms/icons/red-dot.png',
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