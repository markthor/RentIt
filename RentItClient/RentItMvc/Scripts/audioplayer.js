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
    var width = 365;
    var height = 500;
    var left = (screen.width / 2) - (width / 2);
    var top = (screen.height / 2) - (height / 2);
    //window.open(uri, 'playerWindow', 'width = 365, height = 500, left = 100, right = 100');
    window.open(uri, 'playerWindow', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);
}

function upvote(trackId) {
    //Get the user id
    var currentUrl = window.location.href;
    var index = currentUrl.lastIndexOf("=") + 1;
    var userId = currentUrl.substring(index, currentUrl.length);
    alert(userId);
    
    var createUpvoteUri = "http://rentit.itu.dk/BlobfishRadio/Account/CreateUpvote?userId=" + userId + "&trackId=" + trackId;
    //var createUpvoteUri = "http://localhost:49932/Account/CreateUpvote?userId=" + userId + "&trackId=" + trackId;
    var deleteVoteUri = "http://rentit.itu.dk/BlobfishRadio/Account/DeleteVote?userId=" + userId + "&trackId=" + trackId;
    sendRequest(createUpvoteUri);
    
}

function sendRequest(uri) {
    var xml = new XMLHttpRequest().open("GET", uri, true);
    alert(xml);
}