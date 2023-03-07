<<<<<<< client/src/context/global-reducer.ts
=======
import {
	NEW_REPORT,
	UPDATE_REPORT
} from "./actions";
>>>>>>> client/src/context/global-reducer.ts

import { IGlobalContextType, GlobalAction, ITeamCard } from "./types";

<<<<<<< client/src/context/global-reducer.ts

const reducer: (state: any, action: GlobalAction) => IGlobalContextType = (state, action) => {
	switch (action.type) {
		case 'MOVE_COMPONENT':
			const { component, fromParent, toParent, reportCount } = action.payload;
			const fromParentCopy = { ...state.teamCardStatsWindowState[fromParent] };
			const toParentCopy = { ...state.teamCardStatsWindowState[toParent] };
			let repCount: number = 0;
			const componentIndex = fromParentCopy.components.findIndex(
				(c: ITeamCard) => c.id === component.id
			);
			if (componentIndex !== -1) {
				repCount = state.teamCardStatsWindowState["teamCardStatsWindow1"].components[componentIndex].reportCount;
				fromParentCopy.components.splice(componentIndex, 1);
				toParentCopy.components.push(component);
			}

			return {
				reports: [...state.reports],
				teamCardStatsWindowState: { ...state.teamCardStatsWindowState },
				[fromParent]: fromParentCopy,
				[toParent]: toParentCopy,
				[reportCount]: repCount
			}
		case "UPDATE_REPORT" :
			const newState = [...state.reports]
			state.reports.forEach(report => {
				report.additonallInfo.activeCount = 0
				if(report.id === action.payload.id ){
					Object.assign(report, action.payload)
				}
			});
			return {
				reports: newState,
			};
		case "NEW_REPORT" : {
			const newState = [...state.reports]
			state.reports.forEach(report => {
				report.additonallInfo.activeCount = 0
			});
			action.payload.additonallInfo = {activeCount: 0}
			newState.unshift(action.payload)
			return {
				reports: newState,
			};
		}
		default: throw new Error(`No such action: ${action}`);
=======
const reducer : (state: IGlobalContextType, action: {type: string, payload: any}) => IGlobalContextType = (state, action) => {
	if (action.type === NEW_REPORT) {
		const newState = [...state.reports]
		state.reports.forEach(report => {
			report.additonallInfo.activeCount = 0
		});
		action.payload.additonallInfo = {activeCount: 0}
		newState.unshift(action.payload)
		return {
			reports: newState,
		};
	}
	if (action.type === UPDATE_REPORT) {
		const newState = [...state.reports]
		state.reports.forEach(report => {
			report.additonallInfo.activeCount = 0
			if(report.id === action.payload.id ){
				Object.assign(report, action.payload)
			}
		});

		return {
			reports: newState,
		};
>>>>>>> client/src/context/global-reducer.ts
	}

};

export default reducer;