import { useReducer } from "react";

import GlobalContext from './global-context'
import reducer from "./global-reducer";

const GlobalProvider: React.FC<any>  = (props) => {
	const [globalState, dispatchGlobalAction] = useReducer(
		reducer,
		{}
	);

	const globalContext = {
	};

	return (
		<GlobalContext.Provider value={globalContext}>
			{props.children}
		</GlobalContext.Provider>
	);
};

export default GlobalProvider;