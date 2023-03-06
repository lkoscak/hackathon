import { Report } from "../../context/types"

import { InfoWindowContainer } from "./MapStyle"

import moment from "moment"

type InfoWindowContentProps = {
	report: Report
}

const InfoWindowContent : React.FC<InfoWindowContentProps> = ({report}) => {
  return (
    <InfoWindowContainer>
        {
          report.images.length > 0 ? <img src={report.images[0]}></img> : <div className="title">{report.title}</div>
        }
        <div className="info">
          {
            report.images.length !== 0 ? <div className="title">{report.title}</div> : null
          }
          <p className="description">{report.description}</p>
          <div className="details">
            <div className="date-created">{moment(report.created).format('DD.MM.yyyy. HH:mm')}</div>
          </div>
        </div>
    </InfoWindowContainer>
  )
}

export default InfoWindowContent