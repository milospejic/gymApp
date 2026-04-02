import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { Eye, EyeOff, User, Mail, Phone, Lock, Calendar, ClipboardCheck, ArrowRight, UserPlus, Loader2 } from "lucide-react";
import { memberService } from "../services/memberService";
import { membershipPlanService } from "../services/membershipPlanService";
import { MembershipPlan } from "../interfaces";
import "./Forms.css";

const RegistrationForm = () => {
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [phone, setPhone] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [planDuration, setPlanDuration] = useState(1);
  const [membershipPlans, setMembershipPlans] = useState<MembershipPlan[]>([]);
  const [selectedPlan, setSelectedPlan] = useState<string>("");

  const [error, setError] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchMembershipPlans = async () => {
      try {
        const plans = await membershipPlanService.getAllPlans();
        setMembershipPlans(plans);
        if (plans.length > 0) setSelectedPlan(plans[0].planID);
      } catch (err) {
        console.error("Error fetching membership plans:", err);
        setError("Failed to load membership plans.");
      }
    };

    fetchMembershipPlans();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setIsSubmitting(true);

    const memberData = {
      memberName: name,
      memberSurname: surname,
      memberEmail: email,
      memberPhone: phone,
      memberHashedPassword: password,
      membership: {
        planDuration,
        membershipPlanId: selectedPlan,
      },
    };

    try {
      await memberService.createMember(memberData);
      navigate("/login");
    } catch (err) {
      setError("Registration failed. Please check your data and try again.");
      console.error("Registration error:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="form-container" style={{ maxWidth: '600px' }}>
      <div className="glass-card form-card">
        <div className="form-header-glow" />
        
        <div className="form-header">
          <div className="form-icon-wrapper">
            <UserPlus size={32} />
          </div>
          <h2 className="form-title font-black uppercase tracking-tighter">Join The Pack</h2>
          <p className="form-subtitle">Start your fitness journey today</p>
        </div>

        {error && (
          <div className="form-error">
            <div style={{ width: '8px', height: '8px', borderRadius: '50%', background: 'var(--error)' }} />
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          {/* Name & Surname */}
          <div className="grid-2">
            <div className="form-group">
              <label className="form-label">First Name</label>
              <div className="form-input-wrapper">
                <User className="form-input-icon" size={18} />
                <input
                  type="text"
                  className="input-field"
                  placeholder="John"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                />
              </div>
            </div>
            <div className="form-group">
              <label className="form-label">Last Name</label>
              <div className="form-input-wrapper">
                <User className="form-input-icon" size={18} />
                <input
                  type="text"
                  className="input-field"
                  placeholder="Doe"
                  value={surname}
                  onChange={(e) => setSurname(e.target.value)}
                  required
                />
              </div>
            </div>
          </div>

          {/* Email & Phone */}
          <div className="grid-2">
            <div className="form-group">
              <label className="form-label">Email Address</label>
              <div className="form-input-wrapper">
                <Mail className="form-input-icon" size={18} />
                <input
                  type="email"
                  className="input-field"
                  placeholder="john@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>
            </div>
            <div className="form-group">
              <label className="form-label">Phone Number</label>
              <div className="form-input-wrapper">
                <Phone className="form-input-icon" size={18} />
                <input
                  type="text"
                  className="input-field"
                  placeholder="06x xxx xxxx"
                  value={phone}
                  onChange={(e) => setPhone(e.target.value)}
                  required
                />
              </div>
            </div>
          </div>

          {/* Password */}
          <div className="form-group">
            <label className="form-label">Password</label>
            <div className="form-input-wrapper">
              <Lock className="form-input-icon" size={18} />
              <input
                type={showPassword ? "text" : "password"}
                className="input-field input-field-with-eye"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <button
                type="button"
                className="password-toggle"
                onClick={() => setShowPassword(!showPassword)}
              >
                {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
              </button>
            </div>
          </div>

          {/* Membership Selection */}
          <div className="grid-2">
            <div className="form-group">
              <label className="form-label">Duration</label>
              <div className="select-wrapper">
                <Calendar className="form-input-icon" size={18} />
                <select
                  className="input-field"
                  value={planDuration}
                  onChange={(e) => setPlanDuration(Number(e.target.value))}
                >
                  <option value={1}>1 Month</option>
                  <option value={3}>3 Months</option>
                  <option value={6}>6 Months</option>
                  <option value={12}>12 Months</option>
                </select>
                <div className="select-arrow">
                  <svg className="fill-current h-4 w-4" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z"/></svg>
                </div>
              </div>
            </div>
            <div className="form-group">
              <label className="form-label">Select Plan</label>
              <div className="select-wrapper">
                <ClipboardCheck className="form-input-icon" size={18} />
                <select
                  className="input-field"
                  value={selectedPlan}
                  onChange={(e) => setSelectedPlan(e.target.value)}
                  required
                >
                  {membershipPlans.map((plan) => (
                    <option key={plan.planID} value={plan.planID}>
                      {plan.planName} (${plan.planPrice})
                    </option>
                  ))}
                </select>
                <div className="select-arrow">
                  <svg className="fill-current h-4 w-4" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z"/></svg>
                </div>
              </div>
            </div>
          </div>

          <button 
            type="submit" 
            disabled={isSubmitting}
            className="btn-premium w-full"
            style={{ padding: '1rem', fontSize: '1.125rem', marginTop: '1rem' }}
          >
            {isSubmitting ? (
              <span className="btn-loading">
                <Loader2 className="spinner" size={20} />
                Processing...
              </span>
            ) : (
              <span className="btn-loading">
                Create Account <ArrowRight size={20} />
              </span>
            )}
          </button>
        </form>

        <div className="form-footer">
          <p className="text-dim font-medium uppercase tracking-wider" style={{ fontSize: '0.8125rem' }}>
            Already a member?{" "}
            <Link to="/login" className="form-link">
              Sign In
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default RegistrationForm;