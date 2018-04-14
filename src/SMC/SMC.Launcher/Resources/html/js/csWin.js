//
// Super Minecraft Launcher Source
//
// Copyright (c) 2018 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

function showOptionsWindow() {
    window.$("#settingsModal").modal();
}

// show quit questiom message
function showQuitQuestionMessage() {
    window.$("#quitAccept").modal();
}

// show shutdown message
function showShutdownMessage() {
    disposeAllModals();
    window.$("#shutingdown").modal({ keyboard: false, backdrop: "static" });
}

function disposeAllModals() {
    window.$("#processingModal").modal("hide");
    window.$("#fatalProblemModal").modal("hide");
    window.$("#shutingdown").modal("hide");
    window.$("#quitAccept").modal("hide");

    //window.$(".modal").modal("hide");
    fixModalBackdrop();
}

function fixModalBackdrop() {
    window.$(".modal-backdrop").remove();
}