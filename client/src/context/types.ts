import { Dispatch } from 'react';


export type Report = {
    id: string,
    title: string,
    description: string,
    created: string,
    creator: string | null,
    images: string[],
    status: number,
    group: number,
    category: string,
    additionallInfo: {
        activeCount: number
    },
    team: number,
    lat: number,
    lng: number
}

export type ITeamCard = {
    id: number,
    teamName: string,
    reportCount: number,
    symbol: string,
    color: string
}

type TeamCardStatsWindow = {
    components: ITeamCard[]
}

export interface ITeamCardStats {
    teamCardStatsWindow1: TeamCardStatsWindow,
    teamCardStatsWindow2: TeamCardStatsWindow
}

type Status = {
    id:number,
    name:string,
    description:string
}

type Group = {
    id : number,
    name : string,
    description:string
}

type Team = {
    id:number,
    name:string,
    color:string,
    icon:string
}

export type IGlobalContextType = {
    reports: Report[],
    teamCardStatsWindowState: ITeamCardStats,
    dispatch: Dispatch<GlobalAction>,
    reportCount:number,
    statuses:Status[],
    groups:Group[],
    teams:Team[]
}

export type GlobalAction = { type: string, payload: object | null; } 

// export interface IGlobalContextValue {
//     teamCardStatsWindowState: ITeamCardStats,
//     reports: Report[];
//     dispatch: Dispatch<GlobalAction>;
// }

