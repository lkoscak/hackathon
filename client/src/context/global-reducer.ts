import {
	NEW_REPORT,
	UPDATE_REPORT,
	MOVE_COMPONENT
} from "./actions";

import { IGlobalContextType, GlobalAction, ITeamCard,Report } from "./types";


const reducer : (state: any, action: {type: string, payload: any}) => any = (state, action) => {
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
				...state,
				teamCardStatsWindowState: { ...state.teamCardStatsWindowState },
				reportCount: repCount
			}

		case "UPDATE_REPORT" :
			const newState = [...state.reports]
			newState.forEach((report:Report)  => {
				report.additonallInfo.activeCount = 0
				if(report.id === action.payload.id ){
					Object.assign(report, action.payload)
				}
			});
			return {
				...state,
				reports: newState,
			};
		case "NEW_REPORT" : {
			const newState = [...state.reports]
			newState.forEach((report:Report) => {
				report.additonallInfo.activeCount = 0
			});
			action.payload.additonallInfo = {activeCount: 0}
			newState.unshift(action.payload)
			return {
				...state,
				reports: newState,
			};
		}
		default: throw new Error(`No such action: ${action}`);
};
}

export default reducer;