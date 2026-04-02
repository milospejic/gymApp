import { useState } from "react";
import { authService } from "../services/authService";
import { useNavigate, Link } from "react-router-dom";
import { Eye, EyeOff, Mail, Lock, LogIn, ArrowRight, Loader2 } from "lucide-react"; 
import { useAuth } from "../contexts/AuthContext";
import "./Forms.css";

const LoginForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setIsSubmitting(true);

    try {
      const response = await authService.login({ email, password });
      login(response.accessToken, response.refreshToken, response.role);
      navigate("/");
    } catch (err) {
      setError("Invalid email or password. Please try again.");
      console.error(err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="form-container">
      <div className="glass-card form-card">
        <div className="form-header-glow" />
        
        <div className="form-header">
          <div className="form-icon-wrapper">
            <LogIn size={32} />
          </div>
          <h2 className="form-title font-black uppercase tracking-tighter">Welcome Back</h2>
          <p className="form-subtitle">Sign in to your beast account</p>
        </div>
        
        {error && (
          <div className="form-error">
            <div style={{ width: '8px', height: '8px', borderRadius: '50%', background: 'var(--error)' }} />
            {error}
          </div>
        )}
        
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <div className="form-label-row">
              <label className="form-label">Email Address</label>
            </div>
            <div className="form-input-wrapper">
              <Mail className="form-input-icon" size={20} />
              <input
                type="email"
                className="input-field"
                placeholder="beast@example.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
          </div>
          
          <div className="form-group">
            <div className="form-label-row">
              <label className="form-label">Password</label>
              <button type="button" className="form-label" style={{ color: 'var(--primary)', cursor: 'pointer' }}>Forgot?</button>
            </div>
            <div className="form-input-wrapper">
              <Lock className="form-input-icon" size={20} />
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
                {showPassword ? <EyeOff size={20} /> : <Eye size={20} />}
              </button>
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
                Authenticating...
              </span>
            ) : (
              <span className="btn-loading">
                Login to Dashboard <ArrowRight size={20} />
              </span>
            )}
          </button>
        </form>

        <div className="form-footer">
          <p className="text-dim font-medium uppercase tracking-wider" style={{ fontSize: '0.8125rem' }}>
            Don't have an account?{" "}
            <Link to="/register" className="form-link">
              Create One
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default LoginForm;