function playAudio() {
    //Retrieve the button and player
    var player = document.getElementById("player");
    var playstopButton = document.getElementById("playStop");
    
    //Retrieve icons
    var playIcon = document.getElementById("playIcon");
    var stopIcon = document.getElementById("stopIcon");
    
    if (playIcon != null) { //Player is not playing
        //Reload the player - effectivly rebuffering
        player.load();

        //Create the stopIcon and append it to the button
        stopIcon = document.createElement("i");
        stopIcon.setAttribute("id", "stopIcon");
        stopIcon.setAttribute("class", "icon-stop icon-white");
        playstopButton.appendChild(stopIcon);

        //Remove the playIcon
        playstopButton.removeChild(playIcon);
        
        //Start playing
        player.play();
        
    } else { //Player is playing
        //Stop playing
        player.pause();
        
        //Create the playIcon and append it to the button
        playIcon = document.createElement("i");
        playIcon.setAttribute("id", "playIcon");
        playIcon.setAttribute("class", "icon-play icon-white");
        playstopButton.appendChild(playIcon);
        
        //Remove the stopIcon
        playstopButton.removeChild(stopIcon);
    }
}

function updateSlider(newValue) {
    //Retrieve the player
    var player = document.getElementById("player");
    
    //Set the volume
    player.volume = newValue;
}

function openPlayer(channelIdAndUserId) {
    var uri = "http://rentit.itu.dk/BlobfishRadio/Audio/AudioPlayer?channelId=" + channelIdAndUserId;
    //var uri = "http://localhost:49932/Audio/AudioPlayer?channelId=" + channelIdAndUserId;
    var width = 365;
    var height = 500;
    var left = (screen.width / 2) - (width / 2);
    var top = (screen.height / 2) - (height / 2);
    //window.open(uri, 'playerWindow', 'width = 365, height = 500, left = 100, right = 100');
    window.open(uri, 'playerWindow', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);
}

//Upvote button is clicked
function upvote(trackId) {
    var userId = getUserId();
    var voteValue = getVoteValue(userId, trackId);

    if (voteValue == 1) {
        deleteVote(userId, trackId);
        flipUpvote(trackId);
    } else if (voteValue == -1) {
        deleteVote(userId, trackId);
        flipUpvoteAndDownvote(trackId);
        createUpvote(userId, trackId);
    } else {
        flipUpvote(trackId);
        createUpvote(userId, trackId);
    }
}

//Downvote button is clicked
function downvote(trackId) {
    var userId = getUserId();
    var voteValue = getVoteValue(userId, trackId);

    if (voteValue == 1) {
        deleteVote(userId, trackId);
        flipUpvoteAndDownvote(trackId);
        createDownvote(userId, trackId);
    } else if (voteValue == -1) {
        deleteVote(userId, trackId);
        flipDownvote(trackId);
    } else {
        flipDownvote(trackId);
        createDownvote(userId, trackId);
    }
}

//Create an upvote
function createUpvote(userId, trackId) {
    var createUpvoteUri = "http://rentit.itu.dk/BlobfishRadio/Account/CreateUpvote?userId=" + userId + "&trackId=" + trackId;
    sendRequest(createUpvoteUri);
}

//Create a downvote
function createDownvote(userId, trackId) {
    var createDownvoteUri = "http://rentit.itu.dk/BlobfishRadio/Account/CreateDownvote?userId=" + userId + "&trackId=" + trackId;
    sendRequest(createDownvoteUri);
}

//Flip upvote button
function flipUpvote(trackId) {
    var button = document.getElementById(trackId + "-up");
    if (button.className == "btn btn-small btn-success") {
        button.className = "btn btn-small";
    } else {
        button.className = "btn btn-small btn-success";
    }
}

//Flip downvote button
function flipDownvote(trackId) {
    var button = document.getElementById(trackId + "-down");
    if (button.className == "btn btn-small btn-danger") {
        button.className = "btn btn-small";
    } else {
        button.className = "btn btn-small btn-danger";
    }
}

//Flip both buttons
function flipUpvoteAndDownvote(trackId) {
    flipUpvote(trackId);
    flipDownvote(trackId);
}

//Delete a vote
function deleteVote(userId, trackId) {
    var uri = "http://rentit.itu.dk/BlobfishRadio/Account/DeleteVote?userId=" + userId + "&trackId=" + trackId;
    sendRequest(uri);
}

//Get the vote value
function getVoteValue(userId, trackId) {
    var getVotesUri = "http://rentit.itu.dk/BlobfishRadio/Account/GetVotes?userId=" + userId + "&trackId=" + trackId;
    return sendRequest(getVotesUri);
}

//Get the user id
function getUserId() {
    var currentUrl = window.location.href;
    var index = currentUrl.lastIndexOf("=") + 1;
    return currentUrl.substring(index, currentUrl.length);
}

