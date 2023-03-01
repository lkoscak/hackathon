import { useReducer,useState } from "react";

import GlobalContext from './global-context'
import reducer from "./global-reducer";
import {IGlobalContextType} from "../context/types";


const initGlobalContext : IGlobalContextType = {
	reports :[]
}

const GlobalProvider: React.FC<any>  = (props) => {

	const [globalState, dispatchGlobalAction] = useReducer(
		reducer,
		{}
	);

	return (
		<GlobalContext.Provider value={initGlobalContext}>
			{props.children}
		</GlobalContext.Provider>
	);
};

export default GlobalProvider;