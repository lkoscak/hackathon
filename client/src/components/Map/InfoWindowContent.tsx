import { Report } from "../../context/types"

type InfoWindowContentProps = {
	report: Report
}

const InfoWindowContent : React.FC<InfoWindowContentProps> = ({report}) => {
  return (
    <div>
        <div>{report.title}</div>
        <p>{report.description}</p>
    </div>
  )
}

export default InfoWindowContent