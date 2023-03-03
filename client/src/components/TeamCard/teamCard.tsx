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
    import 
        { 
            returnSymbol,
            determineTeamCard 
        } 
        from "../../utils/helper";
import { FunctionComponent } from "react";
// #endregion

interface componentProps {
    width:number,
    height:number,
    isExtended:boolean,
    onClick:()=>void
}

const TeamCard : FunctionComponent<componentProps> = ({width,isExtended,height,onClick}) => {
    return ( 
        <TeamCardContainer 
            onClick={onClick}
            height={height}
            width={width} 
            className={determineTeamCard(isExtended)}
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
}
 
export default TeamCard;