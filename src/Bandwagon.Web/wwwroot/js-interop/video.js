
let player = null;

export function createTestVideoPlayer(elementId) {
    // https://www.twitch.tv/videos/1737346428
    const options = {
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight,
        video: "1737346428",
        parent: ["www.youtube.com"]
    };

    player = new Twitch.Player(elementId, options);
    player.setVolume(0.5);
}

export function getCurrentTime() {
    return player.getCurrentTime();
}