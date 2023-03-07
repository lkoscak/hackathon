//#region Importing ComponentsStyles
import { MainLayoutContainer } from "./mainLayoutStyle"
import { TeamCardCounter, TeamReportCounter } from "../TeamCounter/TeamCounterStyle";
// #endregion

//#region Importing Components
import Map from "../Map/Map";
import TeamCard from "../TeamCard/teamCard";
import TeamCounter from "../TeamCounter/TeamCounter";
// #endregion

//#region Importing React
import { useState, useEffect, useRef } from "react";
// #endregion

//#region Importing GlobalContext
import useGlobalContext from "../../hooks/use-global-context";
// #endregion

//#region Importing AnimationLibarry
import { gsap } from "gsap";
// #endregion

//#region Importing Types
import { ITeamCard } from "../../context/types";
// #endregion

//#region Importing Helper functions
import { getRandomCard } from "../../utils/helper";
// #endregion

const MainLayout = () => {

    const [isExtended, setIsExtended] = useState(false);
    const [cardToExtend, setCardToExtend] = useState<number>();
    const [teamReportCount, setTeamReportCount] = useState<number>();


    const { teamCardStatsWindowState, dispatch } = useGlobalContext();

    let cardsRefs = useRef<Array<HTMLDivElement | null>>(
        Array.from({ length: teamCardStatsWindowState.teamCardStatsWindow1.components.length })
    );
    let teamCounterRef = useRef<HTMLDivElement | null>(null);
    let shownCardRef = useRef<HTMLDivElement | null>(null);

    const cardsContainerRef = useRef<HTMLDivElement | null>(null);

    const moveComponent = (component: any, fromParent: string, toParent: string, reportCount: number) => {
        dispatch({ type: "MOVE_COMPONENT", payload: { component, fromParent, toParent, reportCount } });
        setTeamReportCount(reportCount);
    }

    const handleMoveComponent = (isShown: boolean) => {
        if (cardToExtend && isShown == false) {
            const componentToMove = teamCardStatsWindowState.teamCardStatsWindow1.components[cardToExtend];
            moveComponent(componentToMove, "teamCardStatsWindow1", "teamCardStatsWindow2", componentToMove.reportCount);
        }
        else if (cardToExtend && isShown) {
            const componentToMove = teamCardStatsWindowState.teamCardStatsWindow2.components[0];
            moveComponent(componentToMove, "teamCardStatsWindow2", "teamCardStatsWindow1", componentToMove.reportCount);
        }
    };


    const animteMoving = (isShown: boolean) => {

        let componentRef: HTMLDivElement | null = null;
        let fromParent: HTMLDivElement | null = null;
        let toParent: HTMLDivElement | null = null;

        if (isShown == true) {
            componentRef = shownCardRef.current;
            fromParent = teamCounterRef.current;
            toParent = cardsContainerRef.current;
        }

        else {
            if (cardToExtend)
                componentRef = cardsRefs.current[cardToExtend];

            fromParent = cardsContainerRef.current;
            toParent = teamCounterRef.current;
        }

        const fromRect = fromParent?.getBoundingClientRect();
        const toRect = toParent?.getBoundingClientRect();

        if (fromRect && toRect) {
           gsap.to(componentRef, {
                duration: 0.5,
                x: toRect.left - fromRect.left,
                y: toRect.top - fromRect.top,
                onComplete: () => {
                    if (componentRef != null)
                        handleMoveComponent(isShown);
                    //toParent?.appendChild(componentRef);
                    else return;
                }
            })
        }

    }

    const mapTeamCard = (isShown: boolean, componentToMap: ITeamCard[]) => {
        return (
            componentToMap.map(
                (teamCard, index) => {
                    return (
                        <TeamCard
                            ref={isShown == false ? el => cardsRefs.current[index] = el : shownCardRef}
                            key={teamCard.id}
                            width={100}
                            height={100}
                            isExtended={isExtended}
                            TeamName={teamCard.teamName}
                            ReportCount={teamCard.reportCount}
                        />)
                })
        )
    }

   
    useEffect(() => {
        setCardToExtend(getRandomCard(9))
        const interval = setInterval(() => {
            setCardToExtend(getRandomCard(9))
            const isShownCard: boolean = teamCardStatsWindowState.teamCardStatsWindow2.components.length != 0;
            animteMoving(isShownCard);
        }, 3000)
        return () => {

            clearInterval(interval);
        };

    }, [cardToExtend])

    return (
        <MainLayoutContainer>
            <div className="div1"> <p>Hello</p> </div>
            <div className="div2"> <p>Hello</p> </div>

            <div className="team-counter">
                <TeamCounter ref={teamCounterRef} >
                    {mapTeamCard(true, teamCardStatsWindowState.teamCardStatsWindow2.components)}
                </TeamCounter>
                <TeamCardCounter className="team-card__counter">
                    <TeamReportCounter>Total number :</TeamReportCounter>
                    <TeamReportCounter>{teamReportCount}</TeamReportCounter>
                </TeamCardCounter>
            </div>

            <Map />

            <div className="div5"><p>Hello</p></div>

            <div ref={cardsContainerRef} className="cards-container">
                {mapTeamCard(false, teamCardStatsWindowState.teamCardStatsWindow1.components)}
            </div>

            <div className="div7"> <p>Hello</p> </div>
        </MainLayoutContainer>
    );
}

export default MainLayout;