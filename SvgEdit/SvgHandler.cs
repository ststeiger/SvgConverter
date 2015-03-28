
using System;
using System.Web;


namespace SvgEdit
{


	// http://stackoverflow.com/questions/23785317/mime-types-for-xsp-mono-webserver
	public class SvgHandler : IHttpHandler
	{


		public SvgHandler()
		{ }


		public void ProcessRequest(HttpContext context)
		{
			context.Response.ClearContent ();
			context.Response.ClearHeaders ();
			string absolutePath = context.Server.MapPath ("~" + context.Request.Url.AbsolutePath);
			System.IO.FileInfo fi = new System.IO.FileInfo (absolutePath);

			if (!fi.Exists)
			{
				/*
				throw new HttpException(404
					,string.Format("File \"{0}\" doesn't exist...", absolutePath)
				);
				*/
				context.Response.Clear ();
				context.Response.ClearHeaders ();

				context.Response.ContentType = "text/html";
				context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
				// context.Response.Status = "foo";
				context.Response.StatusDescription = string.Format ("File \"{0}\" doesn't exist...", context.Request.Url.AbsolutePath);
				context.Response.Flush ();
				context.Response.End ();
				return;
			}

			long length = fi.Length;
			context.Response.CacheControl = "Public";
			context.Response.AddHeader ("Content-Length", length.ToString ());
			context.Response.Headers.Add ("Cache-Control", "no-cache, no-store, max-age=0");
			context.Response.Headers.Add ("Pragma", "no-cache");
			context.Response.Headers.Add ("Pragma", "no-cache");
			context.Response.Cache.SetExpires (System.DateTime.UtcNow.AddYears (-1));
			context.Response.Cache.SetCacheability (HttpCacheability.NoCache);
			context.Response.ContentType = "image/svg+xml";
			context.Response.WriteFile (absolutePath);
			context.Response.Flush ();
			context.Response.Close ();
		} // End Sub ProcessRequest


		public bool IsReusable
		{
			get
			{
				return false;
			}
		} // End Property IsReusable


	} // End Class

		
} // End Namespace
