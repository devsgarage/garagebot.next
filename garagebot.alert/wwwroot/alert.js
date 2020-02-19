window.audioLib = {
    playAudio: function (element) {
        document.getElementById(element).play();
    },

    playAudioWithRef: function (element) {
        console.log("playing audio");
        element.play();
    }
};