import {MainLayoutContainer} from "./mainLayoutStyle"

import Map from "../Map/Map";

const MainLayout = () => {
    return ( 
        <MainLayoutContainer>
            <div className="div1"> <p>Hello</p> </div>
            <div className="div2"> <p>Hello</p> </div>
            <div className="div3"> <p>Hello</p> </div>
            <Map></Map>
            <div className="div5"> <p>Hello</p> </div>
            <div className="div6"> <p>Hello</p> </div>
            <div className="div7"> <p>Hello</p> </div>
        </MainLayoutContainer>
     );
}
 
export default MainLayout;