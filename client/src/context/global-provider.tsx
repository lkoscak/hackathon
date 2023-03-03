import { useEffect, useReducer,useState } from "react";

import GlobalContext from './global-context'
import reducer from "./global-reducer";
import {IGlobalContextType} from "../context/types";

import {reports} from "../mocks/reports";

const initGlobalContext : IGlobalContextType = {
	reports : reports
}

const GlobalProvider: React.FC<any>  = (props) => {

	useEffect(() => {
	  const testInterval = setTimeout(()=>{
		console.log('new report')
		dispatchGlobalAction({type: 'TEST'})
	  }, 20000)
	  //return ()=>{clearInterval(testInterval)}
	}, [])
	

	const [globalState, dispatchGlobalAction] : [IGlobalContextType, (type: any) => void] = useReducer(
		reducer,
		initGlobalContext
	);

	return (
		<GlobalContext.Provider value={globalState}>
			{props.children}
		</GlobalContext.Provider>
	);
};

export default GlobalProvider;