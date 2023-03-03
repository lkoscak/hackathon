import {
	TEST
} from "./actions";

import { IGlobalContextType } from "./types";

const reducer : (state: IGlobalContextType, action: {type: string, payload: any}) => IGlobalContextType = (state, action) => {
	if (action.type === TEST) {
		state.reports.forEach(report => {
			report.additonallInfo.activeCount = 0
		});
		const newState = [...state.reports]
		newState.unshift({...state.reports[1], id: Math.random().toString(), lat: 46.29, lng: 16.31, additonallInfo: {activeCount: 0}, created: '2023-03-02 12:00:00',})
		return {
			reports: newState,
		};
	}
	throw new Error(`No such action: ${action.type}`);
};

export default reducer;