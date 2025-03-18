import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Eye, EyeOff } from "lucide-react";
import { memberService } from "../services/memberService";
import { membershipPlanService, MembershipPlan } from "../services/membershipPlanService";

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
  const navigate = useNavigate();

  useEffect(() => {
    const fetchMembershipPlans = async () => {
      try {
        const plans = await membershipPlanService.getMembershipPlans();
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
      navigate("/");
    } catch (err) {
      setError("Registration failed. Please try again.");
      console.error("Registration error:", err);
    }
  };

  return (
    <div className="container d-flex justify-content-center align-items-center">
      <div
        className="card p-4 shadow-lg"
        style={{
          width: "100%",
          maxWidth: "500px",
          maxHeight: "90vh",
          overflowY: "auto",
        }}
      >
        <h3 className="text-center mb-3">Register</h3>

        {error && <div className="alert alert-danger">{error}</div>}

        <form onSubmit={handleSubmit}>
          {/* Name & Surname in One Row */}
          <div className="row mb-2">
            <div className="col">
              <input
                type="text"
                className="form-control"
                placeholder="First Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
              />
            </div>
            <div className="col">
              <input
                type="text"
                className="form-control"
                placeholder="Last Name"
                value={surname}
                onChange={(e) => setSurname(e.target.value)}
                required
              />
            </div>
          </div>

          {/* Email & Phone in One Row */}
          <div className="row mb-2">
            <div className="col">
              <input
                type="email"
                className="form-control"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div className="col">
              <input
                type="text"
                className="form-control"
                placeholder="Phone"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                required
              />
            </div>
          </div>

          {/* Password with Show/Hide Button */}
          <div className="mb-2">
            <div className="input-group">
              <input
                type={showPassword ? "text" : "password"}
                className="form-control"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <button
                type="button"
                className="btn btn-outline-secondary"
                onClick={() => setShowPassword(!showPassword)}
              >
                {showPassword ? <EyeOff size={20} /> : <Eye size={20} />}
              </button>
            </div>
          </div>

          {/* Plan Duration & Membership Plan */}
          <div className="row mb-3">
            <div className="col">
              <select
                className="form-select"
                value={planDuration}
                onChange={(e) => setPlanDuration(Number(e.target.value))}
              >
                <option value={1}>1 Month</option>
                <option value={3}>3 Months</option>
                <option value={6}>6 Months</option>
                <option value={12}>12 Months</option>
              </select>
            </div>
            <div className="col">
              <select
                className="form-select"
                value={selectedPlan}
                onChange={(e) => setSelectedPlan(e.target.value)}
                required
              >
                {membershipPlans.map((plan) => (
                  <option key={plan.planID} value={plan.planID}>
                    {plan.planName} - ${plan.planPrice}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <button type="submit" className="btn btn-primary w-100">
            Register
          </button>
        </form>
      </div>
    </div>
  );
};

export default RegistrationForm;
