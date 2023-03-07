// #region Importing Animation libary
import { gsap } from "gsap";
//#endregion

// #region Importing StyledComponents
import { Symbol } from "../components/TeamCard/teamCardStyle";
// #endregion

//#region Importing SHAPES
import {
  Triangle,
  Circle,
  CircleWrapper
}
  from "../components/TeamCard/teamCardStyle";
// #endregion

import { ITeamCardStats } from "../context/types";

import TeamCard from "../components/TeamCard/teamCard";

//#region Importing Types
import { AnyStyledComponent } from 'styled-components';
// #endregion


export const returnSymbol = (shape: string) => {

  let ShapeComponent: AnyStyledComponent;
  switch (shape) {
    case 'circle':
      ShapeComponent = Circle;
      break;
    case 'triangle':
      ShapeComponent = Triangle;
      break;

    default:
      ShapeComponent = Circle;
      break;
  }

  return (
    <CircleWrapper>
      <Symbol as={ShapeComponent}>

      </Symbol>
    </CircleWrapper>

  );
}

export const determineTeamCard = (isExtended: boolean) => {
  return isExtended == true ? "team-card_expand" : "team-card__expanded"
}

export const getRandomCard =(max:number)=>{
  return Math.floor(Math.random() * max);
}
