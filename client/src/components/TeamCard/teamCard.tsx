//#region ComponentsStyles
import {
    TeamCardContainer,
    TeamCardSymbol,
    TeamCardName,
    CardName,
    TeamCardCounter,
    Counter
}
    from "./teamCardStyle";
// #endregion

//#region HelperFunctions
import {
    returnSymbol,
    determineTeamCard
} from "../../utils/helper";
// #endregion

import { forwardRef } from "react";

type TeamCardProps = {
    width: number;
    height: number;
    isExtended: boolean;
};

const TeamCard = forwardRef<HTMLDivElement, TeamCardProps>((props, ref) => {

    return (
        <TeamCardContainer
            ref={ref}
            height={props.height}
            width={props.width}
            className={determineTeamCard(props.isExtended)}
        >
            <TeamCardSymbol>
                {returnSymbol("triangle")}
            </TeamCardSymbol>
            <TeamCardName>
                <CardName>Tim 1</CardName>
            </TeamCardName>
            <TeamCardCounter>
                <Counter>145</Counter>
            </TeamCardCounter>
        </TeamCardContainer>
    );
})

export default TeamCard;