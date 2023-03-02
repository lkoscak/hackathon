import { useLoadScript } from "@react-google-maps/api"

import { MapContainer } from "./MapStyle";

import { useState, useMemo, useCallback, useRef } from "react";
import {
  GoogleMap,
  MarkerF,
  MarkerClusterer,
} from "@react-google-maps/api";

import useGlobalContext from "../../hooks/use-global-context";

import { markerIconsMap } from "../../utils/utils";

import car from '../../assets/markers/car/Car_3.png'

type LatLngLiteral = google.maps.LatLngLiteral;
type MapOptions = google.maps.MapOptions;

const Map = () => {

    const {reports} = useGlobalContext();

    const mapCenter = useMemo<LatLngLiteral>(() => ({
        lat: 46.305746,
        lng: 16.336607
    }), [])
    const mapOptions = useMemo<MapOptions>(() => ({
        //mapId: "b181cac70f27f5e6",
        disableDefaultUI: true,
        clickableIcons: false,
      }), [])

    const mapRef = useRef<GoogleMap>()

    const {isLoaded} = useLoadScript({
        googleMapsApiKey: 'AIzaSyAVKiCx1vsLNJjQ2g5gXJwzF8vVihFQjY8',
    })

    const onLoad = useCallback((map: any) => {
        mapRef.current = map
        console.log(reports)
    }, []);

    //const onMarkerLoad = useCallback((marker: any) => {
    //    console.log(marker)
    //}, []);

    if(!isLoaded){
        return <div>Loading...</div>
    }

    const markersToDisplay: any = reports.filter(report => report.lat !== null && report.lng !== null)
    .map(report => <MarkerF key={report.id} position={{lat: report.lat, lng: report.lng}} icon={markerIconsMap.get(report.group)}></MarkerF>)

    
    return (
        <MapContainer className="map-container">
            <GoogleMap zoom={13} center={mapCenter} mapContainerClassName="map" options={mapOptions} onLoad={onLoad}>
                {markersToDisplay}
            </GoogleMap>
        </MapContainer>
    )
}

export default Map