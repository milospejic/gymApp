import { FC } from "react";
import { Check, Zap } from "lucide-react";
import "./PlanCard.css";

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
    <div className="glass-card plan-card-container">
      <div className="plan-card-body">
        <div className="plan-card-header">
          <div>
            <h3 className="plan-name">{plan.planName}</h3>
            <p className="plan-description">{plan.planDescription}</p>
          </div>
          <div className="plan-icon">
            <Zap className="text-primary" size={20} />
          </div>
        </div>

        <div className="plan-price-container">
          <span className="plan-price">${plan.planPrice}</span>
          <span className="plan-period">/month</span>
        </div>

        <ul className="plan-features">
          {["Full Gym Access", "Custom Workout Plan", "Mobile App Tracking", "Basic Support"].map((feature) => (
            <li key={feature} className="plan-feature-item">
              <div className="feature-icon-bg">
                <Check className="text-success" size={12} />
              </div>
              {feature}
            </li>
          ))}
        </ul>

        <button className="btn-premium plan-button">
          Select Plan
        </button>
      </div>
      
      <div className="plan-card-footer-glow" />
    </div>
  );
};

export default PlanCard;