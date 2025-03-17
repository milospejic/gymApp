import { useEffect, useState } from "react";
import PlanCard from "../components/PlanCard";
import { getMembershipPlans, MembershipPlan } from "../services/membershipPlanService";

const Plans = () => {
  const [plans, setPlans] = useState<MembershipPlan[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        const data = await getMembershipPlans();
        setPlans(data);
      } catch (error) {
        console.error("Error fetching plans:", error);
        setError("Failed to load membership plans. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchPlans();
  }, []);

  return (
    <div className="container mt-5">
      <h1 className="text-center text-light"> Membership Plans </h1>

      {/* Show loading spinner */}
      {loading && (
        <div className="text-center mt-4">
          <div className="spinner-border text-primary"></div>
        </div>
      )}

      {/* Show error message */}
      {error && <p className="text-danger text-center mt-3">{error}</p>}

      {/* Show empty state if no plans exist */}
      {!loading && !error && plans.length === 0 && (
        <p className="text-center text-muted mt-3">No membership plans available at the moment.</p>
      )}

      {/* Render plans in a grid */}
      {!loading && !error && plans.length > 0 && (
        <div className="row mt-4">
          {plans.map((plan) => (
            <div key={plan.planID} className="col-md-4 mb-4 d-flex justify-content-center">
              <PlanCard plan={plan} />
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Plans;
