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
	},}

import {IGlobalContextType} from "../context/types";

import {reports} from "../mocks/reports";

import { Connect, subscriptionsToActionsMap } from "../utils/mqtt";

const initGlobalContext : IGlobalContextType = {
	reports : reports
}

const GlobalProvider: React.FC<any> = (props) => {

	const [globalState, dispatch] = useReducer(

	const [mqttClient, setMqttClient] = useState<any>(null)

	const onMessageArrived = (topic: string, payload: any)=>{
		console.log('Message arrived in provider')
		dispatchGlobalAction({
			type: subscriptionsToActionsMap.get(topic),
			payload: payload
		})
	}

	//useEffect(() => {
	//	const testInterval = setTimeout(()=>{
	//		console.log('new report')
	//		dispatchGlobalAction({type: 'TEST'})
	//	}, 20000)
	//}, [])

	useEffect(() => {
		setMqttClient((currentMqttClient: any) =>{
			if(currentMqttClient !== null){
				currentMqttClient.disconnect()
			}
			return Connect(onMessageArrived)
		})
	}, [])
	

	const [globalState, dispatchGlobalAction] : [IGlobalContextType, (type: any) => void] = useReducer(
        client/src/context/global-provider.tsx
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