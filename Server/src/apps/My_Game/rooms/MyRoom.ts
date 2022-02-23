import { Room, Client } from "colyseus";
import { MyRoomState } from "./schema/MyRoomState";
import { TournamentApi } from "../../../library/TournamentApi";

export class MyRoom extends Room<MyRoomState> {
  private api: TournamentApi; //This is REQUIRED
  maxClients = 1; //This limits the Colyseus room to one participant
  
  //room options -- these are parameters required to be sent from your game to Colyseus via joinorcreate()
  playerId: string;
  tourneyId: string;
  token: string;
  finalScore:number;

  //call this function at the end of the game round to send the score to OPArcade servers for posting
  submitScore() {
    this.api
      .postScore(this.playerId, this.tourneyId, this.token, this.finalScore);
  }

  onCreate (options: any) {
    console.log("On Create");
    this.api = new TournamentApi(); //This initializes the OPArcade tourney functions
    this.setState(new MyRoomState());
    this.registerMessages();
  }

  private registerMessages(){
    this.onMessage("type", (client, message) => {
      console.log("State", message);
    });

    // this will be called on the game's start function
    this.onMessage("start", (client, message) => {
      // this.state.assign({playerMove: true, numOfUsedButtons: 0});
      console.log("Game start");
    })

    // this will be called after the game ends, win, loose or draw with the score
    this.onMessage("end", (client, message) => {
      console.log("Game end: score -", message);
    })

    // this will be called to move the enemy bot
    this.onMessage("moveBot", (client, message) => {
      this.moveBot(message);
    })

    this.onMessage("increaseUsedBtns", (client, message) => {
      let numOfUsedButtons = this.state.numOfUsedButtons;
      this.state.assign({numOfUsedButtons: numOfUsedButtons + 1});
    })

    this.onMessage("setPlayerMove", (client, message) => {
      let newPlayerMove = this.state.playerMove ? false : true;
      this.state.assign({playerMove: newPlayerMove});
    })

    this.onMessage("setSides", (client, message) => {

      console.log(message);
      if (message === "X") {
        console.log("x side");
        this.state.assign({playerSide: "X", computerSide: "O"});
      } else {
        console.log("o side");
        this.state.assign({playerSide: "O", computerSide: "X"});
      }
    })

    this.onMessage("restartGame", (client, message) => {
      this.state.assign({
        numOfUsedButtons: 0,
        playerMove: true
      })
    })
  }

  //This is a stub required for OPArcade integration
  onAuth(
    client: Client,
    options: Record<string, string>
  ): boolean {
    console.log("On Auth",options);
    this.playerId = options.playerid;
    this.tourneyId = options.tourneyid;
    this.token = options.otp;

    return true;
  }

  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

  // custom methods

  // move bot
  moveBot(msg: any) {
    this.state.assign({ botChosenPos: msg});
  }
}
