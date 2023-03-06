import { useEffect, useReducer, useState } from "react";

import GlobalContext from './global-context'
import reducer from "./global-reducer";
import { IGlobalContextType, IGlobalContextValue } from "../context/types";

import { reports } from "../mocks/reports";
import { teamCards } from "../mocks/teamCards";

const initGlobalContext: IGlobalContextType = {
	reports: reports,
	teamCardStatsWindowState: {
		teamCardStatsWindow1: {
			components: teamCards
		},
		teamCardStatsWindow2: {
			components: []
		}
	},
}

const GlobalProvider: React.FC<any> = (props) => {

	const [globalState, dispatch] = useReducer(
		reducer,
		initGlobalContext
	);

	const contextValue: IGlobalContextValue = {
		...globalState,
		dispatch,
	}

	return (
		<GlobalContext.Provider value={contextValue}>
			{props.children}
		</GlobalContext.Provider>
	);
};

export default GlobalProvider;