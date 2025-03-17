import { FC } from "react";

interface PlanProps {
  plan: {
    planID: string;
    planName: string;
    planDescription: string;
    planPrice: number;
  };
}

const PlanCard: FC<PlanProps> = ({ plan }) => {
  return (
    <div className="card shadow-lg text-center p-4 border-0" style={{ maxWidth: "400px" }}>
      <div className="card-body">
        <h3 className="text-primary">{plan.planName}</h3>
        <p className="text-muted">{plan.planDescription}</p>
        <strong className="text-success fs-4">€{plan.planPrice.toFixed(2)}</strong>
        <div className="mt-3">
          <button className="btn btn-primary">Join Now</button>
        </div>
      </div>
    </div>
  );
};

export default PlanCard;
