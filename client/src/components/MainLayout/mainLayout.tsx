//#region Importing ComponentsStyles
import {MainLayoutContainer} from "./mainLayoutStyle"
// #endregion

//#region Importing Components
    import Map from "../Map/Map";
    import TeamCard from "../TeamCard/teamCard";
    import TeamCounter from "../TeamCounter/TeamCounter";
import { useState } from "react";
// #endregion

const MainLayout = () => {

    const [isExtended,setIsExtended] = useState(false)

    return ( 
        <MainLayoutContainer>
            <div className="div1"> <p>Hello</p> </div>
            <div className="div2"> <p>Hello</p> </div>
            <div className="team-counter"> 
                <TeamCounter/>
            </div>
            <Map/>
            <div className="div5"><p>Hello</p></div>
            <div className="cards-container"> 
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/>
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/>
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/>
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/>
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/>
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/>
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/>
                <TeamCard onClick={()=>setIsExtended(!isExtended)} width={100} height={100} isExtended={isExtended}/> 
            </div>
            <div className="div7"> <p>Hello</p> </div>
        </MainLayoutContainer>
     );
}
 
export default MainLayout;