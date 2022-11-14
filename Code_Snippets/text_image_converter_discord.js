const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
const space = " ";

var string_lines = ["Soon", "Next Stream"];

function determineLetter(letter) {
    if (letters.indexOf(letter.toUpperCase()) > -1) {
        return `:${letter.toUpperCase()}_:`;
    } else {
        return ":M1:";
    }
}

function padding(count) {
    var str = "";
    for (let i = 0; i < count; i++) {
        str += ":M1:";
    }
    return str;
}

function main() {
    var max_length = 0;
    string_lines.forEach((element) => {
        if (element.length > max_length) {
            max_length = element.length;
        }
    });

    var finalStringAll = "";

    string_lines.forEach((line) => {
        var finalString = "";
        var lineLetters = line.split("");

        if (line.length != max_length) {
            var total_buffer = max_length - line.length;

            var left_buffer = Math.floor(total_buffer / 2);
            var right_buffer = Math.ceil(total_buffer / 2);
        }

        finalString += ":L1:";

        if (left_buffer > 0) {
            finalString += padding(left_buffer);
        }

        lineLetters.forEach((letter) => {
            finalString += determineLetter(letter);
        });

        if (right_buffer > 0) {
            finalString += padding(right_buffer);
        }

        finalString += ":R1:";

        finalStringAll += finalString + "\n";
    });

    console.log(finalStringAll);
}

main();
