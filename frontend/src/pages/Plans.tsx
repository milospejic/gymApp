import { useEffect, useState } from "react";
import { membershipPlanService } from "../services/membershipPlanService";
import { MembershipPlan } from "../interfaces";
import PlanCard from "../components/PlanCard";

export const Plans = () => {
  const [plans, setPlans] = useState<MembershipPlan[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        const data = await membershipPlanService.getAllPlans();
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
    <div className="container section-padding animate-fade-in">
      <div className="text-center" style={{ marginBottom: '5rem' }}>
        <h1 className="hero-title" style={{ fontSize: 'clamp(2.5rem, 8vw, 4rem)', marginBottom: '1.5rem' }}>
          Our <span className="text-gradient">Membership Plans</span>
        </h1>
        <p className="hero-description" style={{ maxWidth: '700px' }}>
          Find the perfect membership to crush your fitness goals. Transparent pricing for all levels of athletes.
        </p>
      </div>

      {loading ? (
        <div className="plans-grid">
          {[1, 2, 3].map(i => (
            <div key={i} className="glass-card" style={{ height: '450px', opacity: 0.5 }}></div>
          ))}
        </div>
      ) : error ? (
        <div className="glass-card text-center" style={{ padding: '3rem', borderColor: 'var(--error)' }}>
          <p className="text-error font-bold">{error}</p>
        </div>
      ) : (
        <div className="plans-grid">
          {plans.map((plan) => (
            <PlanCard key={plan.planID} plan={plan} />
          ))}
        </div>
      )}

      {/* Benefits list section */}
      <section style={{ marginTop: '8rem' }}>
        <div className="text-center" style={{ marginBottom: '4rem' }}>
          <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '2rem', marginBottom: '0.5rem' }}>Included In All Plans</h2>
          <div style={{ height: '4px', width: '60px', background: 'var(--primary)', margin: '0 auto', borderRadius: '2px' }} />
        </div>
        
        <div className="grid grid-cols-3 gap-8">
          {[
            { title: "24/7 Access", desc: "Train whenever inspiration strikes." },
            { title: "Smart Lockers", desc: "Secure storage for all your gear." },
            { title: "High-Speed WiFi", desc: "Stay connected while you train." },
            { title: "Shower Facilities", desc: "Premium amenities and clean towels." },
            { title: "Free Parking", desc: "Convenient parking for all members." },
            { title: "Mobile App", desc: "Track workouts and book classes." }
          ].map((item, i) => (
            <div key={i} className="glass-card" style={{ padding: '1.5rem' }}>
              <h3 className="uppercase font-black tracking-tight" style={{ fontSize: '1rem', marginBottom: '0.5rem', color: 'var(--primary)' }}>{item.title}</h3>
              <p className="text-muted font-medium" style={{ fontSize: '0.875rem' }}>{item.desc}</p>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
};

export default Plans;