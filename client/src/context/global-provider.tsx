import { useCallback, useEffect, useReducer, useState } from "react";


import GlobalContext from './global-context'
import reducer from "./global-reducer";
import { IGlobalContextType } from "../context/types";

import { reports } from "../mocks/reports";
import { teamCards } from "../mocks/teamCards";
import { Connect, subscriptionsToActionsMap } from "../utils/mqtt";
import useHttp from "../hooks/use-http";
import { GetAllStatuses,GetAllGroups,GetAllTeams } from "../utils/api";

import { SET_INIT_DATA } from "./actions";

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
	dispatch: () => { },
	statuses:[],
	groups:[],
	teams:[]
}


const GlobalProvider: React.FC<any> = (props) => {

	const [sendRequest, isLoading, error] = useHttp();

	const [mqttClient, setMqttClient] = useState<any>(null)

	const handleData = useCallback((data:any) =>
	{ 
	 	dispatchGlobalAction({type:SET_INIT_DATA,payload:data}) 
		console.log(data);
	},[])

	const onMessageArrived = (topic: string, payload: any)=>{
		console.log('Message arrived in provider')
		dispatchGlobalAction({
			type: subscriptionsToActionsMap.get(topic),
			payload: payload
		})
	}

	useEffect(() => {		
		const fetchInitData = () =>{
				const fetchGroups = fetch(GetAllGroups, {
					method: "GET",
					headers:  {}
				}).then(response => response.json());
				const fetchTeams = fetch(GetAllTeams, {
					method: "GET",
					headers:  {}
				}).then(response => response.json());
				const fetchStatuses = fetch(GetAllStatuses, {
					method: "GET",
					headers:  {}
				}).then(response => response.json());

				Promise.all([fetchGroups,fetchTeams,fetchStatuses]).then((response:any)=>
				{
					const result = {
						groups:response[0],
						teams:response[1],
						statuses:response[2]
					}
					handleData(result);
				}).catch(error=>{ console.log(error)})
		}

		fetchInitData()

	},[])

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