using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class NetworkCommandMessage : MessageBase
{
	public static short Type { get { return MyMessageTypes.Command; } }
	private static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };

	public Command Command { get; private set; }

	public NetworkCommandMessage() { } //for receiving the message
	public NetworkCommandMessage(Command command)
	{
		Command = command;
	}

	public override string ToString()
	{
		return "[NetworkCommandMessage: " + Command + "]";
	}

	// Serialize the contents of this message into the writer
	public override void Serialize(NetworkWriter writer)
	{
		string text = JsonConvert.SerializeObject(Command, SerializerSettings);
		
		writer.Write(text);
	}

	// De-serialize the contents of the reader into this message
	public override void Deserialize(NetworkReader reader)
	{
		string text = reader.ReadString();
		
		Command = JsonConvert.DeserializeObject<Command>(text, SerializerSettings);
	}
}
