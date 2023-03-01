import styled from 'styled-components';

export const MainLayoutContainer = styled.div`
    display: grid;
    width:100%;
    grid-template-columns: repeat(5, 1fr);
    grid-template-rows: repeat(5, 1fr);
    grid-column-gap: 0px;
    grid-row-gap: 0px;

    .div1 { grid-area: 2 / 1 / 6 / 2; }
    .div2 { grid-area: 1 / 1 / 2 / 5; }
    .div3 { grid-area: 1 / 5 / 2 / 6; }
    .div4 { grid-area: 2 / 2 / 5 / 5; }
    .div5 { grid-area: 5 / 2 / 6 / 5; }
    .div6 { grid-area: 2 / 5 / 3 / 6; }
    .div7 { grid-area: 3 / 5 / 6 / 6; }
`
