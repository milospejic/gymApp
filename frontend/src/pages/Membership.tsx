import { useEffect, useState } from "react";
import { memberService } from "../services/memberService";
import { membershipPlanService } from "../services/membershipPlanService";
import { membershipService } from "../services/membershipService";
import { Member, MembershipPlan } from "../interfaces";
import { Check, ShieldCheck, AlertCircle, Calendar, Zap, ArrowRight, ShieldAlert, Loader2 } from "lucide-react";
import axios from "axios";
import "./Profile.css";

export const MembershipPage = () => {
  const [currentUser, setCurrentUser] = useState<Member | null>(null);
  const [plans, setPlans] = useState<MembershipPlan[]>([]);
  const [loading, setLoading] = useState(true);
  const [processingId, setProcessingId] = useState<string | null>(null);
  const [selectedDurations, setSelectedDurations] = useState<Record<string, number>>({});

  const loadData = async () => {
    try {
      const [userData, plansData] = await Promise.all([
        memberService.getMyInfo(),
        membershipPlanService.getAllPlans()
      ]);
      
      setCurrentUser(userData);
      setPlans(plansData);
      
      const initialDurations: Record<string, number> = {};
      plansData.forEach(plan => {
        initialDurations[plan.planID] = 1;
      });
      setSelectedDurations(initialDurations);

    } catch {
      console.error("Failed to load data");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const handleDurationChange = (planId: string, duration: number) => {
    setSelectedDurations(prev => ({ ...prev, [planId]: duration }));
  };

  const handleRenew = async (planId: string) => {
    if (!currentUser || !currentUser.membershipId) return;

    const duration = selectedDurations[planId] || 1;
    setProcessingId(planId);
    
    try {
      await membershipService.updateMembership(currentUser.membershipId, {
        membershipPlanId: planId,
        planDuration: duration 
      });
      
      alert("Membership successfully updated! Please pay the fee at the front desk.");
      await loadData();
      
    } catch (err) {
      if (axios.isAxiosError(err)) {
        alert(err.response?.data || "Failed to update membership.");
      } else {
        alert("An unexpected error occurred.");
      }
    } finally {
      setProcessingId(null);
    }
  };

  const handleCancel = async () => {
    if (!currentUser || !currentUser.membershipId) return;

    if (!window.confirm("Are you sure you want to cancel your membership? This will end your access immediately.")) {
      return;
    }

    setLoading(true);
    try {
      await membershipService.cancelMembership(currentUser.membershipId);
      alert("Membership successfully cancelled.");
      await loadData();
    } catch (err) {
      if (axios.isAxiosError(err)) {
        alert(err.response?.data || "Failed to cancel membership.");
      } else {
        alert("An unexpected error occurred.");
      }
    } finally {
      setLoading(false);
    }
  };

  if (loading) return (
    <div className="container" style={{ minHeight: '60vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
      <div className="text-center">
        <Loader2 className="spinner text-primary" size={40} style={{ marginBottom: '1rem' }} />
        <div className="form-subtitle animate-pulse">Syncing Plans...</div>
      </div>
    </div>
  );
  
  if (!currentUser) return (
    <div className="container" style={{ paddingTop: '5rem' }}>
      <div className="glass-card text-center" style={{ padding: '3rem', maxWidth: '500px', margin: '0 auto', borderColor: 'var(--error)' }}>
        <ShieldAlert className="text-error" size={48} style={{ marginBottom: '1rem' }} />
        <h2 className="uppercase font-black tracking-tight" style={{ fontSize: '1.5rem', marginBottom: '0.5rem' }}>Access Denied</h2>
        <p className="text-muted font-medium">Failed to synchronize your member profile. Please try re-authenticating.</p>
      </div>
    </div>
  );

  const isActive = currentUser.membership && new Date(currentUser.membership.membershipTo) > new Date();

  return (
    <div className="container section-padding animate-fade-in">
      {/* Page Header */}
      <div className="text-center" style={{ marginBottom: '5rem' }}>
        <h1 className="hero-title" style={{ fontSize: 'clamp(2.5rem, 8vw, 4rem)', marginBottom: '1.5rem' }}>
          Member <span className="text-gradient">Privileges</span>
        </h1>
        <p className="hero-description" style={{ maxWidth: '600px' }}>
          Manage your subscription, explore premium tiers, and optimize your fitness journey.
        </p>
      </div>

      {/* Current Status Alert */}
      {isActive ? (
        <div className="glass-card" style={{ padding: '2rem', marginBottom: '4rem', display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: '2rem', flexWrap: 'wrap', borderLeft: '4px solid var(--success)' }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '1.5rem' }}>
            <div style={{ padding: '1rem', background: 'rgba(16, 185, 129, 0.1)', borderRadius: '1rem', color: 'var(--success)' }}>
              <ShieldCheck size={32} />
            </div>
            <div>
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', marginBottom: '0.25rem' }}>
                <h3 className="uppercase font-black tracking-tight" style={{ fontSize: '1.25rem' }}>Active Membership</h3>
                <span className="badge-active">Verified</span>
              </div>
              <p className="text-muted font-medium" style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', fontSize: '0.875rem' }}>
                <Calendar size={16} className="text-primary" /> Valid until {new Date(currentUser.membership!.membershipTo).toLocaleDateString(undefined, { dateStyle: 'long' })}
              </p>
            </div>
          </div>
          <button 
            onClick={handleCancel}
            className="btn-outline"
            style={{ color: 'var(--error)', borderColor: 'rgba(239, 68, 68, 0.2)' }}
          >
            Terminate Access
          </button>
        </div>
      ) : (
        <div className="glass-card" style={{ padding: '2rem', marginBottom: '4rem', display: 'flex', alignItems: 'center', gap: '1.5rem', background: 'rgba(245, 158, 11, 0.05)', borderLeft: '4px solid var(--warning)' }}>
          <div style={{ padding: '0.75rem', background: 'rgba(245, 158, 11, 0.1)', borderRadius: '0.75rem', color: 'var(--warning)' }}>
            <AlertCircle size={28} />
          </div>
          <div>
            <h3 className="uppercase font-black tracking-tight" style={{ fontSize: '1.125rem' }}>No Active Subscription</h3>
            <p className="text-muted font-medium" style={{ fontSize: '0.875rem' }}>Select a plan below to unlock gym facilities and start training.</p>
          </div>
        </div>
      )}

      {/* Pricing Cards Grid */}
      <div className="plans-grid">
        {plans.map((plan) => {
          const isCurrentlyActivePlan = isActive && currentUser.membership?.membershipPlan?.planID === plan.planID;
          
          return (
            <div 
              key={plan.planID} 
              className={`glass-card membership-plan-card ${isCurrentlyActivePlan ? 'plan-active-ring' : ''} ${isActive && !isCurrentlyActivePlan ? 'plan-inactive' : ''}`}
              style={{ padding: '2.5rem', position: 'relative' }}
            >
              {isCurrentlyActivePlan && (
                <div className="active-plan-badge">Current Active Plan</div>
              )}

              <div style={{ flexGrow: 1 }}>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '1.5rem' }}>
                  <div>
                    <h3 className="uppercase font-black tracking-tighter" style={{ fontSize: '1.5rem', marginBottom: '0.25rem' }}>{plan.planName}</h3>
                    <p className="text-dim font-bold uppercase tracking-widest" style={{ fontSize: '0.625rem' }}>{plan.planDescription}</p>
                  </div>
                  <div style={{ padding: '0.5rem', background: 'rgba(59, 130, 246, 0.1)', borderRadius: '0.5rem', color: 'var(--primary)' }}>
                    <Zap size={20} />
                  </div>
                </div>
                
                <div style={{ display: 'flex', alignItems: 'baseline', gap: '0.25rem', marginBottom: '2rem' }}>
                  <span style={{ fontSize: '3rem', fontWeight: 900, color: 'var(--text-main)', lineHeight: 1 }}>${plan.planPrice}</span>
                  <span style={{ color: 'var(--text-dim)', fontWeight: 700, fontSize: '0.75rem', textTransform: 'uppercase' }}>/month</span>
                </div>

                <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem', marginBottom: '2.5rem' }}>
                  {["Full Gym Access", "Custom Nutrition", "24/7 Support"].map(f => (
                    <div key={f} style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', fontSize: '0.75rem', fontWeight: 700, color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                      <Check className="text-success" size={16} />
                      {f}
                    </div>
                  ))}
                </div>
              </div>
              
              <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
                <div className="form-group" style={{ marginBottom: 0 }}>
                  <label className="detail-label" style={{ marginBottom: '0.75rem' }}>Subscription Period</label>
                  <div className="select-wrapper">
                    <Calendar className="form-input-icon" size={18} />
                    <select 
                      value={selectedDurations[plan.planID] || 1}
                      onChange={(e) => handleDurationChange(plan.planID, Number(e.target.value))}
                      disabled={isActive || processingId === plan.planID}
                      className="input-field"
                      style={{ paddingLeft: '3rem' }}
                    >
                      <option value={1}>1 Month — ${plan.planPrice * 1}</option>
                      <option value={3}>3 Months — ${plan.planPrice * 3}</option>
                      <option value={6}>6 Months — ${plan.planPrice * 6}</option>
                      <option value={12}>12 Months — ${plan.planPrice * 12}</option>
                    </select>
                    <div className="select-arrow">
                      <ArrowRight size={16} style={{ transform: 'rotate(90deg)' }} />
                    </div>
                  </div>
                </div>

                <button
                  onClick={() => handleRenew(plan.planID)}
                  disabled={isActive || processingId === plan.planID}
                  className="btn-premium"
                  style={{ width: '100%', padding: '1rem', opacity: isActive ? 0.3 : 1 }}
                >
                  {processingId === plan.planID ? (
                    <span className="btn-loading">
                      <Loader2 className="spinner" size={18} />
                      Processing...
                    </span>
                  ) : (
                    isActive ? "Membership Locked" : "Select This Plan"
                  )}
                </button>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default MembershipPage;