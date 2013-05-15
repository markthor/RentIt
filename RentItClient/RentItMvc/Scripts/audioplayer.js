function initPlayer(ogg, mp3) {
    //Prepent http:// to the link and set the ogg source
    var _ogg = document.getElementById("ogg");
    _ogg.src = "http://" + ogg;

    //Prepent http:// to the link and set the mp3 source
    var _mp3 = document.getElementById("mp3");
    _mp3.src = "http://" + mp3;
}

function playAudio() {
    //Retrieve the button and player
    var player = document.getElementById("player");
    var playstopButton = document.getElementById("playStop");
    
    //Retrieve icons
    var playIcon = document.getElementById("playIcon");
    var stopIcon = document.getElementById("stopIcon");
    
    //Check which icon is present on the player

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

function openPlayer(uri) {
    window.open(uri, 'playerWindow', 'width = 200, height = 400, left = 100, right = 100');
}