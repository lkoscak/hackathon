type Report = {
    id:string,
    title:string,
    description:string,
    created:string,
    creator:string,
    images:string[],
    status:number,
    team:number,
    lat:number,
    lng:number
}


export interface IGlobalContextType {
    reports:Report[]
}

