import React from "react";
import {IGlobalContextType} from "../context/types"

const GlobalContext = React.createContext<IGlobalContextType>({
       reports:[]
});

export default GlobalContext;