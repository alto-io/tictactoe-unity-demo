import { Schema, Context, type } from "@colyseus/schema";

export class MyRoomState extends Schema {

  // just a test property
  @type("string") mySynchronizedProperty: string = "Hello world";

  // real properties
  @type("string") playerSide: string = "";

  @type("string") computerSide: string = "";

  @type("number") numOfUsedButtons: number = 0;

  @type("boolean") playerMove: boolean = false;

  @type("number") delay:  number= 0;

  @type("number") botChosenPos:  number= 0;

}
