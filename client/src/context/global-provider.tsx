import { useEffect, useReducer, useState } from "react";

import GlobalContext from './global-context'
import reducer from "./global-reducer";
import { IGlobalContextType } from "../context/types";

import { reports } from "../mocks/reports";
import { teamCards } from "../mocks/teamCards";
import { Connect, subscriptionsToActionsMap } from "../utils/mqtt";

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
	reportCount:0,
	dispatch: () => { }
}





const GlobalProvider: React.FC<any> = (props) => {


	const [mqttClient, setMqttClient] = useState<any>(null)

	const onMessageArrived = (topic: string, payload: any)=>{
		console.log('Message arrived in provider')
		dispatchGlobalAction({
			type: subscriptionsToActionsMap.get(topic),
			payload: payload
		})
	}

	useEffect(() => {
		setMqttClient((currentMqttClient: any) =>{
			if(currentMqttClient !== null){
				currentMqttClient.disconnect()
			}
			return Connect(onMessageArrived)
		})
	}, [])
	

	const [globalState, dispatchGlobalAction] : [IGlobalContextType, (action: any) => void] = useReducer(
		reducer,
		initGlobalContext
	);

	const contextValue: IGlobalContextType = {
		...globalState,
		dispatch:dispatchGlobalAction
	}

	return (
		<GlobalContext.Provider value={contextValue}>
			{props.children}
		</GlobalContext.Provider>
	);
};

export default GlobalProvider;