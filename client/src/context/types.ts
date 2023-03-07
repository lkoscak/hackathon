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

export type IGlobalContextType = {
    reports: Report[],
    teamCardStatsWindowState: ITeamCardStats,
}

export type GlobalAction = { type: 'TEST', payload: {}; } | { type: 'MOVE_COMPONENT', payload: { component: ITeamCard, fromParent: string, toParent: string, reportCount: number }; };

export interface IGlobalContextValue {
    teamCardStatsWindowState: ITeamCardStats,
    reports: Report[];
    dispatch: Dispatch<GlobalAction>;
}