//Get the channel id
function getChannelId() {
    var currentUrl = window.location.href;
    var index = currentUrl.lastIndexOf("channelId") + 10;
    var endIndex = currentUrl.lastIndexOf("&");
    return currentUrl.substring(index, endIndex);
}

//Send a request to the server
function sendRequest(uri) {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("GET", uri, false);
    xmlhttp.send();
    return xmlhttp.responseText;
}

//////////////////UPDATE THE TRACKS///////////////////////////////

//Set the window to call the function every 5th second
setInterval(updateLatestTracks, 10000);

//Update the tracklist
function updateTracklist() {
    var userId = getUserId();
    var trackTable = document.getElementById("trackTable");
    var parent = trackTable.parentNode;
    parent.removeChild(trackTable);

    var tr;
    var th;
    var td;
    var div;
    var button;
    var icon;

    trackTable = document.createElement("table");
    trackTable.setAttribute("class", "table");
    trackTable.setAttribute("id", "trackTable");
    parent.appendChild(trackTable);
    
    tr = document.createElement("tr");
    trackTable.appendChild(tr);
    
    th = document.createElement("th");
    th.textContent = "Artist";
    tr.appendChild(th);
    
    th = document.createElement("th");
    th.textContent = "Title";
    tr.appendChild(th);
    
    th = document.createElement("th");
    th.textContent = "";
    tr.appendChild(th);
    
    th = document.createElement("th");
    th.textContent = "";
    tr.appendChild(th);
    
    //Get the new tracks
    var json = getNewestTracks();
    var data = JSON.parse(json);
    for (var i = 0; i < data.length; i++) {
        var track = data[i];
        
        tr = document.createElement("tr");
        trackTable.appendChild(tr);
        
        //Create artist
        td = document.createElement("td");
        tr.appendChild(td);
        div = document.createElement("div");
        div.setAttribute("id", track.Id + "-artist");
        div.setAttribute("style", "width: 120px");
        div.setAttribute("class", "text-overflow");
        div.setAttribute("title", track.ArtistName);
        div.textContent = track.ArtistName;
        td.appendChild(div);
        
        //Create title
        td = document.createElement("td");
        tr.appendChild(td);
        div = document.createElement("div");
        div.setAttribute("id", track.Id + "-title");
        div.setAttribute("style", "width: 120px");
        div.setAttribute("class", "text-overflow");
        div.setAttribute("title", track.TrackName);
        div.textContent = track.TrackName;
        td.appendChild(div);
        
        //Create upvote button
        td = document.createElement("td");
        tr.appendChild(td);
        div = document.createElement("div");
        div.setAttribute("style", "width: 30px");
        td.appendChild(div);
        
        button = document.createElement("button");
        button.setAttribute("id", track.Id + "-up");
        button.setAttribute("onclick", "upvote(" + track.Id + ")");
        if (getVoteValue(userId, track.Id) == 1) {
            button.setAttribute("class", "btn btn-small btn-success");
        } else {
            button.setAttribute("class", "btn btn-small");
        }
        if (!isSubscribed()) {
            button.setAttribute("disabled", "disabled");
            button.setAttribute("title", "Subscribe to vote");
        }
        icon = document.createElement("i");
        icon.setAttribute("class", "icon-thumbs-up");
        button.appendChild(icon);
        div.appendChild(button);
        
        //Create downvote button
        td = document.createElement("td");
        tr.appendChild(td);
        div = document.createElement("div");
        div.setAttribute("style", "width: 30px");
        td.appendChild(div);

        button = document.createElement("button");
        button.setAttribute("id", track.Id + "-down");
        button.setAttribute("onclick", "downvote(" + track.Id + ")");
        if (getVoteValue(userId, track.Id) == -1) {
            button.setAttribute("class", "btn btn-small btn-danger");
        } else {
            button.setAttribute("class", "btn btn-small");
        }
        if (!isSubscribed()) {
            button.setAttribute("disabled", "disabled");
            button.setAttribute("title", "Subscribe to vote");
        }
        icon = document.createElement("i");
        icon.setAttribute("class", "icon-thumbs-down");
        button.appendChild(icon);
        div.appendChild(button);
    }
}

function getNewestTracks() {
    var userId = getUserId();
    var channelId = getChannelId();
    var uri = "http://rentit.itu.dk/BlobfishRadio/Audio/Last5TracksJson?channelId=" + channelId + "&userId=" + userId;
    var json = sendRequest(uri);
    return json;
}

function isValidUrl() {
    var currentUrl = window.location.href;
    return currentUrl.contains("/Audio/AudioPlayer?");
}

function updateLatestTracks() {
    if (isValidUrl) {
        updateTracklist();
        getNewestTracks();
    }
}

function isSubscribed() {
    var userId = getUserId();
    var channelId = getChannelId();
    var uri = "http://rentit.itu.dk/BlobfishRadio/Account/IsSubscribedJson?channelId=" + channelId + "&userId=" + userId;
    var json = sendRequest(uri);
    if (json == 1) {
        return true;
    } else {
        return false;
    }
}