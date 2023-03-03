//#region Importing Components
    import TeamCard from "../TeamCard/teamCard";
// #endregion

//#region Importing StyledComponents
import {TeamCardPlaceHolder} from "./TeamCounterStyle";
// #endregion

const TeamCounter  = () => {
    return ( 
        <TeamCardPlaceHolder className="team-card__expanded"/>
     );
}
 
export default TeamCounter;