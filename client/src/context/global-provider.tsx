import { useEffect, useReducer,useState } from "react";

import GlobalContext from './global-context'
import reducer from "./global-reducer";
import {IGlobalContextType} from "../context/types";

import {reports} from "../mocks/reports";

import { Connect, subscriptions } from "../utils/mqtt";

const initGlobalContext : IGlobalContextType = {
	reports : reports
}

const GlobalProvider: React.FC<any>  = (props) => {

	const [mqttClient, setMqttClient] = useState<any>(null)

	const onMessageArrived = (topic: string, payload: any)=>{
		console.log('Message arrived in provider')
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