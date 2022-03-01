// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.32
// 

using Colyseus.Schema;

public partial class TictactoeRoomState : Schema {
	[Type(0, "string")]
	public string mySynchronizedProperty = default(string);

	[Type(1, "string")]
	public string playerSide = default(string);

	[Type(2, "string")]
	public string computerSide = default(string);

	[Type(3, "number")]
	public float numOfUsedButtons = default(float);

	[Type(4, "boolean")]
	public bool playerMove = default(bool);

	[Type(5, "number")]
	public float delay = default(float);

	[Type(6, "number")]
	public float botChosenPos = default(float);
}

