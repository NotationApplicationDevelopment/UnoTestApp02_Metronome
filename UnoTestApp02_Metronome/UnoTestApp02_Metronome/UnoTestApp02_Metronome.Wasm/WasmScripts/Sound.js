var root = ""
var musics = {}
function playSound(text) {
    //get folder of script
    if (root == "") {

        var scripts = document.getElementsByTagName("script");
        var i = scripts.length;
        while (i--) {
            var match = scripts[i].src.match(/(^|.*\/)Sound.js$/);
            if (match) {
                root = match[1];
                break;
            }
        }
    }

    if (text in musics) {
        var music = musics[text];
        music.currentTime = 0;
        music.play();
    }
    else {
        var music = new Audio(root + text);

        music.play();  // 再生
        musics[text] = music;
    }
}