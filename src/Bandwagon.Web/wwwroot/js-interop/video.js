
let player = null;

export function createTestVideoPlayer(elementId, dotNetListener) {
    // https://www.twitch.tv/videos/1737346428
    const options = {
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight,
        video: "1737346428",
        parent: ["www.youtube.com"]
    };

    player = new Twitch.Player(elementId, options);
    player.setVolume(0.5);
    player.addEventListener(Twitch.Player.SEEK, e => {
        console.log(Twitch.Player.SEEK, e);
    });
    player.addEventListener(Twitch.Player.PLAY, e => {
        console.log(Twitch.Player.PLAY, e);
    });
    player.addEventListener(Twitch.Player.PAUSE, e => {
        console.log(Twitch.Player.PAUSE, e);
    });
}

export function getCurrentTime() {
    return player.getCurrentTime();
}