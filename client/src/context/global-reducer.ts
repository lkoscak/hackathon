import {
	TEST
} from "./actions";

const reducer = (state: any, action: {type: string, payload: any}) => {
	if (action.type === TEST) {
		return {
			...state,
		};
	}
	throw new Error(`No such action: ${action.type}`);
};

export default reducer;