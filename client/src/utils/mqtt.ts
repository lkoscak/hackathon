import { System } from "typescript"
import { generateUniqueId } from "./utils"
import { NEW_REPORT, UPDATE_REPORT } from "../context/actions"

const mqttHost = 'new-fleet.mobilisis.hr'
const mqttPort = 443
const clientName = 'hackathon'
const mqttConnectOptions = {
    userName: 'Slottdcemqtt',
    password: 'MojJ4k3Passw0rd',
    cleanSession: false,
    useSSL: true,
    onSuccess: ()=>{},
    onFailure:  ()=>{},
}

export const subscriptions = [
    {
        topic: 'hackathon/report/new',
        options: {
            qos: 1
        }
    },
    {
        topic: 'hackathon/report/update',
        options: {
            qos: 1
        }
    }
]

export const subscriptionsToActionsMap = new Map<string,string>(
    [
        [subscriptions[0].topic, NEW_REPORT],
        [subscriptions[1].topic, UPDATE_REPORT],
    ]
);

export const Connect = (onMessageArrived: (topic: string, payload: any) => void) => {

    let reconnectInterval: ReturnType<typeof setInterval> | null = null 
    let forcingDisconnect = false

    let client: any = null

    function retryConnect(){
        if(forcingDisconnect) return;
        if(!reconnectInterval){
            reconnectInterval = setInterval(tryToConnectToServer, 10000)
        }
    }

    const onConnect = ()=>{
        console.log('Connected to mqtt server')
        if (reconnectInterval) {
            clearInterval(reconnectInterval);
            reconnectInterval = null;
        }
        subscriptions.forEach(subscription => subscribe(subscription.topic, subscription.options))
    }

    const onFailedConnect = ()=>{
        console.log('Connecting to mqtt server failed')
        retryConnect()
    }

    const subscribe = (topic: string, options: {qos: number})=>{
        console.log('Subscribing to topics')
        client.subscribe(topic, options)
    }

    const disconnect = () =>{
        console.log('Disconnecting from mqtt server')
        forcingDisconnect = true
        client.disconnect()
    }

    const tryToConnectToServer = ()=>{
        console.log('Trying to connect to mqtt server')

        client = new window.Paho.Client(mqttHost, mqttPort, "/fleetws",  `${clientName}${generateUniqueId()}`);

        mqttConnectOptions.onSuccess = onConnect;
        mqttConnectOptions.onFailure = onFailedConnect;

        client.onMessageArrived = (message: any)=>{
            try {
                const messagePayload = JSON.parse(message.payloadString);
                console.log(messagePayload)
                onMessageArrived(message.destinationName, messagePayload);
            } catch (error) {
                console.log('Message parsing error!')
                return;
            }
        }
        client.onConnectionLost = ()=>{
            console.log('Connection to mqtt server lost')
            //retryConnect()
        }
        
        client.connect(mqttConnectOptions);
    }

    tryToConnectToServer()

    return {
        disconnect: disconnect
    };
}