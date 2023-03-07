import {
	NEW_REPORT,
	UPDATE_REPORT
} from "./actions";

import { IGlobalContextType } from "./types";

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
	}
	throw new Error(`No such action: ${action.type}`);
};

export default reducer;