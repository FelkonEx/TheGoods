var allLinks = document.querySelectorAll("a");
var filter = Array.from(allLinks).filter((link) =>
    link.href.includes("magnet")
);
var returnString = "";
filter.forEach((item) => {
    returnString += item.href + "\n";
});

console.log(returnString);
