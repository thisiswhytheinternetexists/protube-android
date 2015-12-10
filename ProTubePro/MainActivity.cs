using Android.App;
using Android.Widget;
using Android.OS;
using Android.Net;
using System.Collections.Generic;
using System.Linq;
using WebSocket4Net;

namespace ProTubePro
{
	[Activity (Label = "ProTube Pro", MainLauncher = true, Icon = "@mipmap/icon")]
	[IntentFilter (new[]{"android.intent.action.VIEW"}, Categories = new[] {"android.intent.category.BROWSABLE","android.intent.category.DEFAULT"},
		DataScheme="http",
		DataHost="protu.be")]
	public class MainActivity : Activity
	{
		int count = 1;
		JsonWebSocket websocket;
		string clientId;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			if (Intent.Data != null) {
				Uri data = Intent.Data;
				List<string> parameters = data.PathSegments.ToList();
				if (data.PathSegments.Count > 1)
					Toast.MakeText (this, "Sorry, unsupported URL!", ToastLength.Long);
				clientId = parameters[0]; // "status"
				ConnectToProtube(clientId);
			}

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};
		}

		public void ConnectToProtube(string clientId) {
			
			websocket = new JsonWebSocket("ws://protu.be:80/");
			websocket.Opened += new System.EventHandler(websocket_Opened);
			websocket.On<string> ("identify", DoStuff);
			websocket.Open ();

			//websocket.On<string> ("connect", Identify);

		}

		public void DoStuff (string content) {
			string s = content;
		}


		private void websocket_Opened(object sender, System.EventArgs e)
		{
			websocket.Send ("identify", string.Format(@"\{ userid: {0}, client: 'remote'\}", clientId));
		}
	}
}


