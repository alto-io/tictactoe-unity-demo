export interface IResponse<T> {
    status: number;
    msg?: string;
    data?: T;
    err?: any[];
}

//STUB API implemented in OPArcade production
export class TournamentApi {
    private baseUrl:string;

    constructor() {
        
    }

    postScore(playerId:string, tournamentId:string, token:string, score:number):boolean{
        return true;
    }

}