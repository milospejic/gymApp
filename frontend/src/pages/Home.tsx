import { useEffect, useState } from "react";
import { membershipPlanService, MembershipPlan } from "../services/membershipPlanService";
import PlanCard from "../components/PlanCard";

const Home = () => {
  const [plans, setPlans] = useState<MembershipPlan[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        const data = await membershipPlanService.getMembershipPlans();
        setPlans(data);
      } catch {
        setError("Failed to load membership plans. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchPlans();
  }, []);

  return (
    <div className="container mt-5 vh-100">
      <h1 className="text-center text-light">🏋️ Welcome to My Gym! 🏋️</h1>
      <p className="text-center text-light">
        Transform your body, push your limits, and achieve your fitness goals. No pain, no gain! 💪🔥
      </p>

      {loading && <div className="text-center"><div className="spinner-border text-light"></div></div>}
      {error && <p className="text-danger text-center">{error}</p>}

      {/* Carousel */}
      {!loading && !error && plans.length > 0 && (
        <div id="planCarousel" className="carousel slide mt-4" data-bs-ride="carousel">
          <div className="carousel-inner">
            {plans.map((plan, index) => (
              <div key={plan.planID} className={`carousel-item ${index === 0 ? "active" : ""}`}>
                <div className="d-flex justify-content-center">
                  <PlanCard plan={plan} />
                </div>
              </div>
            ))}
          </div>

          <button className="carousel-control-prev" type="button" data-bs-target="#planCarousel" data-bs-slide="prev">
            <span className="carousel-control-prev-icon" aria-hidden="true"></span>
            <span className="visually-hidden">Previous</span>
          </button>
          <button className="carousel-control-next" type="button" data-bs-target="#planCarousel" data-bs-slide="next">
            <span className="carousel-control-next-icon" aria-hidden="true"></span>
            <span className="visually-hidden">Next</span>
          </button>
        </div>
      )}
    </div>
  );
};

export default Home;
