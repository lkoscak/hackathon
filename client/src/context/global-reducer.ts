
import { IGlobalContextType, GlobalAction, ITeamCard } from "./types";


const reducer: (state: any, action: GlobalAction) => IGlobalContextType = (state, action) => {
	switch (action.type) {
		case 'TEST':
			return {
				reports: [...state.reports],
				teamCardStatsWindowState: { ...state.teamCardStatsWindowState }
			};
		case 'MOVE_COMPONENT':
			const { component, fromParent, toParent } = action.payload;
			const fromParentCopy = { ...state.teamCardStatsWindowState[fromParent] };
			const toParentCopy = { ...state.teamCardStatsWindowState[toParent] };
			const componentIndex = fromParentCopy.components.findIndex(
				(c: ITeamCard) => c.id === component.id
			);
			if (componentIndex !== -1) {
				fromParentCopy.components.splice(componentIndex, 1);
				toParentCopy.components.push(component);
			}

			return {
				reports: [...state.reports],
				teamCardStatsWindowState: { ...state.teamCardStatsWindowState },
				[fromParent]: fromParentCopy,
				[toParent]: toParentCopy,
			}
		default: throw new Error(`No such action: ${action}`);
	}

};

export default reducer;