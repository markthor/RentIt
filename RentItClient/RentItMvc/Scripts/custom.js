var controls;
var player;
var playstopButton;
var volume = 0.8; //0 to 1
var playIcon;

var _ogg = "http://ia600302.us.archive.org/17/items/1920sPop/BestThingsInLifeAreFree.ogg";
var _mp3 = "http://ia600302.us.archive.org/17/items/1920sPop/BestThingsInLifeAreFree_64kb.mp3";

function initPlayerControls() {
    controls = document.getElementById("controls");
    player = document.getElementById("player");
    if (player.canPlayType) { //This browser supports HTML5 audio
        
        player.volume = volume;
    }
}

function playAudio() {
    //Set the volume
    player.volume = volume;
    
    //Retrieve the button and player
    player = document.getElementById("player");
    playstopButton = document.getElementById("playStop");

    //Retrieve icons
    var playIcon = document.getElementById("playIcon");
    var stopIcon = document.getElementById("stopIcon");
    
    //Check which icon is present on the player

    if (playIcon != null) { //Player is not playing
        //Remove all previous sources from the player
        player.removeChild(document.getElementById("ogg"));
        player.removeChild(document.getElementById("mp3"));

        //Add the sources
        var ogg = document.createElement("source");
        ogg.setAttribute("id", "ogg");
        ogg.setAttribute("src", _ogg);
        ogg.setAttribute("type", "audio/ogg");
        player.appendChild(ogg);
        
        var mp3 = document.createElement("source");
        mp3.setAttribute("id", "mp3");
        mp3.setAttribute("src", _mp3);
        mp3.setAttribute("type", "audio/mp3");
        player.appendChild(mp3);

        //Start playing
        player.play();

        //Create the stopIcon and append it to the button
        stopIcon = document.createElement("i");
        stopIcon.setAttribute("id", "stopIcon");
        stopIcon.setAttribute("class", "icon-stop");
        playstopButton.appendChild(stopIcon);

        //Remove the playIcon
        playstopButton.removeChild(playIcon);
        
    } else { //Player is playing
        //Stop playing
        player.pause();
        
        //Create the playIcon and append it to the button
        playIcon = document.createElement("i");
        playIcon.setAttribute("id", "playIcon");
        playIcon.setAttribute("class", "icon-play");
        playstopButton.appendChild(playIcon);
        
        //Remove the stopIcon
        playstopButton.removeChild(stopIcon);
    }
}

function updateSlider(newValue) {
    //Retrieve the player
    player = document.getElementById("player");
    
    //Set the volume
    player.volume = newValue;
}