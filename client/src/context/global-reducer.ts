import {
	TEST
} from "./actions";

import { IGlobalContextType } from "./types";

const reducer : (state: any, action: {type: string, payload: any}) => IGlobalContextType = (state, action) => {
	if (action.type === TEST) {
		return {
			reports: [...state.reports],
		};
	}
	throw new Error(`No such action: ${action.type}`);
};

export default reducer;