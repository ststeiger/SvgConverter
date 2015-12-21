
function loadRequiredFiles(callback)
{
    var scripts = ['Scripts/Script2.js', 'Scripts/Script1.js'];
    var styles = ['Scripts/test1.css'];
    var filesloaded = 0;
    var filestoload = scripts.length + styles.length;

    function finishLoad()
    {
        if (callback === null)
            return;

        if (filesloaded === filestoload)
        {
            callback();
        }
    }


    for (var i = 0; i < scripts.length; i++)
    {
        console.log('Loading script ' + scripts[i]);
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = scripts[i];
        script.onload = function ()
        {
            console.log('Loaded script');
            console.log(this);
            filesloaded++;  // (This means increment, i.e. add one)
            finishLoad();
        };
        document.head.appendChild(script);
    }

    for (var i = 0; i < styles.length; i++)
    {
        console.log('Loading style ' + styles[i]);
        var style = document.createElement('link');
        style.rel = 'stylesheet';
        style.href = styles[i];
        style.type = 'text/css';
        style.onload = function ()
        {
            console.log('Loaded style');
            console.log(this);
            filesloaded++;
            finishLoad();
        };
        document.head.appendChild(style);
    }

}
