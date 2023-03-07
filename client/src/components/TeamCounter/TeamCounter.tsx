//#region Importing StyledComponents
import { TeamCardPlaceHolder } from "./TeamCounterStyle";
// #endregion

//#region React
import { forwardRef } from 'react';
// #endregion

type TeamCounterProps = {
    children: any
}
const TeamCounter = forwardRef<HTMLDivElement, TeamCounterProps>((props, ref) => {
    return (
        <TeamCardPlaceHolder
            ref={ref}
            className="team-card__expanded"
        >
            {props.children}
        </TeamCardPlaceHolder>
    );
})

export default TeamCounter;