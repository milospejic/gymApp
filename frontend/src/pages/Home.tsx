import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { membershipPlanService } from "../services/membershipPlanService";
import { MembershipPlan } from "../interfaces";
import PlanCard from "../components/PlanCard";
import { ArrowRight, ChevronRight, Activity, Users, Shield, Trophy, Dumbbell } from "lucide-react";
import "./Home.css";

const Home = () => {
  const [plans, setPlans] = useState<MembershipPlan[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
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
    <div className="home-container">
      
      {/* Hero Section */}
      <section className="hero-section bg-mesh">
        <div className="hero-content">
          <div className="hero-badge animate-fade-in">
            <span className="badge-dot">
              <span className="badge-dot-ping"></span>
              <span className="badge-dot-inner"></span>
            </span>
            <span className="text-xs font-bold text-primary uppercase tracking-widest">The Ultimate Fitness Experience</span>
          </div>

          <h1 className="hero-title animate-fade-in">
            Unleash Your <br />
            <span className="text-gradient">Beast Mode</span>
          </h1>
          
          <p className="hero-description animate-fade-in delay-100">
            Transform your body, push your limits, and achieve your fitness goals with world-class facilities and expert guidance.
          </p>

          <div className="hero-actions animate-fade-in delay-200">
            <Link to="/register" className="btn-premium">
              Start Free Trial <ArrowRight size={20} />
            </Link>
            <Link to="/about" className="btn-outline">
              Explore Gym
            </Link>
          </div>
        </div>
      </section>

      {/* Stats Section */}
      <section className="stats-section container">
        <div className="stats-grid">
          {[
            { icon: <Users size={24} className="text-primary" />, val: "1.2k+", label: "Active Members" },
            { icon: <Activity size={24} className="text-secondary" />, val: "50+", label: "Daily Classes" },
            { icon: <Trophy size={24} className="text-success" />, val: "25+", label: "Expert Trainers" },
            { icon: <Shield size={24} className="text-accent" />, val: "24/7", label: "Security Access" },
          ].map((stat, i) => (
            <div key={i} className="glass-card stat-card animate-fade-in" style={{ animationDelay: `${i * 100}ms` }}>
              <div className="stat-icon-wrapper">
                {stat.icon}
              </div>
              <div className="stat-value">{stat.val}</div>
              <div className="stat-label">{stat.label}</div>
            </div>
          ))}
        </div>
      </section>

      {/* Membership Plans */}
      <section className="container section-padding">
        <div className="plans-section-header">
          <div className="plans-header-content">
            <h2 className="text-gradient uppercase tracking-tighter" style={{ fontSize: '3rem', marginBottom: '1rem' }}>
              Choose Your Plan
            </h2>
            <p className="text-muted font-medium" style={{ fontSize: '1.125rem' }}>
              Simple, transparent pricing for everyone. Whether you're just starting out or a pro athlete, we have the right plan for you.
            </p>
          </div>
          <Link to="/plans" className="text-primary font-bold flex items-center gap-2 tracking-wide hover:text-secondary transition-all">
            VIEW ALL FEATURES <ChevronRight size={20} />
          </Link>
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
      </section>
      
      {/* CTA Section */}
      <section className="cta-section container">
        <div className="glass-card cta-card animate-fade-in">
          <div className="cta-card-bg" />
          <div className="cta-content">
            <h2 className="uppercase font-black tracking-tighter" style={{ fontSize: 'clamp(2rem, 5vw, 4rem)', lineHeight: 1, marginBottom: '1.5rem' }}>
              Ready to Join <br />
              <span className="text-primary">The Community?</span>
            </h2>
            <p className="text-muted font-medium" style={{ fontSize: '1.125rem', maxWidth: '450px' }}>
              Get started today and receive a complimentary personal training session and nutrition guide.
            </p>
          </div>
          <div className="cta-actions">
            <Link to="/register" className="btn-premium" style={{ padding: '1.25rem 3rem', fontSize: '1.125rem' }}>
              Get Started Now
            </Link>
          </div>
        </div>
      </section>

      {/* Simple Footer */}
      <footer className="footer">
        <div className="container">
          <div className="footer-logo">
            <Dumbbell className="text-primary" size={24} />
            <span className="font-black tracking-tighter italic uppercase" style={{ fontSize: '1.25rem' }}>
              My<span className="text-primary">Gym</span>
            </span>
          </div>
          <p className="footer-copyright uppercase">
            &copy; 2026 MyGym Beast Mode. All rights reserved.
          </p>
        </div>
      </footer>

    </div>
  );
};

export default Home;