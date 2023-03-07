import React from "react";
import { IGlobalContextType, IGlobalContextValue } from "../context/types"

const GlobalContext = React.createContext<IGlobalContextValue>({
       teamCardStatsWindowState: {
              teamCardStatsWindow1: { components: [] },
              teamCardStatsWindow2: { components: [] }
       },
       reports: [],
       dispatch: () => { }
});

export default GlobalContext;