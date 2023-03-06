import car from '../assets/markers/car/Car_3.png'
import trash from '../assets/markers/trash/Trash_5.png'

import { Report } from '../context/types';

export const markerIconsMap = new Map<number,string>(
    [
        [1, car],
        [2, trash],
    ]
);

export const getTopNReportsForDisplay = (reports: Report[], n: number) => {
    return reports.sort((a, b) => {
        return new Date(b.created).getTime() - new Date(a.created).getTime();
    }).slice(0, n);
}

export const generateUniqueId = ()=> Math.random().toString().replace(".", "M");