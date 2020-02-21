window.audioLib = {
    playAudio: function (element) {
        document.getElementById(element).play();
    },

    playAudioWithRef: function (element) {
        console.log("playing audio");
        element.play();
    }
};

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/garageHub")
    .configureLogging(signalR.LogLevel.Debug)
    .build();

connection.start().then(function () {
    console.log("connected");
});

connection.on("alertreceived", (user, message) => {
    const ref = document.getElementById("alertAudio");
    audioLib.playAudioWithRef(ref);
});