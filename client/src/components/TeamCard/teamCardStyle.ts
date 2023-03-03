import styled from 'styled-components';

interface ITeamCardContainerProps {
    width:number,
    height:number
}

export const TeamCardContainer = styled.div<ITeamCardContainerProps>`
    display:flex;
    border-radius:6px;
    flex-direction: column;
    background-color: #E0E0E0;
    transition: transform .3s ease-out;
    transform-origin:1500px 9px;
    width:${props => props.width }%;
    height:${props => props.height }%;
`
export const TeamCardSymbol = styled.div`
    display:flex;
    justify-content:center;
    align-items:center;
    padding:5px;
    border-radius:6px 6px 0px 0px;
    width:100%;
    height:50%;
`
export const TeamCardName = styled.div`
    display:flex;
    justify-content:center;
    align-items:center;
    width:100%;
    height:25%;
`
export const TeamCardCounter = styled.div`
    display:flex;
    align-items:center;
    justify-content:center;
    width:100%;
    height:25%;
    border-radius:0px 0px 6px 6px;
`

export const CardName = styled.span`
    font-size:.9rem;
`
export const Counter = styled.span`
    font-size:.9rem;
`

///SHAPES
export const Cone = styled.div`
    width: 0;
    height: 0;
    border-left: 70px solid transparent;
    border-right: 70px solid transparent;
    border-top: 100px solid #32557f;
    border-radius: 50%;
`;
export const Triangle = styled.div`
    display:flex;
    width: 0;
    height: 0;
    border-left: 10px solid transparent;
    border-right: 10px solid transparent;
    border-bottom: 20px solid red;
`;
export const Diamond = styled.div`
    width: 10%;
    height: 50px;
    border: 50px solid transparent;
    border-bottom-color: #32557f;
    position: relative;
    top: -50px;

    &::after {
        content: '';
        position: absolute;
        left: -50px;
        top: 50px;
        width: 0;
        height: 0;
        border: 50px solid transparent;
        border-top-color: #32557f;
    }
`;
export const Pentagon = styled.div`
    position: relative;
    width: 54px;
    box-sizing: content-box;
    border-width: 50px 18px 0;
    border-style: solid;
    border-color: #32557f transparent;

    &::before {
    content: '';
    position: absolute;
    height: 0;
    width: 0;
    top: -85px;
    left: -18px;
    border-width: 0 45px 35px;
    border-style: solid;
    border-color: transparent transparent #32557f;
    }
`;
export const Hexagon = styled.div`
    width: 100px;
    height: 55px;
    background: #32557f;
    position: relative;

    &::before {
    content: '';
    position: absolute;
    top: -25px;
    left: 0;
    width: 0;
    height: 0;
    border-left: 50px solid transparent;
    border-right: 50px solid transparent;
    border-bottom: 25px solid #32557f;
    }

    &::after {
    content: '';
    position: absolute;
    bottom: -25px;
    left: 0;
    width: 0;
    height: 0;
    border-left: 50px solid transparent;
    border-right: 50px solid transparent;
    border-top: 25px solid #32557f;
    }
`;
export const Star = styled.div`
    position: relative;
    display: block;
    width: 0px;
    height: 0px;
    margin: 50px 0;
    color: #32557f;
    border-left: 100px solid transparent;
    border-right: 100px solid transparent;
    border-bottom: 70px solid #32557f;
    transform: rotate(35deg);

    &::before {
    content: '';
    position: absolute;
    top: -45px;
    left: -65px;
    display: block;
    height: 0;
    width: 0;
    border-left: 30px solid transparent;
    border-right: 30px solid transparent;
    border-bottom: 80px solid #32557f;
    transform: rotate(-35deg);
    }

    &::after {
    content: '';
    position: absolute;
    top: 3px;
    left: -105px;
    display: block;
    width: 0px;
    height: 0px;
    color: #32557f;
    border-left: 100px solid transparent;
    border-right: 100px solid transparent;
    border-bottom: 70px solid #32557f;
    transform: rotate(-70deg);
    }
`;
export const Cross = styled.div`
    background: #32557f;
    height: 50px;
    position: relative;
    width: 10px;

    &::after {
    background: #32557f;
    content: '';
    height: 10px;
    left: -20px;
    position: absolute;
    top: 20px;
    width: 50px;
    }
`;
export const Egg = styled.div`
    display: block;
    width: 130px;
    height: 175px;
    background-color: #32557f;
    border-radius: 50% 50% 50% 50% / 60% 60% 40% 40%;
`;

export const CircleWrapper = styled.div`
    display:flex;
    justify-content:center;
    align-items:center;
    width: 50%;
    height: 100%;
    background: black;
    border-radius: 50%
`

export const Circle = styled.div`
    display:flex;
    width: 70%;
    height: 70%;
    background: red;
    border-radius: 50%
`
export const Symbol = styled.div``;




