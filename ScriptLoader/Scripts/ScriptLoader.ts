class cScriptLoader {
    private m_js_files: string[];
	private m_css_files: string[];
	private m_head:HTMLHeadElement;
	
	private log = (t:any) =>
	{
		console.log("ScriptLoader: " + t);
	}
	
	
    constructor(files: string[]) {
        this.m_js_files = [];
		this.m_css_files = [];
		this.m_head = document.getElementsByTagName("head")[0];
		// this.m_head = document.head; // IE9+ only
		
		
		function endsWith(str:string, suffix:string):boolean 
		{
			if(str === null || suffix === null)
				return false;
				
			return str.indexOf(suffix, str.length - suffix.length) !== -1;
		}
		
		
		for(var i:number = 0; i < files.length; ++i) 
		{
			if(endsWith(files[i], ".css"))
			{
				this.m_css_files.push(files[i]);
			}
			else if(endsWith(files[i], ".js"))
			{
				this.m_js_files.push(files[i]);
			}
			else
				this.log('Error unknown filetype "' + files[i] +'".');
		}
		
    }
	
	
	public withNoCache = (filename:string):string =>
	{
		if(filename.indexOf("?") === -1)
			filename += "?no_cache=" + new Date().getTime();
		else
			filename += "&no_cache=" + new Date().getTime();
			
		return filename;	
	}
	

	public loadStyle = (filename:string) =>
	{
		// HTMLLinkElement
		var link = document.createElement("link");
		link.rel = "stylesheet";
		link.type = "text/css";
		link.href = this.withNoCache(filename);
		
		this.log('Loading style ' + filename);
        link.onload = () =>
        {
            this.log('Loaded style "' + filename + '".');
			
        };
		
		link.onerror = () =>
        {
			this.log('Error loading style "' + filename + '".');
        };
		
		this.m_head.appendChild(link);
	}
	
	
	public loadScript = (i:number) => 
	{
        var script = document.createElement('script');
        script.type = 'text/javascript';
		script.src = this.withNoCache(this.m_js_files[i]);
		
		var loadNextScript = () => 
		{
			if (i + 1 < this.m_js_files.length)
			{
				this.loadScript(i + 1);
			}
		}
		
        script.onload = () =>
        {
            this.log('Loaded script "' + this.m_js_files[i] + '".');
			loadNextScript();
        };
		
		
		script.onerror = () =>
        {
			this.log('Error loading script "' + this.m_js_files[i] + '".');
			loadNextScript();
        };
		
		
		this.log('Loading script "' + this.m_js_files[i] + '".');
		this.m_head.appendChild(script);
    }
	
	public loadFiles = () => 
	{
		// this.log(this.m_css_files);
		// this.log(this.m_js_files);
		
		for(var i:number = 0; i < this.m_css_files.length; ++i)
			this.loadStyle(this.m_css_files[i])
		
        this.loadScript(0);
    }
	
}


var ScriptLoader = new cScriptLoader(["foo.css", "Scripts/Script4.js", "foobar.css", "Scripts/Script1.js", "Scripts/Script2.js", "Scripts/Script3.js"]);
ScriptLoader.loadFiles();
