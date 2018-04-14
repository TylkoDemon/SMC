//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

function updateProgress(currentText, currentValue, globalText, globalValue) {
    document.getElementById("currLabel").innerHTML = currentText;
    document.getElementById("globalLabel").innerHTML = globalText === "" ? "..." : globalText;

    var value = currentValue + "%";
    document.getElementById("currentProgressbar").style.width = value;
    document.getElementById("currentProgressbar").innerHTML = (value === "0%" ? "": value);

    value = globalValue + "%";
    document.getElementById("globalProgressbar").style.width = value;
    document.getElementById("globalProgressbar").innerHTML = (value === "0%" ? "" : value);
}

function setActiveButtons(buttonyPlay, buttonRepair) {
    var button = document.getElementById("playButton");
    if (buttonyPlay === "False") {
        button.disabled = true;
        button.innerHTML = "DOWNLOADING";
    }
    else {
        button.disabled = false;
        button.innerHTML = "PLAY";
    }

    button = document.getElementById("repairButton");
    if (buttonRepair === "False") {
        button.disabled = true;
    }
    else {
        button.disabled = false;
    }
}

function showPlayWindow() {
    window.$("#playModal").modal();
}

function setPlayRequest() {
    var userName = document.getElementById("playInputUserName").value;
    var ram = document.getElementById("playInputRAM").value;
    csMain.onPlayRequest(userName, ram);
}