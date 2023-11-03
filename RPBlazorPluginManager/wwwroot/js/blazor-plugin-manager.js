

window.includeStylesheet = function (cssPath) {
    // include css in head
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = cssPath;
    head.appendChild(link);

    console.log("stylesheet added: " + cssPath);
}

// include js
window.includeScript = function (jsPath) {
    var body = document.getElementsByTagName('body')[0];
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = jsPath;
    body.appendChild(script);

console.log("script added: " + jsPath);
}