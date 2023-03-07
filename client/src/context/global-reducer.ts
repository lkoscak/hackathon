
import { IGlobalContextType, GlobalAction, ITeamCard } from "./types";


const reducer: (state: any, action: GlobalAction) => IGlobalContextType = (state, action) => {
	switch (action.type) {
		case 'TEST':
			return {
				reports: [...state.reports],
				teamCardStatsWindowState: { ...state.teamCardStatsWindowState }
			};
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
		default: throw new Error(`No such action: ${action}`);
	}

};

export default reducer;