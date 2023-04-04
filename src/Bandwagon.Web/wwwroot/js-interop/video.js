
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
    if (dotNetListener) {
        player.addEventListener(Twitch.Player.SEEK, e => {
            dotNetListener.invokeMethodAsync("SetPosition", e.position);
        });
        player.addEventListener(Twitch.Player.PLAY, e => {
            dotNetListener.invokeMethodAsync("Play", e.position);
        });
        player.addEventListener(Twitch.Player.PAUSE, e => {
            dotNetListener.invokeMethodAsync("Pause", e.position);
        });
    }
}

export function getCurrentTime() {
    return player.getCurrentTime();
}