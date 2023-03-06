import styled from 'styled-components';

export const MapContainer = styled.div`
    .map {
        width: 100%;
        height: 100%;
    }
`

export const InfoWindowContainer = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 2em;

    .info{
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        gap: .7em
    }
    .title{
        font-weight: 500;
    }
    .description{
        margin: 0;
    }
    .details{
        display: flex;
        justify-content: flex-end;
        align-items: center;
        width: 100%;

        .date-created{
            font-size: .9em;
        }
    }
    img{
        width: 8em;
        height: 10em;
        object-fit: contain;
    }
`
