using System.Linq;
using DiscordRPC;

namespace DiscordRichPresencePresets.Shared
{
	public class PresenceApiWorker
	{
		private DiscordRpcClient RpcClient;

		public PresenceApiWorker(string clientId)
		{
			RpcClient = new(clientId);
			RpcClient.Initialize();
			RpcClient.Invoke();
		}

		public void SetRichPresence(Presence presence)
		{
			RpcClient.SetPresence(new RichPresence
			{
				Assets = new Assets
				{
					LargeImageKey  = presence.BigImage,
					LargeImageText = presence.BigImageText,
					SmallImageKey  = presence.SmallImage,
					SmallImageText = presence.SmallImageText
				},
				Details    = presence.Data1,
				Party      = null,
				Secrets    = null,
				State      = presence.Data2,
				Timestamps = null,
				Buttons = presence.Buttons
				                  .Where(b => !string.IsNullOrWhiteSpace(b.Text) && !string.IsNullOrWhiteSpace(b.Url))
				                  .Select(b => new Button {Label = b.Text, Url = b.Url}).ToArray()
			});

			RpcClient.Invoke();
		}

		public void RemoveRichPresence()
		{
			RpcClient.SetPresence(null);
			RpcClient.Invoke();
		}

		public void Reset(string clientId)
		{
			RpcClient.Dispose();
			RpcClient = new DiscordRpcClient(clientId);
		}
	}
}