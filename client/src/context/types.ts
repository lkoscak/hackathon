export type Report = {
    id:string,
    title:string,
    description:string,
    created:string,
    creator:string | null,
    images:string[],
    status:number,
    group: number,
    category: string,
    team:number,
    lat:number,
    lng:number,
    additonallInfo: {
        activeCount: number
    }
}


export type IGlobalContextType = {
    reports:Report[]
}

