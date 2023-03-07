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
    TeamName: string;
    ReportCount: number
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
                <CardName>{props.TeamName}</CardName>
            </TeamCardName>
            <TeamCardCounter>
                <Counter>{props.ReportCount}</Counter>
            </TeamCardCounter>
        </TeamCardContainer>
    );
})

export default TeamCard;