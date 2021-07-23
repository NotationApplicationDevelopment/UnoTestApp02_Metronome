var root = ""
var musics = {}
function playSound(text, volume) {

    if (text in musics) {
        var music = musics[text];
        music.currentTime = 0;
        music.volume = volume * 0.01;
        music.play();
    }
}

function loadSound(text) {
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
        return true;
    }
    else {
        var music = new Audio(root + text);
        musics[text] = music;
        return music.readyState != HAVE_NOTHING;
    }
}