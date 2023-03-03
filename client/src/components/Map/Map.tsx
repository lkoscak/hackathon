import { useLoadScript } from "@react-google-maps/api"

import { MapContainer } from "./MapStyle";

import InfoWindowContent from "./InfoWindowContent";

import { useState, useMemo, useCallback, useRef, useEffect } from "react";
import {
  GoogleMap,
  MarkerF,
  InfoWindowF,
  InfoWindowProps
} from "@react-google-maps/api";

import useGlobalContext from "../../hooks/use-global-context";

import { markerIconsMap, getTopTenReportsForDisplay } from "../../utils/utils";
import { Report } from "../../context/types";

type LatLngLiteral = google.maps.LatLngLiteral;
type MapOptions = google.maps.MapOptions;

const Map = () => {
    const [activeReport, setActiveReport] = useState<Report | null>(null);
    const [activeCounter, setActiveCounter] = useState<number>(1);
    const {reports} = useGlobalContext();

    const topTenReportsForDisplay = useMemo(()=>getTopTenReportsForDisplay(reports), [reports])

    console.log(topTenReportsForDisplay)

    const handleActiveReport = (report: Report) => {
        if (report.id === activeReport?.id) {
          return;
        }
        setActiveReport(report);
    };

    const onInfoWindowClosing = useCallback(() => {
        setActiveReport(null);
    }, []);

    useEffect(() => {
        console.log('new move interval')
        let focusMarkerInterval = setInterval(()=>{
            const nextActiveReport = fetchNextActiveReport()
            if(nextActiveReport){
                nextActiveReport.additonallInfo.activeCount+=1;
                const bounds = new google.maps.LatLngBounds();
                bounds.extend({lat: nextActiveReport.lat, lng: nextActiveReport.lng})
                mapRef.current?.panTo({lat: nextActiveReport.lat, lng: nextActiveReport.lng});
                handleActiveReport(nextActiveReport)
            }else{
                fitBoundsToReports();
                setActiveReport(null);
                setActiveCounter(current => current+1)
            }
        }, 3000)

        function fetchNextActiveReport(){
            return topTenReportsForDisplay.find(report=>{
                return report.additonallInfo.activeCount !== activeCounter
            })
        }

        return ()=>{
            clearInterval(focusMarkerInterval)
        }
        
    }, [activeCounter])

    useEffect(() => {
        setActiveCounter(1)
    }, [reports])

    const fitBoundsToReports = useCallback(() => {
        const bounds = new google.maps.LatLngBounds();
        topTenReportsForDisplay.forEach(report => {bounds.extend({lat: report.lat, lng: report.lng})})
        mapRef.current?.fitBounds(bounds);
    }, [activeCounter])

    const mapCenter = useMemo<LatLngLiteral>(() => ({
        lat: 46.305746,
        lng: 16.336607
    }), [])

    const mapOptions = useMemo<MapOptions>(() => ({
        //mapId: "b181cac70f27f5e6",
        disableDefaultUI: true,
        clickableIcons: false,
      }), [])

    const mapRef = useRef<google.maps.Map>()

    const {isLoaded} = useLoadScript({
        googleMapsApiKey: 'AIzaSyAVKiCx1vsLNJjQ2g5gXJwzF8vVihFQjY8',
    })

    const onLoad = useCallback((map: google.maps.Map) => {
        mapRef.current = map
        fitBoundsToReports()
        console.log('map loaded')
    }, []);

    if(!isLoaded){
        return <div>Loading...</div>
    }
    const markersToDisplay: any = topTenReportsForDisplay.filter(report => report.lat !== null && report.lng !== null)
    .map(report => <MarkerF key={report.id} position={{lat: report.lat, lng: report.lng}} icon={markerIconsMap.get(report.group)} onClick={()=>{handleActiveReport(report)}}>
        {activeReport !== null && activeReport.id === report.id ? (
                    <InfoWindowF position={{lat: activeReport.lat, lng: activeReport.lng} } onCloseClick={onInfoWindowClosing} options={{maxWidth: 300}}>
                        <InfoWindowContent report = {activeReport}></InfoWindowContent>
                    </InfoWindowF>
        ) : null}
    </MarkerF>)

    
    return (
        <MapContainer className="map-container">
            <GoogleMap zoom={13} center={mapCenter} mapContainerClassName="map" options={mapOptions} onLoad={onLoad}>
                {markersToDisplay}
            </GoogleMap>
        </MapContainer>
    )
}

export default Map