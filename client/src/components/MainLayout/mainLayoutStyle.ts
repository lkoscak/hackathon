import * as React from 'react';
import styled from 'styled-components';

export const MainLayoutContainer = styled.div`
    display: grid;
    width:100%;
    height:100vh;
    grid-template-columns: repeat(5, 1fr);
    grid-template-rows: repeat(5, 1fr);
    grid-column-gap: 0px;
    grid-row-gap: 0px;

    .div1 { grid-area: 2 / 1 / 6 / 2; }
    .div2 { grid-area: 1 / 1 / 2 / 4; }
    .team-counter { grid-area: 1 / 4 / 2 / 6; }
    .map-container { grid-area: 2 / 2 / 5 / 5; }
    .div5 { grid-area: 5 / 2 / 6 / 5; }
    .cards-container { grid-area: 2 / 5 / 3 / 6; }
    .div7 { grid-area: 3 / 5 / 6 / 6; }

    .team-counter
    {        
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        grid-template-rows: 1fr;
        grid-column-gap: 0px;
        grid-row-gap: 0px;
            .team-card__counter  { grid-area: 1 / 3 / 2 / 8;  }
            .team-card__expanded { grid-area: 1 / 1 / 2 / 3;  }
    }

    .cards-container { 
        display:grid; 
        padding:1%;
        grid-template-columns: repeat(4, 1fr);
        grid-template-rows: repeat(2, 1fr);
        grid-column-gap: 2px;
        grid-row-gap: 2px;

        .team-card_expand{
        }
    }
`
